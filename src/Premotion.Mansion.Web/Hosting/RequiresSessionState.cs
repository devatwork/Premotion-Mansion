using System;

namespace Premotion.Mansion.Web.Hosting
{
	/// <summary>
	/// Requires a certain session state for the current request.
	/// </summary>
	public class RequiresSessionState
	{
		#region Constants
		private const string NoString = "no";
		private const string ReadOnlyString = "readonly";
		private const string FullString = "full";
		/// <summary>
		/// No session state is required.
		/// </summary>
		public static readonly RequiresSessionState No = new RequiresSessionState(0);
		/// <summary>
		/// Read-only state is required.
		/// </summary>
		public static readonly RequiresSessionState ReadOnly = new RequiresSessionState(1);
		/// <summary>
		/// Full state is required.
		/// </summary>
		public static readonly RequiresSessionState Full = new RequiresSessionState(2);
		#endregion
		#region Constructors
		/// <summary>
		/// Private constructors.
		/// </summary>
		private RequiresSessionState(int weight)
		{
			this.weight = weight;
		}
		#endregion
		#region Parse Methods
		/// <summary>
		/// Parses the given <paramref name="stateString"/> into a <see cref="RequiresSessionState"/>.
		/// </summary>
		/// <param name="stateString">The state string.</param>
		/// <returns>Returns the parsed <see cref="RequiresSessionState"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="stateString"/> is null.</exception>
		public static RequiresSessionState Parse(string stateString)
		{
			// validate arguments
			if (stateString == null)
				throw new ArgumentNullException("stateString");

			switch (stateString)
			{
				case NoString:
					return No;
				case ReadOnlyString:
					return ReadOnly;
				case FullString:
					return Full;
			}

			// use stateles by default
			return No;
		}
		#endregion
		#region Math Functions
		/// <summary>
		/// Determines the highest demand of the two given demands.
		/// </summary>
		/// <param name="one">The <see cref="RequiresSessionState"/>.</param>
		/// <param name="other">The <see cref="RequiresSessionState"/>.</param>
		/// <returns>Returns the highest demand.</returns>
		/// <exception cref="ArgumentNullException">Thrown if any of the parameters is null.</exception>
		public static RequiresSessionState DetermineHighestDemand(RequiresSessionState one, RequiresSessionState other)
		{
			// validate arguments
			if (one == null)
				throw new ArgumentNullException("one");
			if (other == null)
				throw new ArgumentNullException("other");

			return one.weight > other.weight ? one : other;
		}
		#endregion
		#region Private Fields
		private readonly int weight;
		#endregion
	}
}