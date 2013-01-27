using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Provides an interface to read <see cref="IPropertyBag"/>s from a streaming source.
	/// </summary>
	public interface IPropertyBagReader : IEnumerable<IPropertyBag>, IDisposable
	{
	}
}