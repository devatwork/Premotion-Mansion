using System;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Converts <see cref="string"/>s into <see cref="ITypeDefinition"/>s.
	/// </summary>
	public class StringToTypeDefinitionConverter : ConverterBase<string, ITypeDefinition>
	{
		#region Overrides of ConverterBase<string,ITypeDefinition>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override ITypeDefinition DoConvert(IMansionContext context, string source, Type sourceType)
		{
			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// load the type
			return typeService.Load(context, source);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override ITypeDefinition DoConvert(IMansionContext context, string source, Type sourceType, ITypeDefinition defaultValue)
		{
			try
			{
				return DoConvert(context, source, sourceType);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		#endregion
	}
}