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



		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void Execute(IJobExecutionContext context)
		{
			var dataMap = context.MergedJobDataMap;
			var mansionContext = (IMansionContext) dataMap["context"];
			var record = (Node) dataMap["record"];

			var type = GetType();
			var typeName = type.Name;
			var editProperties = new PropertyBag();
			var taskOutput = new StringBuilder();

			using (RepositoryUtil.Open(mansionContext))
			{
				try
				{
					var ranSuccessfully = DoExecute(mansionContext, record, ref taskOutput);
					editProperties.Add(typeName + ".lastRunSuccessfull", ranSuccessfully);
					editProperties.Add(typeName + ".exceptionThrown", false);
				}
				catch (Exception e)
				{
					editProperties.Add(typeName + ".lastRunSuccessfull", false);
					editProperties.Add(typeName + ".exceptionThrown", true);
					editProperties.Add(typeName + ".exceptionMessage", e.Message);
				}
				finally
				{
					editProperties.Add(typeName + ".lastRun", DateTime.Now);
					editProperties.Add(typeName + ".taskOutput", taskOutput);
					mansionContext.Repository.UpdateNode(mansionContext, record, editProperties);
				}
			}
		}
	}
}