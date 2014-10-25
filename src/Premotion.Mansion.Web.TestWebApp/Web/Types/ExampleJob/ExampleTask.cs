using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Scheduler;

namespace Premotion.Mansion.Web.TestWebApp.Web.Types.ExampleJob
{
	public class ExampleTask : Task
	{
		public override void DoExecute(IMansionContext context, Node jobNode)
		{
			// Russian roulette
			// To demonstrate error handling, sometimes this task will generate an 'Attempted to divide by zero' error.
			var rnd = new Random();
			int a = rnd.Next(1,10);
			int b = rnd.Next(3);
			int c = a/b;
		}
	}

	public class SecondExampleTask : Task
	{
		public override void DoExecute(IMansionContext context, Node jobNode)
		{
			
		}
	}
}