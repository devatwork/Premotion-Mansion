using Premotion.Mansion.Core.Nucleus;

namespace Premotion.Mansion.Core.Conversion
{
	/// <summary>
	/// Defines the public interface for conversion services.
	/// </summary>
	public interface IConversionService : IService
	{
		#region Conversion Methods
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <returns>Returns the converted object.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		TTarget Convert<TTarget>(IContext context, object source);
		/// <summary>
		/// Converts the source object into the desired target type.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <typeparam name="TTarget">The target type.</typeparam>
		/// <param name="source">The source object.</param>
		/// <param name="defaultValue">The default value returned when the conversion can not be found.</param>
		/// <returns>Returns the converted object or <paramref name="defaultValue"/>.</returns>
		/// <exception cref="NoConverterFoundException">Thrown when there is no convert for <typeparamref name="TTarget"/>.</exception>
		TTarget Convert<TTarget>(IContext context, object source, TTarget defaultValue);
		#endregion
		#region Comparision Methods
		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="left">The left-hand object.</param>
		/// <param name="right">The right-hand object.</param>
		/// <returns>Returns 0 when the objects are equal, greater than zero when <paramref name="left"/> is greater than <paramref name="right"/>, or less than zero when <paramref name="left"/> is smaller than <paramref name="right"/>.</returns>
		int Compare(IContext context, object left, object right);
		#endregion
	}
}