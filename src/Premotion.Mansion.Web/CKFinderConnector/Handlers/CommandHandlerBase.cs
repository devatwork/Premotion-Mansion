using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using Premotion.Mansion.Core;
using Premotion.Mansion.Web.Assets;

namespace Premotion.Mansion.Web.CKFinderConnector.Handlers
{
	/// <summary>
	/// Base class for all CKFinder command handlers.
	/// </summary>
	public abstract class CommandHandlerBase
	{
		#region Handle Methods
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="IMansionWebContext"/>.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		public void Handle(IMansionWebContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			try
			{
				//set values
				Request = context.HttpContext.Request;
				Response = context.HttpContext.Response;

				// get the upload service
				AssetService = context.Nucleus.ResolveSingle<IAssetService>();

				// get the asset type
				var request = context.HttpContext.Request;
				AssetType = AssetService.ParseResourceType(context, request.QueryString["type"] ?? string.Empty);

				// get the current folder
				CurrentAssetFolder = AssetService.ParseFolder(context, AssetType, request.QueryString["currentFolder"] ?? "/");

				// invoke concrete implementation
				DoHandle(context);
			}
			catch (ThreadAbortException)
			{
				// thread is aborted, so don't throw any new exceptions
			}
			catch (Exception)
			{
				throw new ConnectorException(ErrorCodes.Unknown);
			}
		}
		/// <summary>
		/// Handels the incomming CKFinder command.
		/// </summary>
		/// <param name="context">The incoming <see cref="IMansionWebContext"/>.</param>
		/// <exception cref="ConnectorException">Thrown when an error occured while handling the command.</exception>
		protected abstract void DoHandle(IMansionWebContext context);
		#endregion
		#region ACL Methods
		/// <summary>
		/// Gets the ACL mask of the <paramref name="assetType"/>.
		/// </summary>
		/// <param name="assetType">The <see cref="AssetType"/> for which to generate the ACL mask.</param>
		/// <returns>Returns the mask code.</returns>
		protected static int GetACLMask(AssetType assetType)
		{
			// validate arguments
			if (assetType == null)
				throw new ArgumentNullException("assetType");
			return (int) (AccessControlRules.FolderView | AccessControlRules.FileView | AccessControlRules.FolderCreate | AccessControlRules.FileUpload);
		}
		/// <summary>
		/// Gets the ACL mask of the <paramref name="folder"/>.
		/// </summary>
		/// <param name="folder">The <see cref="AssetFolder"/> for which to generate the ACL mask.</param>
		/// <returns>Returns the mask code.</returns>
		protected static int GetACLMask(AssetFolder folder)
		{
			// validate arguments
			if (folder == null)
				throw new ArgumentNullException("folder");
			return (int) (AccessControlRules.FolderView | AccessControlRules.FileView | AccessControlRules.FolderCreate | AccessControlRules.FileUpload);
		}
		#endregion
		#region Security Methods
		///<summary>
		/// Hash an input string and return the hash as a 40 character hexadecimal string.
		/// </summary>
		protected static string GetMACTripleDESHash(string input)
		{
			// Create a new instance of the MACTripleDESCryptoServiceProvider object.
			byte[] data;
			using (var macTripleDESHasher = KeyedHashAlgorithm.Create())
				data = macTripleDESHasher.ComputeHash(Encoding.Default.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			var sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (var i = 0; i < data.Length; i++)
				sBuilder.Append(data[i].ToString("x2"));

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
		#endregion
		#region Formatting Methods
		/// <summary>
		/// Formats the uri.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		protected static string FormatUri(Uri url)
		{
			// validate arguments
			if (url == null)
				throw new ArgumentNullException("url");

			return url.ToString().Trim(new[] {'/'}) + '/';
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the current resource type.
		/// </summary>
		protected AssetType AssetType { get; private set; }
		/// <summary>
		/// Gets the current folder.
		/// </summary>
		protected AssetFolder CurrentAssetFolder { get; private set; }
		/// <summary>
		/// Gets the <see cref="IAssetService"/>.
		/// </summary>
		protected IAssetService AssetService { get; private set; }
		/// <summary>
		/// Gets the <see cref="HttpRequestBase"/> of the current request.
		/// </summary>
		protected HttpRequestBase Request { get; private set; }
		/// <summary>
		/// Gets the <see cref="HttpResponseBase"/> of the current request.
		/// </summary>
		protected HttpResponseBase Response { get; private set; }
		#endregion
	}
}