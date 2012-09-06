namespace Premotion.Mansion.Core.Patterns.Specifications
{
	/// <summary>
	/// Represents the base interface of the specification pattern.
	/// </summary>
	/// <typeparam name="TSubject">The type of subject against which to check this specification.</typeparam>
	/// <typeparam name="TResult">The type of result returned from the specification check.</typeparam>
	public interface ISpecification<in TSubject, out TResult>
	{
		#region Satisfies Methods
		/// <summary>
		/// Checks whether the given <paramref name="subject"/> satisfies this specification.
		/// </summary>
		/// <param name="subject">The subject which to check against this specification.</param>
		/// <returns>Returns the result of this check.</returns>
		TResult IsSatisfiedBy(TSubject subject);
		#endregion
	}
}