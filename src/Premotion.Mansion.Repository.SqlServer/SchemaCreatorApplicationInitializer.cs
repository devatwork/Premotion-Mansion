using System;
using System.Linq;
using System.Text;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Repository.SqlServer.Schemas.Descriptors;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Initializes the current repository from the application if there is one. This initializer makes sure the common nodes exist in the repository.
	/// </summary>
	public class SchemaCreatorApplicationInitializer : ApplicationInitializer
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SchemaCreatorApplicationInitializer(ITypeService typeService) : base(20)
		{
			// validate arguments
			if (typeService == null)
				throw new ArgumentNullException("typeService");

			// set values
			this.typeService = typeService;
		}
		#endregion
		#region Overrides of ApplicationInitializer
		/// <summary>
		/// Initializes the application.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		protected override void DoInitialize(IMansionContext context)
		{
			// open the repository
			SqlServerRepository repository;
			if (!context.Nucleus.TryResolveSingle(out repository))
				return;

			// loop over all the registered types and gather their schema
			var schemaBuffer = new StringBuilder();
			foreach (var type in typeService.LoadAll(context).OrderBy(type =>
			                                                          {
			                                                          	RootTypeTableDescriptor rootTypeTableDescriptor;
			                                                          	return type.TryGetDescriptor(out rootTypeTableDescriptor) ? 0 : 1;
			                                                          }))
			{
				// get the schema if available
				SchemaDescriptor schemaDescriptor;
				if (!type.TryGetDescriptor(out schemaDescriptor))
					continue;

				// append the schema to the buffer
				schemaBuffer.Append(schemaDescriptor.GetSchema(context)).AppendLine("; ");
			}

			// execute the schema creation
			repository.ExecuteWithoutTransaction(context, schemaBuffer.ToString());
		}
		#endregion
		#region Private Fields
		private readonly ITypeService typeService;
		#endregion
	}
}