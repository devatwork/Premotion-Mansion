﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Web.Templating.Html.Encoders
{
	/// <summary>
	/// Html Encodes an attribute value.
	/// </summary>
	[ScriptFunction(FunctionName)]
	public class AntiXssHtmlAttributeEncode : FunctionExpression
	{
		#region Constants
		/// <summary>
		/// Defines the function name of this encoder.
		/// </summary>
		public const string FunctionName = "AntiXssHtmlAttributeEncode";
		#endregion
		#region Evaluate Methods
		/// <summary>
		/// Html Encodes an attribute value.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="input">The content which to encode.</param>
		/// <returns>Returns the encoded value.</returns>
		public string Evaluate(IMansionContext context, string input)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// return the encoded version
			return input == null ? string.Empty : input.HtmlAttributeEncode();
		}
		#endregion
	}
}