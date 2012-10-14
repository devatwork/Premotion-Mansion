using System;
using System.Globalization;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Conversion;

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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to load the state.</param>
		/// <returns>Returns the loaded <see cref="FormState"/>.</returns>
		protected override FormState DoLoadState(IMansionWebContext context, Form form)
		{
			// get the GET and POST propertybags
			var getProperties = context.Stack.Peek<IPropertyBag>("Get");
			var postProperties = context.Stack.Peek<IPropertyBag>("Post");
			var fieldProperties = new PropertyBag();
			var formProperties = new PropertyBag();
			var action = string.Empty;

			// if the form has more than one step, try to load the state
			var stateLossDetected = false;
			if (form.Steps.Count > 1)
			{
				// try to load the state
				string stateString;
				var stateKey = form.Prefix + "state";
				if (!postProperties.TryGet(context, stateKey, out stateString) || string.IsNullOrEmpty(stateString))
					stateString = getProperties.Get<string>(context, stateKey, null);

				// dehydrate the field properties, if there is state
				if (!string.IsNullOrEmpty(stateString))
				{
					// dehydrate the field properties
					var dehydratedFieldProperties = context.Nucleus.ResolveSingle<IConversionService>().Convert<IPropertyBag>(context, stateString);

					// merge them with the current properties
					fieldProperties.Merge(dehydratedFieldProperties);
				}
				else
					stateLossDetected = true;
			}

			// load the properties from the data source
			foreach (var candidate in postProperties.Where(candidate => candidate.Key.StartsWith(form.Prefix, StringComparison.OrdinalIgnoreCase)))
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
			var currentStepId = formProperties.Get(context, "current-step", getProperties.Get(context, form.Prefix + "current-step", 0));
			var currentStep = form.Steps.ElementAtOrDefault(currentStepId) ?? form.Steps.First();

			// always go to the first step on multi step forms when the state has been lost
			if (form.Steps.Count > 1 && stateLossDetected)
				currentStep = form.Steps.First();

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
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> for which to store the state.</param>
		/// <param name="state">The <see cref="FormState"/> which to save.</param>
		protected override void DoStoreState(IMansionWebContext context, Form form, FormState state)
		{
			// do nothing
		}
		/// <summary>
		/// Advances the <paramref name="form"/> to the <paramref name="step"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> being advanced.</param>
		/// <param name="step">The <see cref="Step"/> to which to advance.</param>
		protected override void DoAdvanceTo(IMansionWebContext context, Form form, Step step)
		{
			// build the new URL
			var url = context.Request.RequestUrl.Clone();
			url.QueryString.Add(form.Prefix + "current-step", (form.Steps.ToList().IndexOf(step)).ToString(CultureInfo.InvariantCulture));

			// copy all non form field properties to the query string
			foreach (var parameter in context.Stack.Peek<IPropertyBag>("Post").Where(candidate => !candidate.Key.StartsWith(form.Prefix, StringComparison.OrdinalIgnoreCase)))
				url.QueryString.Add(parameter.Key, parameter.Value != null ? parameter.Value.ToString() : string.Empty);

			// set the form state
			url.QueryString.Add(form.Prefix + "state", context.Nucleus.ResolveSingle<IConversionService>().Convert<string>(context, form.State.FieldProperties));

			// set redirect
			WebUtilities.RedirectRequest(context, url);
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag dataSource;
		#endregion
	}
}