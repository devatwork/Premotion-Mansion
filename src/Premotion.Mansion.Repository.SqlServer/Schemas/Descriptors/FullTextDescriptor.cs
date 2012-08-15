using System;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors
{
	/// <summary>
	/// Describes the full text columns.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "fullText")]
	public class FullTextDescriptor : TypeDescriptor
	{
		/// <summary>
		/// Populates the fullText property.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="modifiedProperties">The modified <see cref="IPropertyBag"/>.</param>
		/// <param name="originalProperties">The original <see cref="IPropertyBag"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public void Populate(IMansionContext context, IPropertyBag modifiedProperties, IPropertyBag originalProperties)
		{
			//validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (modifiedProperties == null)
				throw new ArgumentNullException("modifiedProperties");
			if (originalProperties == null)
				throw new ArgumentNullException("originalProperties");

			// get all the properties over which to loop
			var properties = Properties.Get<string>(context, "properties").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(property => property.Trim());

			// assemble the content
			var buffer = new StringBuilder();
			foreach (var property in properties)
			{
				//  get the content for the given property
				String content;
				if (!modifiedProperties.TryGet(context, property, out content))
				{
					if (!originalProperties.TryGet(context, property, out content))
						continue;
				}

				// strip the HTML
				content = content.StripHtml();

				// add it to the buffer
				buffer.AppendLine(content);
			}

			// if there is full-text content, add it to the full-text property, otherwise set it to null
			modifiedProperties.Set("fullText", buffer.Length > 0 ? buffer.ToString() : null);
		}
	}
}