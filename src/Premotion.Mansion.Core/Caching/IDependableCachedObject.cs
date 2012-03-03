namespace Premotion.Mansion.Core.Caching
{
	/// <summary>
	/// Marker interface indicating whether the <see cref="CachedObject{TObject}"/> is an dependency of other <see cref="CachedObject{TObject}"/>s.
	/// </summary>
	public interface IDependableCachedObject
	{
	}
}