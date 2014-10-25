using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Quartz;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task : IJob
	{
		public abstract void DoExecute(IMansionContext context, Record record);

		public void Execute(IJobExecutionContext context)
		{
			var dataMap = context.MergedJobDataMap;
			var mansionContext = (IMansionContext)dataMap["context"];
			var record = (Record)dataMap["record"];
			DoExecute(mansionContext, record);
		}
	}
}