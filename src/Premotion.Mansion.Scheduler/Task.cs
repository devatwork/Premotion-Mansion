using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Quartz;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task : IJob
	{
		public abstract void DoExecute(IMansionContext context, Node jobNode);

		public void Execute(IJobExecutionContext context)
		{
			var dataMap = context.MergedJobDataMap;
			var mansionContext = (IMansionContext)dataMap["context"];
			var record = (Node)dataMap["record"];

			try
			{
				using (RepositoryUtil.Open(mansionContext))
				{
					DoExecute(mansionContext, record);

					var editProperties = new PropertyBag
					{
						{"lastRunSuccessfull", true}
					};
					mansionContext.Repository.UpdateNode(mansionContext, record, editProperties);
				}
			}
			catch (Exception e)
			{
				// Log error
				using (RepositoryUtil.Open(mansionContext))
				{
					var editProperties = new PropertyBag
					{
						{"lastRunSuccessfull", false},
						{"exceptionMessage", e.Message}
					};
					mansionContext.Repository.UpdateNode(mansionContext, record, editProperties);
				}
			}
		}
	}
}