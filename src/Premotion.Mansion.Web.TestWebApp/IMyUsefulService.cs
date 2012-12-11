using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.TestWebApp
{
	/// <summary>
	/// Provides a useful service.
	/// </summary>
	public interface IMyUsefulService
	{
		/// <summary>
		/// Retrieves the latest message
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		IPropertyBag RetrieveLatestMessage(IMansionContext context, string username);
		/// <summary>
		/// Retrieves the all messages.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		Dataset RetrieveMessages(IMansionContext context, string username);
	}
	/// <summary>
	/// Provides cool messages
	/// </summary>
	public class CoolAndUsefulService : IMyUsefulService
	{
		#region Implementation of IMyUsefulService
		/// <summary>
		/// Retrieves the latest message
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public IPropertyBag RetrieveLatestMessage(IMansionContext context, string username)
		{
			return new PropertyBag
			       {
			       	{"message", string.Format("Wazzup {0}?", username)}
			       };
		}
		/// <summary>
		/// Retrieves the all messages.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"> </param>
		/// <returns></returns>
		public Dataset RetrieveMessages(IMansionContext context, string username)
		{
			var dataset = new Dataset();
			dataset.AddRow(new PropertyBag
			               {
			               	{"message", string.Format("Friends are like street lights, Along the road, they don't make the distance any shorter {0}.", username)}
			               });
			dataset.AddRow(new PropertyBag
			               {
			               	{"message", string.Format("On this auspicious Festival of lights May the glow of joys Prosperity and happiness Alluminate {0}.", username)}
			               });
			return dataset;
		}
		#endregion
	}
	/// <summary>
	/// Provides cool messages
	/// </summary>
	public class PirateAndUsefulService : IMyUsefulService
	{
		#region Implementation of IMyUsefulService
		/// <summary>
		/// Retrieves the latest message
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public IPropertyBag RetrieveLatestMessage(IMansionContext context, string username)
		{
			return new PropertyBag
			       {
			       	{"message", string.Format("Yo-ho-ho {0}", username)}
			       };
		}
		/// <summary>
		/// Retrieves the all messages.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="username"> </param>
		/// <returns></returns>
		public Dataset RetrieveMessages(IMansionContext context, string username)
		{
			var dataset = new Dataset();
			dataset.AddRow(new PropertyBag
			               {
			               	{"message", string.Format("Stop wastin' me time an' put somethin' in th' box {0}!", username)}
			               });
			dataset.AddRow(new PropertyBag
			               {
			               	{"message", string.Format("What else ye got {0}? An' be quick about it, I be shippin' out soon!", username)}
			               });
			return dataset;
		}
		#endregion
	}
}