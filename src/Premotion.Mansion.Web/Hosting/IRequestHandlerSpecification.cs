using Premotion.Mansion.Core.Patterns.Specifications;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Marker interface for all  request handlers.
	/// </summary>
	public interface IRequestHandlerSpecification : ISpecification<IMansionWebContext, bool>
	{
	}
}