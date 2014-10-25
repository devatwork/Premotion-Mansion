using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Scheduler;

namespace Premotion.Mansion.Web.TestWebApp.Web.Types.ExampleJob
{
	public class ExampleTask : Task
	{
		public override void DoExecute(IMansionContext context, Record record)
		{
			// do something
		}
	}

	public class SecondExampleTask : Task
	{
		public override void DoExecute(IMansionContext context, Record record)
		{
			// do something
		}
	}
}