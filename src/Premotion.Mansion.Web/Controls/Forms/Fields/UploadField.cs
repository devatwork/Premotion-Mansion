using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Scripting.TagScript;

namespace Premotion.Mansion.Web.Controls.Forms.Fields
{
	/// <summary>
	/// Implements an upload <see cref="Field{TValue}"/>.
	/// </summary>
	public class UploadField : Field<string>
	{
		#region Nested type: UploadFactoryTag
		/// <summary>
		/// This tag creates a <see cref="UploadField"/>.
		/// </summary>
		[ScriptTag(Constants.FormTagNamespaceUri, "upload")]
		public class UploadFactoryTag : FieldFactoryTag<UploadField>
		{
			#region Overrides of FieldFactoryTag<Upload>
			/// <summary>
			/// Creates the <see cref="Control"/>.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
			protected override UploadField Create(IMansionWebContext context, ControlDefinition definition)
			{
				return new UploadField(definition);
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a control with the specified ID.
		/// </summary>
		/// <param name="definition">The <see cref="ControlDefinition"/>.</param>
		public UploadField(ControlDefinition definition) : base(definition)
		{
		}
		#endregion
		#region Overrides of Field{TValue}
		/// <summary>
		/// Initializes this form control.
		/// </summary>
		/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
		/// <param name="form">The <see cref="Form"/> to which this control belongs.</param>
		protected override void DoInitialize(IMansionWebContext context, Form form)
		{
			// initialize base
			base.DoInitialize(context, form);

			// check for postback
			if (!form.State.IsPostback)
				return;

			// check if the file is uploaded for this field
			WebFile uploadedFile;
			if (context.Request.Files.TryGetValue(form.FieldPrefix + Name + "-upload", out uploadedFile) && !string.IsNullOrEmpty(uploadedFile.FileName) && uploadedFile.ContentLength != 0)
			{
				// store the file
				var contentResourceService = context.Nucleus.ResolveSingle<IContentResourceService>();
				var resourcePath = contentResourceService.ParsePath(context, new PropertyBag
				                                                             {
				                                                             	{"fileName", uploadedFile.FileName},
				                                                             	{"category", "Uploads"}
				                                                             });
				var resource = contentResourceService.GetResource(context, resourcePath);
				using (var pipe = resource.OpenForWriting())
					uploadedFile.InputStream.CopyTo(pipe.RawStream);

				// set the path to the file as the value of the property
				var uploadedFilePath = contentResourceService.GetFirstRelativePath(context, resourcePath);
				SetValue(uploadedFilePath);
				form.State.FieldProperties.Set(Name, uploadedFilePath);
			}
		}
		#endregion
	}
}