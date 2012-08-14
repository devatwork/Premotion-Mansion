using System;
using System.Text;

namespace Premotion.Mansion.Core.Data.Queries
{
	/// <summary>
	/// Represents a component of a <see cref="Query"/>. A component describes a single part of a <see cref="Query"/>.
	/// </summary>
	public abstract class QueryComponent
	{
		#region Overrides of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override sealed string ToString()
		{
			// create a builder
			var builder = new StringBuilder();

			// fill the builder;
			AsString(builder);

			//return the content of the builder
			return builder.ToString();
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		public void AsString(StringBuilder builder)
		{
			// validate arguments
			if (builder == null)
				throw new ArgumentNullException("builder");

			// invoke template method
			DoAsString(builder);
		}
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="builder">The <see cref="T:System.Text.StringBuilder"/> in which to store the <see cref="T:System.String"/> representation.</param>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
		protected abstract void DoAsString(StringBuilder builder);
		#endregion
	}
}