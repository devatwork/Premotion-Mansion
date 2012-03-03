using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Core.Scripting.ExpressionScript
{
	/// <summary>
	/// Implements <see cref="CachedObject{TObject}"/> for <see cref="TagScript"/>.
	/// </summary>
	public class CachedPhrase : CachedObject<IExpressionScript>
	{
		#region Constructors
		/// <summary>
		/// Constructs a new cached object.
		/// </summary>
		/// <param name="obj">The object which to cache.</param>
		public CachedPhrase(IExpressionScript obj) : base(obj)
		{
			Priority = Priority.NotRemovable;
		}
		#endregion
	}
}