using System;
using System.Collections.Generic;
using System.ComponentModel;
using dotless.Core.Plugins;

namespace Premotion.Mansion.Web.DotLess
{
	/// <summary>
	/// Implements the mansion <see cref="IFunctionPlugin"/>.
	/// </summary>
	[Description("Registers Mansion specific functions"), DisplayName("MansionFunctions")]
	public class MansionFunctionsPlugin : IFunctionPlugin
	{
		#region Implementation of IFunctionPlugin
		/// <summary>
		/// Gets the plugin types.
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, Type> GetFunctions()
		{
			return plugins;
		}
		#endregion
		#region Private Fields
		private readonly Dictionary<string, Type> plugins = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
		                                                    {
		                                                    	{"StaticResourcePath", typeof (StaticResourcePathFunction)},
		                                                    	{"DynamicResourcePath", typeof (DynamicResourcePathFunction)}
		                                                    };
		#endregion
	}
}