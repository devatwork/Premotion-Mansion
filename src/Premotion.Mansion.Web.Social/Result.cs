using System;

namespace Premotion.Mansion.Web.Social
{
	/// <summary>
	/// Represents the result of a social request.
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	public class Result<TModel> where TModel : class
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		private Result()
		{
		}
		#endregion
		#region Factory Methods
		/// <summary>
		/// Constructs a succesful result.
		/// </summary>
		/// <param name="model">The model of the result.</param>
		/// <returns>Returns the created <see cref="Result{TModel}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="model"/>  is null.</exception>
		public static Result<TModel> Success(TModel model)
		{
			// validate arguments
			if (model == null)
				throw new ArgumentNullException("model");

			// set values
			return new Result<TModel>
			       {
			       	model = model
			       };
		}
		/// <summary>
		/// Constructs a OAuth redirect result.
		/// </summary>
		/// <param name="redirectUri">The <see cref="Uri"/> to which to redirect.</param>
		/// <returns>Returns the created <see cref="Result{TModel}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="redirectUri"/>  is null.</exception>
		public static Result<TModel> Redirect(Uri redirectUri)
		{
			// validate arguments
			if (redirectUri == null)
				throw new ArgumentNullException("redirectUri");

			// set values
			return new Result<TModel>
			       {
			       	redirectUri = redirectUri
			       };
		}
		/// <summary>
		/// Constructs a error result.
		/// </summary>
		/// <param name="exception">The <see cref="Exception"/> which caused the errorr.</param>
		/// <returns>Returns the created <see cref="Result{TModel}"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="exception"/>  is null.</exception>
		public static Result<TModel> Error(Exception exception)
		{
			// validate arguments
			if (exception == null)
				throw new ArgumentNullException("exception");

			// set values
			return new Result<TModel>
			       {
			       	exception = exception
			       };
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the <typeparamref name="TModel"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="IsSuccessful"/> is false .</exception>
		public TModel Model
		{
			get
			{
				// check if a model was supplied
				if (model == null)
				{
					if (redirectUri != null)
						throw new InvalidOperationException("The result does not contain a model, the request is redirected to the OAuth flow.");
					if (exception != null)
						throw new InvalidOperationException("The result does not contain a model, the request resulted in an exception.");

					throw new InvalidOperationException("The result does not contain a model.");
				}

				return model;
			}
		}
		/// <summary>
		/// Gets the OAuth redirect <see cref="Uri"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="IsOAuthRedirect"/> is false .</exception>
		public Uri RedirectUri
		{
			get
			{
				// check if a model was supplied
				if (redirectUri == null)
				{
					if (model != null)
						throw new InvalidOperationException("The result does not contain a redirect uri, the request is succesful.");
					if (exception != null)
						throw new InvalidOperationException("The result does not contain a redirect uri, the request resulted in an exception.");

					throw new InvalidOperationException("The result does not contain a redirect uri.");
				}

				return redirectUri;
			}
		}
		/// <summary>
		/// Gets the <see cref="System.Exception"/> thrown while executing the request..
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if <see cref="HasError"/> is false .</exception>
		public Exception Exception
		{
			get
			{
				// check if a model was supplied
				if (exception == null)
				{
					if (model != null)
						throw new InvalidOperationException("The result does not contain an exception, the request is succesful.");
					if (redirectUri != null)
						throw new InvalidOperationException("The result does not contain an exception, the request is redirected to the OAuth flow.");

					throw new InvalidOperationException("The result does not contain an exception.");
				}

				return exception;
			}
		}
		/// <summary>
		/// Flag indicating whether the request is successful.
		/// </summary>
		public bool IsSuccessful
		{
			get { return model != null; }
		}
		/// <summary>
		/// Flag indicating whether the request is redirected to the OAuth workflow.
		/// </summary>
		public bool IsOAuthRedirect
		{
			get { return redirectUri != null; }
		}
		/// <summary>
		/// Flag indicating whether the request resulted in an <see cref="Exception"/>.
		/// </summary>
		public bool HasError
		{
			get { return exception != null; }
		}
		#endregion
		#region Private Fields
		private Exception exception;
		private TModel model;
		private Uri redirectUri;
		#endregion
	}
}