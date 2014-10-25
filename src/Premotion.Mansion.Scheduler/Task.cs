using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Quartz;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task : IJob
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobNode"></param>
		/// <param name="taskOutput"></param>
		/// <returns>Boolean indicating if the job ran successfully</returns>
		public abstract bool DoExecute(IMansionContext context, Node jobNode, ref StringBuilder taskOutput);



		public void Execute(IJobExecutionContext context)
		{
			var dataMap = context.MergedJobDataMap;
			var mansionContext = (IMansionContext) dataMap["context"];
			var record = (Node) dataMap["record"];

			var editProperties = new PropertyBag();
			var taskOutput = new StringBuilder();

			using (RepositoryUtil.Open(mansionContext))
			{
				try
				{
					var ranSuccessfully = DoExecute(mansionContext, record, ref taskOutput);
					editProperties.Add("lastRunSuccessfull", ranSuccessfully);
					editProperties.Add("exceptionThrown", false);
				}
				catch (Exception e)
				{
					editProperties.Add("lastRunSuccessfull", false);
					editProperties.Add("exceptionThrown", true);
					editProperties.Add("exceptionMessage", e.Message);
				}
				finally
				{
					editProperties.Add("lastRun", DateTime.Now);
					editProperties.Add("taskOutput", taskOutput);
					mansionContext.Repository.UpdateNode(mansionContext, record, editProperties);
				}
			}
		}
	}
}