using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.Caching;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns.Tokenizing;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Templating.Html
{
	///<summary>
	/// Implements <see cref="ITemplateService"/> for HTML template.
	///</summary>
	public class HtmlTemplateService : ITemplateServiceInternal
	{
		#region Constructors
		/// <summary>
		/// Constructs the HTML template serivce.
		/// </summary>
		/// <param name="interpreters">The <see cref="IEnumerable{T}"/>s.</param>
		/// <param name="cachingService">The <see cref="ICachingService"/>.</param>
		public HtmlTemplateService(IEnumerable<SectionInterpreter> interpreters, ICachingService cachingService)
		{
			// validate arguments
			if (interpreters == null)
				throw new ArgumentNullException("interpreters");
			if (cachingService == null)
				throw new ArgumentNullException("cachingService");

			// set values
			this.interpreters = interpreters;
			this.cachingService = cachingService;
		}
		#endregion
		#region Open Methods
		/// <summary>
		/// Opens a template.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		public IDisposable Open(IMansionContext context, IEnumerable<IResource> resources)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The resource which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		public IDisposable Open(IMansionContext context, IResource resource)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (resource == null)
				throw new ArgumentNullException("resource");

			// open the template
			var cacheKey = ResourceCacheKey.Create(resource);
			var template = cachingService.GetOrAdd(
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
					                let interpreter = Election<IMansionContext, SectionInterpreter, string>.Elect(context, interpreters, rawSection)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		public IDisposable Render(IMansionContext context, string sectionName)
		{
			return Render(context, sectionName, null);
		}
		/// <summary>
		/// Renders the section with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		public IDisposable Render(IMansionContext context, string sectionName, string targetField)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="content">The content which to write.</param>
		/// <param name="targetField">The field to which to render.</param>
		public void RenderContent(IMansionContext context, string content, string targetField)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns the rendered content of the section.</returns>
		public string RenderToString(IMansionContext context, string sectionName)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName"></param>
		private static ISection FindSection(IMansionContext context, string sectionName)
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
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="targetField">The target field.</param>
		/// <returns>Returns the target field.</returns>
		private static IField FindTargetField(IMansionContext context, string targetField)
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
		/// Gets a flag indicating whether this template engine is writing to the top-most <see cref="IMansionContext.OutputPipe"/> or not.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns true when this template engine is writing to the top-most <see cref="IMansionContext.OutputPipe"/>, otherwise false.</returns>
		private static bool IsWritingToTopMostOutputPipe(IMansionContext context)
		{
			// find the top most OutputPipeTargetField
			var topMostOutputPipeTargetField = (OutputPipeTargetField) context.ActiveSectionStack.Select(x => x.TargetField).FirstOrDefault(x => x is OutputPipeTargetField);

			// check if the output pipe of the OutputPipeTargetField is the top-most output pipe
			return topMostOutputPipeTargetField != null && ReferenceEquals(topMostOutputPipeTargetField.OutputPipe, context.OutputPipe);
		}
		#endregion
		#region Private Fields
		private readonly ICachingService cachingService;
		private readonly IEnumerable<SectionInterpreter> interpreters;
		private readonly ITokenizer<string, string> sectionTokenizer = new HtmlTemplateTokenizer();
		#endregion
	}
}