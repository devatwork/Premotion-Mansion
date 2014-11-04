using System.Text;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Scheduler
{
	public interface ITask
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobProperties"></param>
		/// <param name="taskOutput"></param>
		/// <returns>Boolean indicating if the task ran successfully</returns>
		bool DoExecute(IMansionContext context, IPropertyBag jobProperties, ref StringBuilder taskOutput);
	}
}