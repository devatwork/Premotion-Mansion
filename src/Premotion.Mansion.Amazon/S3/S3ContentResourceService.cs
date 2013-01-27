using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.IO.Windows;
using Premotion.Mansion.Core.Patterns;

namespace Premotion.Mansion.Amazon.S3
{
	/// <summary>
	/// Implements <see cref="IContentResourceService"/> for Amazon S3.
	/// </summary>
	public class S3ContentResourceService : DisposableBase, IContentResourceService
	{
		#region Nested type: S3Resource
		/// <summary>
		/// Implements <see cref="IResource"/> for S3 resources.
		/// </summary>
		private class S3Resource : IResource
		{
			#region Nested type: S3InputPipe
			/// <summary>
			/// Implements <see cref="IInputPipe"/> for files.
			/// </summary>
			private class S3InputPipe : DisposableBase, IInputPipe
			{
				#region Constructors
				/// <summary>
				/// Constructs a <see cref="FileResource.FileInputPipe"/>.
				/// </summary>
				/// <param name="response">The <see cref="GetObjectResponse"/>.</param>
				/// <param name="path">The <see cref="IResourcePath"/>.</param>
				public S3InputPipe(GetObjectResponse response, IResourcePath path)
				{
					// validate arguments
					if (response == null)
						throw new ArgumentNullException("response");
					if (path == null)
						throw new ArgumentNullException("path");

					// set values
					this.response = response;
					reader = new StreamReader(response.ResponseStream);
				}
				#endregion
				#region Implementation of IPipe
				/// <summary>
				/// Gets the encoding of this pipe.
				/// </summary>
				public Encoding Encoding
				{
					get
					{
						CheckDisposed();
						return reader.CurrentEncoding;
					}
				}
				#endregion
				#region Implementation of IInputPipe
				/// <summary>
				/// Gets the reader for this pipe.
				/// </summary>
				public TextReader Reader
				{
					get
					{
						CheckDisposed();
						return reader;
					}
				}
				/// <summary>
				/// Gets the underlying stream of this pipe. Use with caution.
				/// </summary>
				public Stream RawStream
				{
					get
					{
						CheckDisposed();
						return reader.BaseStream;
					}
				}
				#endregion
				#region Overrides of DisposableBase
				/// <summary>
				/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
				/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
				/// </summary>
				/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
				protected override void DisposeResources(bool disposeManagedResources)
				{
					if (!disposeManagedResources)
						return;

					if (reader != null)
						reader.Dispose();
					if (response != null)
						response.Dispose();
				}
				#endregion
				#region Private Fields
				private readonly StreamReader reader;
				private readonly GetObjectResponse response;
				#endregion
			}
			#endregion
			#region Nested type: S3OutputPipe
			/// <summary>
			/// Implements <see cref="IOutputPipe"/> for files.
			/// </summary>
			private class S3OutputPipe : DisposableBase, IOutputPipe
			{
				#region Constructors
				/// <summary>
				/// Constructs a <see cref="FileResource.FileInputPipe"/>.
				/// </summary>
				/// <param name="service">The <see cref="S3ContentResourceService"/>.</param>
				/// <param name="path">The <see cref="IResourcePath"/>.</param>
				public S3OutputPipe(S3ContentResourceService service, IResourcePath path)
				{
					// validate arguments
					if (service == null)
						throw new ArgumentNullException("service");
					if (path == null)
						throw new ArgumentNullException("path");

					// set values
					this.service = service;
					this.path = path;
					stream = new MemoryStream();
					writer = new StreamWriter(stream);
				}
				#endregion
				#region Implementation of IPipe
				/// <summary>
				/// Gets the encoding of this pipe.
				/// </summary>
				public Encoding Encoding
				{
					get
					{
						CheckDisposed();
						return writer.Encoding;
					}
				}
				#endregion
				#region Implementation of IOutputPipe
				/// <summary>
				/// Gets the writer for this pipe.
				/// </summary>
				public TextWriter Writer
				{
					get
					{
						CheckDisposed();
						return writer;
					}
				}
				/// <summary>
				/// Gets the underlying stream of this pipe. Use with caution.
				/// </summary>
				public Stream RawStream
				{
					get
					{
						CheckDisposed();
						return writer.BaseStream;
					}
				}
				#endregion
				#region Overrides of DisposableBase
				/// <summary>
				/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
				/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
				/// </summary>
				/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
				protected override void DisposeResources(bool disposeManagedResources)
				{
					if (!disposeManagedResources)
						return;

					// makes sure all content is written
					writer.Flush();

					// rewind the stream
					stream.Seek(0, SeekOrigin.Begin);

					// create the request
					var request = new PutObjectRequest();
					request.WithBucketName(service.bucketName).WithKey(path.Paths.First()).WithInputStream(stream);

					// execute the request
					service.client.Value.PutObject(request).Dispose();

					// dispose the objects
					writer.Dispose();
					stream.Dispose();
				}
				#endregion
				#region Private Fields
				private readonly IResourcePath path;
				private readonly S3ContentResourceService service;
				private readonly MemoryStream stream;
				private readonly StreamWriter writer;
				#endregion
			}
			#endregion
			#region Constructors
			/// <summary>
			/// Constructs an S3 resource.
			/// </summary>
			/// <param name="service">The <see cref="S3ContentResourceService"/>.</param>
			/// <param name="metadata">The <see cref="GetObjectMetadataResponse"/> describing the data.</param>
			/// <param name="path">The <see cref="IResourcePath"/> of the resource.</param>
			public S3Resource(S3ContentResourceService service, GetObjectMetadataResponse metadata, IResourcePath path)
			{
				// validate arguments
				if (service == null)
					throw new ArgumentNullException("service");
				if (path == null)
					throw new ArgumentNullException("path");

				// set values
				this.service = service;
				this.metadata = metadata;
				this.path = path;
			}
			#endregion
			#region Implementation of IResource
			/// <summary>
			/// Opens this resource for reading.
			/// </summary>
			/// <returns>Returns a <see cref="IOutputPipe"/>.</returns>
			public IInputPipe OpenForReading()
			{
				// get the response
				var response = service.RetrieveResponse(path.Paths.First());

				// create and return the read pipe
				return new S3InputPipe(response, path);
			}
			/// <summary>
			/// Opens this resource for writing.
			/// </summary>
			/// <returns>Returns a <see cref="IInputPipe"/>.</returns>
			public IOutputPipe OpenForWriting()
			{
				return new S3OutputPipe(service, path);
			}
			/// <summary>
			/// Serves as a hash function for a particular type. 
			/// </summary>
			/// <returns>A hash code for the current <see cref="IResource"/>.</returns>
			public string GetResourceIdentifier()
			{
				return metadata != null ? metadata.AmazonId2 : Guid.NewGuid().ToString();
			}
			/// <summary>
			/// Gets the path of this resource.
			/// </summary>
			public IResourcePath Path
			{
				get { return path; }
			}
			/// <summary>
			/// Gets the size of this resource in bytes.
			/// </summary>
			public long Length
			{
				get { return metadata != null ? metadata.ContentLength : -1; }
			}
			/// <summary>
			/// Gets the version of this resource.
			/// </summary>
			public string Version
			{
				get { return metadata != null ? metadata.VersionId : "0"; }
			}
			#endregion
			#region Private Fields
			private readonly GetObjectMetadataResponse metadata;
			private readonly IResourcePath path;
			private readonly S3ContentResourceService service;
			#endregion
		}
		#endregion
		#region Nested type: S3ResourcePath
		/// <summary>
		/// Implements <see cref="IResourcePath"/> for <see cref="IContentResourceService"/>.
		/// </summary>
		private class S3ResourcePath : IResourcePath
		{
			#region Constructors
			/// <summary>
			/// Constructs a content resource path.
			/// </summary>
			/// <param name="relativePath">The relative path to the content resource.</param>
			public S3ResourcePath(string relativePath)
			{
				// validate arguments
				if (string.IsNullOrEmpty(relativePath))
					throw new ArgumentNullException("relativePath");

				// store the values
				Paths = new[] {relativePath.Replace('\\', '/')};
			}
			#endregion
			#region Implementation of IResourcePath
			/// <summary>
			/// Gets a flag indicating whether this resource is overridable or not.
			/// </summary>
			public bool Overridable
			{
				get { return false; }
			}
			/// <summary>
			/// Gets the relative path to this resource.
			/// </summary>
			public IEnumerable<string> Paths { get; private set; }
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs an S3 content resource service.
		/// </summary>
		public S3ContentResourceService()
		{
			// get the settings
			var appSettings = ConfigurationManager.AppSettings;

			// get the settings from the application settings
			bucketName = appSettings[Constants.BucketNameApplicationSetting];
			if (string.IsNullOrEmpty(bucketName))
				throw new ArgumentException(string.Format("Bucket name could not be found.  Add an appsetting to your App.config with the name {0} with a value of your bucket name.", Constants.BucketNameApplicationSetting));
		}
		#endregion
		#region Implementation of IResourceService
		/// <summary>
		/// Checks whether a resource exists at the specified paths.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The path to the resource.</param>
		/// <returns>Returns true when a resource exists, otherwise false.</returns>
		public bool Exists(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			try
			{
				client.Value.GetObjectMetadata(new GetObjectMetadataRequest().WithBucketName(bucketName).WithKey(path.Paths.Single())).Dispose();
				return true;
			}
			catch (AmazonS3Exception ex)
			{
				if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
					return false;

				//status wasn't not found, so throw the exception
				throw;
			}
		}
		/// <summary>
		/// Parses the properties into a resource path.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="properties">The properties which to parse.</param>
		/// <returns>Returns the parsed resource path.</returns>
		public IResourcePath ParsePath(IMansionContext context, IPropertyBag properties)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (properties == null)
				throw new ArgumentNullException("properties");
			CheckDisposed();

