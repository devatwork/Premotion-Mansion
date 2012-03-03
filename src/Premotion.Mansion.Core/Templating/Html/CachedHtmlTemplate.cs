using Premotion.Mansion.Core.Caching;

namespace Premotion.Mansion.Core.Templating.Html
{
	/// <summary>
	/// Implements <see cref="CachedObject{TObject}"/> for <see cref="Template"/>.
	/// </summary>
	public class CachedHtmlTemplate : CachedObject<Template>
	{
		#region Constructors
		/// <summary>
		/// Constructs a new cached object.
		/// </summary>
		/// <param name="obj">The object which to cache.</param>
		public CachedHtmlTemplate(Template obj) : base(obj)
		{
			Priority = Priority.NotRemovable;
		}
		#endregion
	}
}