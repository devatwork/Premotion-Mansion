using System;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Conversion.Converters
{
	/// <summary>
	/// Handles the conversion from <see cref="NodePointer"/> to <see cref="ITypeDefinition"/>.
	/// </summary>
	public class NodeToNodePointerConverter : ConverterBase<Node, NodePointer>
	{
		#region Overrides of ConverterBase<Node,NodePointer>
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IContext context, Node source, Type sourceType)
		{
			return source.Pointer;
		}
		/// <summary>
		/// Converts the object to <see cref="IConverter.TargetType"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="source">The input value.</param>
		/// <param name="sourceType">The actual type of the source.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Returns the converted value.</returns>
		protected override NodePointer DoConvert(IContext context, Node source, Type sourceType, NodePointer defaultValue)
		{
			return source.Pointer;
		}
		#endregion
	}
}