			// get the resource base path
			var categoryBasePath = properties.Get(context, "category", "Temp");

			// check if it is an existing resource
			string relativePath;
			if (properties.TryGet(context, "relativePath", out relativePath))
				return new S3ResourcePath(relativePath);

			// check if it is a new file name
			string fileName;
			if (properties.TryGet(context, "fileName", out fileName))
			{
				// get the current date
				var today = DateTime.Today;

				// get the file base name and extension
				var fileBaseName = Path.GetFileNameWithoutExtension(fileName);
				var fileExtension = Path.GetExtension(fileName);

				// make sure the file name is unique
				var index = 0;
				while (Exists(context, new S3ResourcePath(ResourceUtils.Combine(categoryBasePath, today.Year.ToString(CultureInfo.InvariantCulture), today.Month.ToString(CultureInfo.InvariantCulture), fileBaseName + index + fileExtension))))
					index++;

				// create the resource path
				return new S3ResourcePath(ResourceUtils.Combine(categoryBasePath, today.Year.ToString(CultureInfo.InvariantCulture), today.Month.ToString(CultureInfo.InvariantCulture), fileBaseName + index + fileExtension));
			}

			throw new NotImplementedException();
		}
		/// <summary>
		/// Gets the first and most important relative path of <paramref name="path"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/>.</param>
		/// <returns>Returns a string version of the most important relative path.</returns>
		public string GetFirstRelativePath(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// just return the first path
			return path.Paths.First();
		}
		/// <summary>
		/// Opens the resource using the specified path. This will create the resource if it does not already exist.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/> identifying the resource.</param>
		/// <returns>Returns the <see cref="IResource"/>.</returns>
		public IResource GetResource(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// get the meta data for the file and dispose the response streams inmediately
			GetObjectMetadataResponse metaData = null;
			try
			{
				metaData = client.Value.GetObjectMetadata(new GetObjectMetadataRequest().WithBucketName(bucketName).WithKey(path.Paths.Single()));
				metaData.Dispose();
			}
			catch (AmazonS3Exception ex)
			{
				//status wasn't not found, so throw the exception
				if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
					throw;
			}

			// create the resource
			return new S3Resource(this, metaData, path);
		}
		/// <summary>
		/// Deletes a resource idefentified by the specified <paramref name="path"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="path">The <see cref="IResourcePath"/> identifying the resource.</param>
		public void DeleteResource(IMansionContext context, IResourcePath path)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (path == null)
				throw new ArgumentNullException("path");
			CheckDisposed();

