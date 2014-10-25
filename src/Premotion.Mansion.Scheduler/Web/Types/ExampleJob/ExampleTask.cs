using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Scheduler.Web.Types.ExampleJob
{
	public class ExampleTask : Task
	{
		public override void DoExecute(IMansionContext context, Record record)
		{
			// do something
		}
	}
}