using System;
using System.Collections.Generic;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a template service.
	/// </summary>
	public interface ITemplateService
	{
		#region Open Methods
		/// <summary>
		/// Opens a single template.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resource">The resource which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		/// <exception cref="ParseTemplateException">Thrown when <paramref name="resource"/> can not be parsed into a template.</exception>
		IDisposable Open(IMansionContext context, IResource resource);
		/// <summary>
		/// Opens several templates at the same time.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="resources">The resources which to open.</param>
		/// <returns>Returns a marker which will close the template automatically.</returns>
		/// <exception cref="ParseTemplateException">Thrown when one of the <paramref name="resources"/> can not be parsed into a template.</exception>
		IDisposable Open(IMansionContext context, IEnumerable<IResource> resources);
		#endregion
		#region Render Methods
		/// <summary>
		/// Renders the section with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		/// <exception cref="SectionNotFoundException">Thrown when no section with the name <paramref name="sectionName"/> could be found.</exception>
		/// <exception cref="FieldNotFoundException">Thrown when the field to which to render could not be found.</exception>
		IDisposable Render(IMansionContext context, string sectionName);
		/// <summary>
		/// Renders the section with the specified name.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section which to render.</param>
		/// <param name="targetField">The name of the field to which to render.</param>
		/// <returns>Returns a marker which will close the active section automatically.</returns>
		/// <exception cref="SectionNotFoundException">Thrown when no section with the name <paramref name="sectionName"/> could be found.</exception>
		/// <exception cref="FieldNotFoundException">Thrown when not field with the name <paramref name="targetField"/> could not be found.</exception>
		IDisposable Render(IMansionContext context, string sectionName, string targetField);
		/// <summary>
		/// Renders <paramref name="content"/> directory to the <paramref name="targetField"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="content">The content which to write.</param>
		/// <param name="targetField">The field to which to render.</param>
		void RenderContent(IMansionContext context, string content, string targetField);
		#endregion
	}
}