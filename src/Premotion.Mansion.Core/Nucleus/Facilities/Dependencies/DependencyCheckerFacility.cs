using System.Linq;
using Premotion.Mansion.Core.Nucleus.Extension;

namespace Premotion.Mansion.Core.Nucleus.Facilities.Dependencies
{
	/// <summary>
	/// This facility checks whether all the dependencies of a service are available in the nucleus before starting the service.
	/// </summary>
	public class DependencyCheckerFacility : FacilityBase
	{
		#region Overrides of FacilityBase
		/// <summary>
		/// Activates this facility in the <paramref name="nucleus"/>. This event it typically used to register listeners to nucleus events.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="nucleus">The <see cref="IExtendedNucleus"/> in which this facility is activated.</param>
		protected override void DoActivate(IContext context, IExtendedNucleus nucleus)
		{
			// check starting services for IServiceWithDependencies
			nucleus.ServiceActivating += (ctx, model) =>
			                             {
			                             	// check if the service does not implement the IServiceWithDependencies
			                             	var serviceWithDependencies = model.ImplementationType as IServiceWithDependencies;
			                             	if (serviceWithDependencies == null)
			                             		return;

			                             	// get all the dependend services
			                             	foreach (var dependentServiceContract in serviceWithDependencies.Dependencies.Dependencies.Select(x => x.ContractType))
			                             	{
			                             		try
			                             		{
			                             			nucleus.Get(ctx, dependentServiceContract);
			                             		}
			                             		catch (NoServiceFoundException)
			                             		{
			                             			throw new MissingDependencyException(model.ImplementationType, dependentServiceContract);
			                             		}
			                             	}
			                             };
		}
		#endregion
	}
}