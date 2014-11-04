using System;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Scheduler;

namespace Premotion.Mansion.Web.TestWebApp.Web.Types.ExampleJob
{
	public class ExampleTask : Task
	{
		public override bool DoExecute(IMansionContext context, IPropertyBag jobProperties, ref StringBuilder taskOutput)
		{
			// To demonstrate error handling, the next calculation could generate an 'Attempted to divide by zero' error.
			var rnd = new Random();
			int a = rnd.Next(1, 10);
			int b = rnd.Next(3);
			double c = a / b;

			taskOutput.Append(String.Format("Successfully divided {0} by {1}. Result: {2}", a, b, c));
			return true;
		}
	}

	public class FailingTask : Task
	{
		public override bool DoExecute(IMansionContext context, IPropertyBag jobProperties, ref StringBuilder taskOutput)
		{
			throw new ApplicationException("something went terribly wrong");
		}
	}
}