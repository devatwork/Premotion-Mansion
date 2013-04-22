using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Provides helper methods for interacting with the linkbase.
	/// </summary>
	public static class LinkHelper
	{
		/// <summary>
		/// Safely copies the <see cref="Linkbase"/> from <paramref name="source"/> to <paramref name="target"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/>.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the arguments is null.</exception>
		public static void CopyLinkbase(IMansionContext context, IPropertyBag source, IPropertyBag target)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");

			LinkbaseData data;
			if (!source.TryGet(context, Constants.LinkbaseDataKey, out data))
				return;
			target.TrySet(Constants.LinkbaseDataKey, data);
		}
		/// <summary>
		/// Copies common properties from the <paramref name="source"/> and <paramref name="target"/> to the <paramref name="linkProperties"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="source">The source <see cref="IPropertyBag"/>.</param>
		/// <param name="target">The target <see cref="IPropertyBag"/>.</param>
		/// <param name="linkProperties">The linkProperties <see cref="IPropertyBag"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public static void CopyLinkEndsProperties(IMansionContext context, IPropertyBag source, IPropertyBag target, PropertyBag linkProperties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (source == null)
				throw new ArgumentNullException("source");
			if (target == null)
				throw new ArgumentNullException("target");
			if (linkProperties == null)
				throw new ArgumentNullException("linkProperties");

			// copy the most common properties
			var propertyNames = new[] {"id", "guid", "name", "type"};
			foreach (var propertyName in propertyNames)
			{
				linkProperties.Set("source" + propertyName, source.Get<object>(context, propertyName, null));
				linkProperties.Set("target" + propertyName, target.Get<object>(context, propertyName, null));
			}
		}
	}
}