			// delete the data
			try
			{
				client.Value.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey(path.Paths.Single())).Dispose();
			}
			catch (AmazonS3Exception ex)
			{
				// status wasn't not found, so throw the exception
				if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
					throw;
			}
		}
		#endregion
		#region Read/Writer Methods
		/// <summary>
		/// Retrieve an <see cref="GetObjectResponse"/> by it's <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>Returns the <see cref="GetObjectResponse"/>.</returns>
		private GetObjectResponse RetrieveResponse(string key)
		{
			// validate arguments
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			return client.Value.GetObject(new GetObjectRequest().WithBucketName(bucketName).WithKey(key));
		}
		#endregion
		#region Overrides of DisposableBase
		/// <summary>
		/// Dispose resources. Override this method in derived classes. Unmanaged resources should always be released
		/// when this method is called. Managed resources may only be disposed of if disposeManagedResources is true.
		/// </summary>
		/// <param name="disposeManagedResources">A value which indicates whether managed resources may be disposed of.</param>
		protected override void DisposeResources(bool disposeManagedResources)
		{
			// do not do unmanaged disposes
			if (!disposeManagedResources)
				return;

			// do nothing
			if (client.IsValueCreated)
				client.Value.Dispose();
		}
		#endregion
		#region Private Fields
		private readonly string bucketName;
		private readonly Lazy<AmazonS3> client = new Lazy<AmazonS3>(() => {
			// get the settings
			var appSettings = ConfigurationManager.AppSettings;

			// get the settings from the application settings
			var awsAccessKeyId = appSettings[Constants.AccessKeyApplicationSetting];
			if (string.IsNullOrEmpty(awsAccessKeyId))
				throw new ArgumentException(string.Format("Access Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your access key.", Constants.AccessKeyApplicationSetting));
			var awsSecretAccessKey = appSettings[Constants.AccessSecretApplicationSetting];
			if (string.IsNullOrEmpty(awsSecretAccessKey))
				throw new ArgumentException(string.Format("Secret Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your secret key.", Constants.AccessSecretApplicationSetting));
			var bucketName = appSettings[Constants.BucketNameApplicationSetting];
			if (string.IsNullOrEmpty(bucketName))
				throw new ArgumentException(string.Format("Bucket name could not be found.  Add an appsetting to your App.config with the name {0} with a value of your bucket name.", Constants.BucketNameApplicationSetting));

			try
			{
				// create the amazon S3 client
				var client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey);

				// verify if the bucket exists and is set up correctly
				if (!AmazonS3Util.DoesS3BucketExist(bucketName, client))
					throw new InvalidOperationException(string.Format("S3 bucket with the name '{0}' does not exist, check your appsetting", bucketName));

				// verify permissions
				var acl = client.GetACL(new GetACLRequest {BucketName = bucketName});
				var grants = acl.AccessControlList.Grants;
				if (grants.Any(x => x.Permission == S3Permission.FULL_CONTROL))
					return client;
				if (grants.All(x => x.Permission != S3Permission.WRITE))
					throw new InvalidOperationException(string.Format("You do not have write access to S3 bucket '{0}', check your amazon permission configuration", bucketName));
				if (grants.All(x => x.Permission != S3Permission.READ))
					throw new InvalidOperationException(string.Format("You do not have read access to S3 bucket '{0}', check your amazon permission configuration", bucketName));

				return client; // unreachable
			}
			catch (AmazonS3Exception ex)
			{
				// check credentials
				if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") || ex.ErrorCode.Equals("InvalidSecurity")))
					throw new InvalidOperationException("Invalid AWS credentials");

				// check permission
				if (ex.ErrorCode != null && ex.ErrorCode.Equals("AccessDenied"))
					throw new InvalidOperationException(string.Format("Access denied. You don't have permission to access the bucket '{0}'", bucketName));

				// unknown exception
				throw;
			}
		});
		#endregion
	}
}