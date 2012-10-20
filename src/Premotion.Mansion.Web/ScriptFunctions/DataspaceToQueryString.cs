using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Conversion;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.ScriptFunctions
{
	/// <summary>
	/// Turns a dataspace into a query string.
	/// </summary>
	[ScriptFunction("DataspaceToQueryString")]
	public class DataspaceToQueryString : FunctionExpression
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conversionService"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public DataspaceToQueryString(IConversionService conversionService)
		{
			//validate arguments
			if (conversionService == null)
				throw new ArgumentNullException("conversionService");

			// set values
			this.conversionService = conversionService;
		}
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Changes the value of an attribute in the query string.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="url">The <see cref="Url"/> which to change.</param>
		/// <param name="dataspace">The <see cref="IPropertyBag"/> which to add to the query string.</param>
		/// <returns>The <see cref="Url"/> with the modified query string.</returns>
		public Url Evaluate(IMansionContext context, Url url, IPropertyBag dataspace)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (url == null)
				throw new ArgumentNullException("url");
			if (dataspace == null)
				throw new ArgumentNullException("dataspace");

			var modifiableUri = url.Clone();

			// merge the dataspace
			foreach (var entry in dataspace)
				modifiableUri.QueryString[entry.Key] = conversionService.Convert<string>(context, entry.Value);

			// return the url
			return modifiableUri;
		}
		#endregion
		#region Private Fields
		private readonly IConversionService conversionService;
		#endregion
	}
}