using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.IO;

namespace Premotion.Mansion.Core.Templating
{
	/// <summary>
	/// Represents a single template with sections.
	/// </summary>
	public class Template : ITemplate
	{
		#region Constructors
		/// <summary>
		/// Constructs a new template.
		/// </summary>
		/// <param name="sections">The sections defined in this template.</param>
		/// <param name="path">The resource path from which this template is loaded.</param>
		public Template(IEnumerable<Section> sections, IResourcePath path)
		{
			// validate arguments
			if (sections == null)
				throw new ArgumentNullException("sections");
			if (path == null)
				throw new ArgumentNullException("path");

			// initialize all the sections
			sections = sections.ToList();
			foreach (var section in sections)
				section.Template = this;

			// set values
			this.sections = sections;
			Path = path;
		}
		#endregion
		#region Section Methods
		/// <summary>
		/// Tries to get the section with the name <paramref name="sectionName"/> from this template.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="sectionName">The name of the section.</param>
		/// <param name="section">The section when found.</param>
		/// <returns>Returns true when the section was found, otherwise false.</returns>
		public bool TryGet(IMansionContext context, string sectionName, out ISection section)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(sectionName))
				throw new ArgumentNullException("sectionName");

			// get the section
			section = sections.SingleOrDefault(candidate => sectionName.Equals(candidate.Name, StringComparison.OrdinalIgnoreCase));
			return section != null;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the path to the resource from which this template is loaded.
		/// </summary>
		public IResourcePath Path { get; private set; }
		#endregion
		#region Private Fields
		private readonly IEnumerable<ISection> sections;
		#endregion
	}
}