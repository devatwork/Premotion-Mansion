using System;
using System.Globalization;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;

namespace Premotion.Mansion.Web.Controls.Forms.Engines
{
	/// <summary>
	/// Provides the default implementation of <see cref="FormEngine"/>.
	/// </summary>
	public class DefaultFormEngine : FormEngine
	{
		#region Constructors
		/// <summary>
		/// Constructs a standard form engine.
		/// </summary>
		/// <param name="dataSource">The default data source.</param>
		public DefaultFormEngine(IPropertyBag dataSource)
		{
			// set values
			this.dataSource = dataSource;
		}
		#endregion
		#region Overrides of FormEngine
		/// <summary>
		/// Loads the <see cref="FormState"/> of a particular <paramref name="form"/> from the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to load the state.</param>
		/// <returns>Returns the loaded <see cref="FormState"/>.</returns>
		protected override FormState DoLoadState(MansionWebContext context, Form form)
		{
			// load the properties from the data source
			var fieldProperties = new PropertyBag();
			var formProperties = new PropertyBag();
			var action = string.Empty;
			foreach (var candidate in context.Stack.Peek<IPropertyBag>("Post").Where(candidate => candidate.Key.StartsWith(form.Prefix, StringComparison.OrdinalIgnoreCase)))
			{
				// check for field property
				if (candidate.Key.StartsWith(form.FieldPrefix, StringComparison.OrdinalIgnoreCase))
					fieldProperties.Add(candidate.Key.Substring(form.FieldPrefix.Length), candidate.Value);
					// check for action property
				else if (candidate.Key.StartsWith(form.ActionPrefix, StringComparison.OrdinalIgnoreCase))
					action = candidate.Value.ToString();
				else
					formProperties.Add(candidate.Key.Substring(form.Prefix.Length), candidate.Value);
			}

			// determine the current step
			var currentStepId = formProperties.Get(context, "current-step", context.Stack.Peek<IPropertyBag>("Get").Get(context, form.Prefix + "current-step", 0));
			var currentStep = form.Steps.ElementAtOrDefault(currentStepId) ?? form.Steps.First();

			// determine the next set
			var nextStep = form.Steps.ElementAtOrDefault(currentStepId + 1) ?? form.Steps.Last();

			// create the state
			return new FormState
			       {
			       	FieldProperties = fieldProperties,
			       	FormInstanceProperties = formProperties,
			       	DataSource = dataSource,
			       	CurrentAction = action,
			       	IsPostback = !string.IsNullOrEmpty(action) || formProperties.Count > 0,
			       	CurrentStep = currentStep,
			       	NextStep = nextStep
			       };
		}
		/// <summary>
		/// Stores the <see cref="FormState"/> of a particular <paramref name="form"/> in the <paramref name="context"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to store the state.</param>
		/// <param name="state">The <see cref="FormState"/> which to save.</param>
		protected override void DoStoreState(MansionWebContext context, Form form, FormState state)
		{
			// do nothing
		}
		/// <summary>
		/// Advances the <paramref name="form"/> to the <paramref name="step"/>.
		/// </summary>
		/// <param name="context">The <see cref="MansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being advanced.</param>
		/// <param name="step">The <see cref="Step"/> to which to advance.</param>
		protected override void DoAdvanceTo(MansionWebContext context, Form form, Step step)
		{
			// build the new URL
			var url = new ParameterizedUri(context.HttpContext.Request.Url).SetParameter(form.Prefix + "current-step", (form.Steps.ToList().IndexOf(step)).ToString(CultureInfo.InvariantCulture));

			// copy all non form field properties to the query string
			foreach (var parameter in context.Stack.Peek<IPropertyBag>("Post").Where(candidate => !candidate.Key.StartsWith(form.Prefix, StringComparison.OrdinalIgnoreCase)))
				url.SetParameter(parameter.Key, parameter.Value != null ? parameter.Value.ToString() : string.Empty);

			// set redirect
			WebUtilities.RedirectRequest(context, url);
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag dataSource;
		#endregion
	}
}