﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;
using Premotion.Mansion.Core.Data.Clauses;
using Premotion.Mansion.Core.Data.Queries;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Repository.SqlServer.Converters;

namespace Premotion.Mansion.Repository.SqlServer
{
	/// <summary>
	/// Implements <see cref="IRepositoryFactory"/> for <see cref="Repository.SqlServer"/>s.
	/// </summary>
	[Named(typeof (IRepositoryFactory), Constants.NamespaceUri, "Factory")]
	public class SqlServerRepositoryFactory : IRepositoryFactory
	{
		#region Implementation of IRepositoryFactory
		/// <summary>
		/// Creates a <see cref="IRepository"/> instance.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="arguments">The arguments which to use to create the <see cref="IRepository"/></param>
		/// <returns>Returns the created <see cref="IRepository"/>.</returns>
		public IRepository Create(IMansionContext context, IPropertyBag arguments)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// check for connection string
			string connectionString;
			if (!arguments.TryGet(context, "SQLSERVER_CONNECTION_STRING", out connectionString) || string.IsNullOrEmpty(connectionString))
				throw new InvalidOperationException("Could not open connection to SQL server repository without a connection string. Make sure the setting SQLSERVER_CONNECTION_STRING is specified.");

			// create the repository
			return new SqlServerRepository(context, connectionString, context.Nucleus.Resolve<IClauseConverter>(), context.Nucleus.Resolve<QueryInterpreter>(), context.Nucleus.ResolveSingle<IQueryParser>());
		}
		#endregion
	}
}