using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Nucleus.Facilities.Dependencies;
using Premotion.Mansion.Core.Nucleus.Facilities.Lifecycle;
using Premotion.Mansion.Core.Nucleus.Facilities.Reflection;
using Premotion.Mansion.Core.Patterns.Tokenizing;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Templating.Html
{
	///<summary>
	/// Implements <see cref="ITemplateService"/> for HTML template.
	///</summary>
	public class HtmlTemplateService : ManagedLifecycleService, ITemplateServiceInternal, IServiceWithDependencies
	{
		#region Open Methods
		/// <summary>
		/// Opens a template.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		public IDisposable Open(MansionContext context, IEnumerable<IResource> resources)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resources == null)
				throw new ArgumentNullException("resources");

			// create a stack group key
			var groupKey = new AutoStackKeyGroup();

			// loop through all the resourcem
			foreach (var resource in resources)
				groupKey.Push(Open(context, resource));

			return groupKey;
		}
		/// <summary>
		/// Opens a template.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="resource">The resource which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		public IDisposable Open(MansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// get the cache service
			var cacheKey = ResourceCacheKey.Create(resource);
			var cacheService = context.Nucleus.Get<ICachingService>(context);

			// open the template
			var template = cacheService.GetOrAdd(
				context,
				cacheKey,
				() =>
				{
					// read the complete template into memory
					string rawTemplate;
					using (var resourceStream = resource.OpenForReading())
					using (var reader = resourceStream.Reader)
						rawTemplate = reader.ReadToEnd();

					// loop through all the sections
					var sections = (from rawSection in sectionTokenizer.Tokenize(context, rawTemplate)
					                let interpreter = Election<MansionContext, SectionInterpreter, string>.Elect(context, interpreters, rawSection)
					                select interpreter.Interpret(context, rawSection)
					               ).ToList();

					// create the template
					var htmlTemplate = new Template(sections, resource.Path);

					// create the cached template
					return new CachedHtmlTemplate(htmlTemplate);
				});

			// push the template to the stack
			return context.TemplateStack.Push(template);
		}
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the section with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		public IDisposable Render(MansionContext context, string sectionName)
		{
			return Render(context, sectionName, null);
		}
		/// <summary>
		/// Renders the section with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		public IDisposable Render(MansionContext context, string sectionName, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException("sectionName");

			// find the section in the open templates
			var section = FindSection(context, sectionName);

			// find the target field
			var target = FindTargetField(context, string.IsNullOrEmpty(targetField) ? section.GetTargetField(context) : targetField);

			// return th active section
			var activeSection = new ActiveSection(this, section, target);

			// push the active section to the stack
			return context.ActiveSectionStack.Push(activeSection, x => x.FinalizeRendering(context));
		}
		/// <summary>
		/// Renders <paramref name="content"/> directory to the <paramref name="targetField"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="content">The content which to write.</param>
		/// <param name="targetField">The field to which to render.</param>
		public void RenderContent(MansionContext context, string content, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// check if there is no content to write
			if (string.IsNullOrEmpty(content))
				return;

			// find the target field
			var target = FindTargetField(context, targetField);

			// write the content
			target.Append(content);
		}
		/// <summary>
		/// Renders the section as a string.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns the rendered content of the section.</returns>
		public string RenderToString(MansionContext context, string sectionName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException("sectionName");

			// find the section in the open templates
			var section = FindSection(context, sectionName);
			if (!section.AreRequirementsSatified(context))
				return null;

			// create a buffer field
			var targetField = new StringBufferField();

			// create an active section
			var activeSection = new ActiveSection(this, section, targetField);

			// finish the rendering
			activeSection.FinalizeRendering(context);

			// return the content
			return targetField.Content;
		}
		#endregion
		#region Find Methods
		/// <summary>
		/// Finds a section on the template context stack.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="sectionName"></param>
		private static ISection FindSection(MansionContext context, string sectionName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException("sectionName");

			// loop through the template stack to find a template containing the section
			ISection section = null;
			if (!context.TemplateStack.Any(template => template.TryGet(context, sectionName, out section)))
			{
				// section not found
				throw new SectionNotFoundException(sectionName, context);
			}

			return section;
		}
		/// <summary>
		/// Gets the target field by it's name.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="targetField">The target field.</param>
		/// <returns>Returns the target field.</returns>
		private static IField FindTargetField(MansionContext context, string targetField)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(targetField))
				throw new ArgumentNullException("targetField");

			// if no active sections render to outputpipe or the target is an output field
			if (context.ActiveSectionStack.Count == 0 || !IsWritingToTopMostOutputPipe(context) || TemplateServiceConstants.OutputTargetField.Equals(targetField, StringComparison.OrdinalIgnoreCase))
				return new OutputPipeTargetField(context);

			// loop through the active sections to find a section containing the field
			foreach (var activeSection in context.ActiveSectionStack.Where(activeSection => activeSection.ContainsField(targetField)))
				return activeSection.GetField(targetField);

			throw new FieldNotFoundException(targetField, context);
		}
		/// <summary>
		/// Gets a flag indicating whether this template engine is writing to the top-most <see cref="MansionContext.OutputPipe"/> or not.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns true when this template engine is writing to the top-most <see cref="MansionContext.OutputPipe"/>, otherwise false.</returns>
		private static bool IsWritingToTopMostOutputPipe(MansionContext context)
		{
			// find the top most OutputPipeTargetField
			var topMostOutputPipeTargetField = (OutputPipeTargetField) context.ActiveSectionStack.Select(x => x.TargetField).FirstOrDefault(x => x is OutputPipeTargetField);

			// check if the output pipe of the OutputPipeTargetField is the top-most output pipe
			return topMostOutputPipeTargetField != null && ReferenceEquals(topMostOutputPipeTargetField.OutputPipe, context.OutputPipe);
		}
		#endregion
		#region Implementation of IStartableService
		/// <summary>
		/// Starts this service.
		/// </summary>
		/// <param name="context">The <see cref="INucleusAwareContext"/> in which this service is started.</param>
		protected override void DoStart(INucleusAwareContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the services
			var namingService = context.Nucleus.Get<ITypeDirectoryService>(context);
			var objectFactoryService = context.Nucleus.Get<IObjectFactoryService>(context);

			// get the section interpreters
			interpreters.AddRange(objectFactoryService.Create<SectionInterpreter>(namingService.Lookup<SectionInterpreter>()));
		}
		#endregion
		#region Implementation of IServiceWithDependencies
		/// <summary>
		/// Gets the <see cref="DependencyModel"/> of this service.
		/// </summary>
		public DependencyModel Dependencies
		{
			get { return dependencies; }
		}
		#endregion
		#region Private Fields
		private static readonly DependencyModel dependencies = new DependencyModel().Add<ICachingService>().Add<ITypeDirectoryService>().Add<IObjectFactoryService>();
		private readonly List<SectionInterpreter> interpreters = new List<SectionInterpreter>();
		private readonly ITokenizer<string, string> sectionTokenizer = new HtmlTemplateTokenizer();
		#endregion
	}
}