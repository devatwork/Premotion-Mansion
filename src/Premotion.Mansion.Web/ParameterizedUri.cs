using System;
using System.Linq;
using System.Text;
using System.Web;
using Premotion.Mansion.Core;

namespace Premotion.Mansion.Web
{
	/// <summary>
	/// Describes a parameterized URL.
	/// </summary>
	public class ParameterizedUri
	{
		#region Constructors
		/// <summary>
		/// Constructs a new parameterized uri.
		/// </summary>
		/// <param name="baseUrl"></param>
		public ParameterizedUri(Uri baseUrl)
		{
			// validate arguments
			if (baseUrl == null)
				throw new ArgumentNullException("baseUrl");

			// parse the current properties
			url = baseUrl.GetLeftPart(UriPartial.Path);
			parameters = HttpUtility.ParseQueryString(baseUrl.Query, Encoding.UTF8).ToPropertyBag();
		}
		#endregion
		#region Override of Object
		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			// check if there are parameters
			var buffer = new StringBuilder(url);
			if (parameters.Count > 0)
				buffer.Append("?" + string.Join("&", parameters.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value.ToString()))));

			// return the content of the buffer
			return buffer.ToString();
		}
		#endregion
		#region Operators
		/// <summary>
		/// Converts this <see cref="ParameterizedUri"/> to an <see cref="String"/>.
		/// </summary>
		/// <param name="uri">The uri which to convert.</param>
		/// <returns>Returns the string representation of <paramref name="uri"/>.</returns>
		public static implicit operator string(ParameterizedUri uri)
		{
			// validate arguments
			if (uri == null)
				throw new ArgumentNullException("uri");

			return uri.ToString();
		}
		/// <summary>
		/// Converts this <see cref="ParameterizedUri"/> to an <see cref="Uri"/>.
		/// </summary>
		/// <param name="uri">The uri which to convert.</param>
		/// <returns>Returns the <see cref="Uri"/> representation of <paramref name="uri"/>.</returns>
		public static implicit operator Uri(ParameterizedUri uri)
		{
			// validate arguments
			if (uri == null)
				throw new ArgumentNullException("uri");

			var builder = new UriBuilder(uri.url)
			              {
			              	Query = string.Join("&", uri.parameters.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value.ToString())))
			              };

			return builder.Uri;
		}
		#endregion
		#region Parameter Methods
		/// <summary>
		/// Sets a parameter in this query.
		/// </summary>
		/// <param name="name">The name of the parameter which to set.</param>
		/// <param name="value">The value of the parapeter.</param>
		/// <returns>Returns the instance for chaining.</returns>
		public ParameterizedUri SetParameter(string name, string value)
		{
			// validate arguments
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// set value or clear it
			if (!string.IsNullOrEmpty(value))
				parameters.Set(name, value);
			else
				parameters.Remove(name);
			return this;
		}
		#endregion
		#region Private Fields
		private readonly IPropertyBag parameters;
		private readonly string url;
		#endregion
	}
}