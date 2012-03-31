using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Web.Portal.Descriptors
{
	/// <summary>
	/// Implements the descriptor for layouts.
	/// </summary>
	[TypeDescriptor(Constants.DescriptorNamespaceUri, "columnSchema")]
	public class ColumnSchemaDescriptor : TypeDescriptor
	{
		#region Schema Methods
		/// <summary>
		/// Gets the <see cref="ColumnSchema"/>.
		/// </summary>
		/// <param name="context">The request context.</param>
		/// <returns>Returns the <see cref="ColumnSchema"/>.</returns>
		public ColumnSchema GetSchema(IMansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get the columns
			var columns = Properties.Get<string>(context, "columns");

			return new ColumnSchema(columns.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries));
		}
		#endregion
	}
}