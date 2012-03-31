using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="Node"/> to <see cref="ITypeDefinition"/>.
	/// </summary>
	public class NodeToTypeDefinitionConverter : ConverterBase<Node, ITypeDefinition>
	{
		#region Overrides of ConverterBase<Node,ITypeDefinition>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override ITypeDefinition DoConvert(IMansionContext context, Node source, Type sourceType)
		{
			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// try to load the type
			ITypeDefinition typeDefinition;
			return typeService.TryLoad(context, (source).Pointer.Type, out typeDefinition) ? typeDefinition : typeService.LoadRoot(context);
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override ITypeDefinition DoConvert(IMansionContext context, Node source, Type sourceType, ITypeDefinition defaultValue)
		{
			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// try to load the type
			ITypeDefinition typeDefinition;
			return typeService.TryLoad(context, (source).Pointer.Type, out typeDefinition) ? typeDefinition : defaultValue;
		}
		#endregion
	}
}