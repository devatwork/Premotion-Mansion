﻿using System;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="int"/>s.
	/// </summary>
	public class StringToInt32Converter : ConverterBase<String, Int32>
	{
		#region Overrides of ConverterBase<string,int>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override int DoConvert(IContext context, string source, Type sourceType)
		{
			return System.Convert.ToInt32(source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override int DoConvert(IContext context, string source, Type sourceType, int defaultValue)
		{
			// try to convert
			try
			{
				return System.Convert.ToInt32(source);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}