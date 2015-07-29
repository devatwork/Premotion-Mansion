using System;
using System.Net.Mail;
using System.Text;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobProperties"></param>
		/// <param name="taskOutput"></param>
		/// <returns>Boolean indicating if the task ran successfully</returns>
		public abstract bool DoExecute(IMansionContext context, IPropertyBag jobProperties, ref StringBuilder taskOutput);

		public virtual void EnrichErrorReportEmail(MailMessage message, Exception exception)
		{
			// Can be overwritten if neccessary
		}
	}
}