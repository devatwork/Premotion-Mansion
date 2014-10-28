using System;
using System.Collections.Concurrent;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Nucleus;
using Quartz;

namespace Premotion.Mansion.Scheduler
{
	public abstract class Task : IJob
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="jobRecord"></param>
		/// <param name="taskOutput"></param>
		/// <returns>Boolean indicating if the job ran successfully</returns>
		public abstract bool DoExecute(IMansionContext context, Record jobRecord, ref StringBuilder taskOutput);

		private static readonly ConcurrentDictionary<int, object> JobLocks = new ConcurrentDictionary<int, object>();

		/// <summary>
		/// This method is called when the task is triggered to execute.
		/// </summary>
		/// <param name="jobContext"></param>
		public void Execute(IJobExecutionContext jobContext)
		{
			// Initialize and set context
			var dataMap = jobContext.MergedJobDataMap;
			var context = new MansionContext((INucleus) dataMap["nucleus"]);
			var jobNode = (Node)dataMap["jobNode"];
			var jobRecord = jobNode as Record;
			var type = GetType();
			var typeName = type.Name;
			JobLocks.TryAdd(jobRecord.Id, new object());

			var editProperties = new PropertyBag();
			var taskOutput = new StringBuilder();

			using (RepositoryUtil.Open(context))
			{
				try
				{
					var ranSuccessfully = DoExecute(context, jobRecord, ref taskOutput);
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
					editProperties.Add("_scheduleStatusUpdate", taskOutput);

					// Make sure no other task is editing the same JobNode at the same time
					var jobLock = JobLocks.GetOrAdd(jobRecord.Id, (key) => new object());
					lock (jobLock)
					{
						var repository = context.Repository;
						repository.UpdateNode(context, jobNode, editProperties);
					}
				}
			}
		}
	}
}