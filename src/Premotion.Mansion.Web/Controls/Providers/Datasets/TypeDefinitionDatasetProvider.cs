using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Scripting.TagScript;
using Premotion.Mansion.Core.Types;
using Premotion.Mansion.Web.Cms.Descriptors;
using Premotion.Mansion.Web.Cms.Model;

namespace Premotion.Mansion.Web.Controls.Providers.Datasets
{
	/// <summary>
	/// Provides a <see cref="Dataset"/> containing name/value pairs of all <see cref="ITypeDefinition"/>s.
	/// </summary>
	public class TypeDefinitionDatasetProvider : DatasetProvider
	{
		#region Nested type: AllTypesDefinitionDatasetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="TypeDefinitionDatasetProvider"/>s.
		/// </summary>
		[ScriptTag(Constants.DataProviderTagNamespaceUri, "allTypesDefinitionDatasetProvider")]
		public class AllTypesDefinitionDatasetProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Nested type: AllTypesLoadStrategy
			/// <summary>
			/// Simply provides all the available types.
			/// </summary>
			private class AllTypesLoadStrategy : LoadStrategy
			{
				#region Overrides of LoadStrategy
				/// <summary>
				/// Gets the <see cref="ITypeDefinition"/>s which to provide.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <param name="typeService">The <see cref="ITypeService"/> from which to load the types.</param>
				/// <returns>Returns the types which to provide.</returns>
				protected override IEnumerable<ITypeDefinition> DoGet(IMansionContext context, ITypeService typeService)
				{
					return typeService.LoadAll(context);
				}
				#endregion
			}
			#endregion
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(IMansionWebContext context)
			{
				// choose the proper loading strategy
				var strategy = new AllTypesLoadStrategy();

				// by default return the all types strategy
				return new TypeDefinitionDatasetProvider(strategy);
			}
			#endregion
		}
		#endregion
		#region Nested type: ChildTypeDefinitionDatasetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="TypeDefinitionDatasetProvider"/>s.
		/// </summary>
		[ScriptTag(Constants.DataProviderTagNamespaceUri, "childTypeDefinitionDatasetProvider")]
		public class ChildTypeDefinitionDatasetProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Nested type: ChildTypesLoadStrategy
			/// <summary>
			/// Provides a list of all child types.
			/// </summary>
			private class ChildTypesLoadStrategy : LoadStrategy
			{
				#region Constructors
				/// <summary>
				/// Private constructor, use <see cref="Create"/>.
				/// </summary>
				/// <param name="behavior">The <see cref="CmsBehavior"/>.</param>
				private ChildTypesLoadStrategy(CmsBehavior behavior)
				{
					// set values
					this.behavior = behavior;
				}
				#endregion
				#region Factory Methods
				/// <summary>
				/// Creates a <see cref="ChildTypesLoadStrategy"/> strategy.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <param name="parentType">The <see cref="ITypeDefinition"/> for which to load the child types.</param>
				/// <returns>Returns the <see cref="ChildTypesLoadStrategy"/> instance.</returns>
				public static ChildTypesLoadStrategy Create(IMansionContext context, ITypeDefinition parentType)
				{
					// validate arguments
					if (context == null)
						throw new ArgumentNullException("context");
					if (parentType == null)
						throw new ArgumentNullException("parentType");

					// get the CMS behavoir
					CmsBehaviorDescriptor cmsDescriptor;
					if (!parentType.TryFindDescriptorInHierarchy(out cmsDescriptor))
						throw new InvalidOperationException(string.Format("Could not find cmd behavoir descriptor on type {0}", parentType.Name));
					var behavior = cmsDescriptor.GetBehavior(context);

					// create the strategy
					return new ChildTypesLoadStrategy(behavior);
				}
				#endregion
				#region Overrides of LoadStrategy
				/// <summary>
				/// Gets the <see cref="ITypeDefinition"/>s which to provide.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <param name="typeService">The <see cref="ITypeService"/> from which to load the types.</param>
				/// <returns>Returns the types which to provide.</returns>
				protected override IEnumerable<ITypeDefinition> DoGet(IMansionContext context, ITypeService typeService)
				{
					return behavior.GetAllowedChildTypes(context).Where(candidate =>
					                                                    {
					                                                    	CmsBehaviorDescriptor cmsDescriptor;
					                                                    	if (!candidate.TryFindDescriptorInHierarchy(out cmsDescriptor))
					                                                    		throw new InvalidOperationException(string.Format("Could not find cmd behavoir descriptor on type {0}", candidate.Name));
					                                                    	var cmsBehavoir = cmsDescriptor.GetBehavior(context);
					                                                    	return !cmsBehavoir.IsAbstract;
					                                                    });
				}
				#endregion
				#region Private Fields
				private readonly CmsBehavior behavior;
				#endregion
			}
			#endregion
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(IMansionWebContext context)
			{
				// get the type
				var parentType = GetRequiredAttribute<ITypeDefinition>(context, "type");

				// create the childtypestrategy
				var strategy = ChildTypesLoadStrategy.Create(context, parentType);

				// by default return the all types strategy
				return new TypeDefinitionDatasetProvider(strategy);
			}
			#endregion
		}
		#endregion
		#region Nested type: InheritingTypesDefinitionDatasetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="TypeDefinitionDatasetProvider"/>s.
		/// </summary>
		[ScriptTag(Constants.DataProviderTagNamespaceUri, "inhertingTypesDefinitionDatasetProvider")]
		public class InheritingTypesDefinitionDatasetProviderFactoryTag : DatasetProviderFactoryTag<DatasetProvider>
		{
			#region Nested type: InhertingTypesLoadStrategy
			/// <summary>
			/// Provides a list of all child types.
			/// </summary>
			private class InhertingTypesLoadStrategy : LoadStrategy
			{
				#region Constructors
				/// <summary>
				/// Private constructor, use <see cref="Create"/>.
				/// </summary>
				/// <param name="inhertingTypes">The <see cref="CmsBehavior"/>.</param>
				private InhertingTypesLoadStrategy(ICollection<ITypeDefinition> inhertingTypes)
				{
					// set values
					this.inhertingTypes = inhertingTypes;
				}
				#endregion
				#region Factory Methods
				/// <summary>
				/// Creates a <see cref="InhertingTypesLoadStrategy"/> strategy.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <param name="parentType">The <see cref="ITypeDefinition"/> for which to load the child types.</param>
				/// <returns>Returns the <see cref="InhertingTypesLoadStrategy"/> instance.</returns>
				public static InhertingTypesLoadStrategy Create(IMansionContext context, ITypeDefinition parentType)
				{
					// validate arguments
					if (context == null)
						throw new ArgumentNullException("context");
					if (parentType == null)
						throw new ArgumentNullException("parentType");

					// set the inheriting types
					var inhertingTypes = parentType.GetInheritingTypes(context).ToList();

					// create the strategy
					return new InhertingTypesLoadStrategy(inhertingTypes);
				}
				#endregion
				#region Overrides of LoadStrategy
				/// <summary>
				/// Gets the <see cref="ITypeDefinition"/>s which to provide.
				/// </summary>
				/// <param name="context">The <see cref="IMansionContext"/>.</param>
				/// <param name="typeService">The <see cref="ITypeService"/> from which to load the types.</param>
				/// <returns>Returns the types which to provide.</returns>
				protected override IEnumerable<ITypeDefinition> DoGet(IMansionContext context, ITypeService typeService)
				{
					return inhertingTypes;
				}
				#endregion
				#region Private Fields
				private readonly ICollection<ITypeDefinition> inhertingTypes;
				#endregion
			}
			#endregion
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="IMansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override DatasetProvider Create(IMansionWebContext context)
			{
				// get the type
				var parentType = GetRequiredAttribute<ITypeDefinition>(context, "type");

				// create the childtypestrategy
				var strategy = InhertingTypesLoadStrategy.Create(context, parentType);

				// by default return the all types strategy
				return new TypeDefinitionDatasetProvider(strategy);
			}
			#endregion
		}
		#endregion
		#region Nested type: LoadStrategy
		/// <summary>
		/// Strategy for retrieving the type dataset.
		/// </summary>
		private abstract class LoadStrategy
		{
			/// <summary>
			/// Gets the <see cref="ITypeDefinition"/>s which to provide.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="typeService">The <see cref="ITypeService"/> from which to load the types.</param>
			/// <returns>Returns the types which to provide.</returns>
			public IEnumerable<ITypeDefinition> Get(IMansionContext context, ITypeService typeService)
			{
				// validate arguments
				if (context == null)
					throw new ArgumentNullException("context");
				if (typeService == null)
					throw new ArgumentNullException("typeService");
				return DoGet(context, typeService);
			}
			/// <summary>
			/// Gets the <see cref="ITypeDefinition"/>s which to provide.
			/// </summary>
			/// <param name="context">The <see cref="IMansionContext"/>.</param>
			/// <param name="typeService">The <see cref="ITypeService"/> from which to load the types.</param>
			/// <returns>Returns the types which to provide.</returns>
			protected abstract IEnumerable<ITypeDefinition> DoGet(IMansionContext context, ITypeService typeService);
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a dataset provider.
		/// </summary>
		/// <param name="strategy">The <see cref="LoadStrategy"/> from which to load the types.</param>
		private TypeDefinitionDatasetProvider(LoadStrategy strategy)
		{
			// validate arguments
			if (strategy == null)
				throw new ArgumentNullException("strategy");

			// set values
			this.strategy = strategy;
		}
		#endregion
		#region Overrides of DataProvider<Dataset>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Dataset DoRetrieve(IMansionContext context)
		{
			// create the dataset
			var dataset = new Dataset();

			// loop over all the types
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();
			foreach (var type in strategy.Get(context, typeService))
			{
				CmsBehaviorDescriptor cmsDescriptor;
				if (!type.TryFindDescriptorInHierarchy(out cmsDescriptor))
					throw new InvalidOperationException(string.Format("Could not find cmd behavoir descriptor on type {0}", type.Name));
				var cmsBehavoir = cmsDescriptor.GetBehavior(context);

				// create a row
				dataset.AddRow(new PropertyBag
				               {
				               	{"label", cmsBehavoir.Label},
				               	{"value", type.Name}
				               });
			}

			// sort the dataset
			dataset.Sort((x, y) =>
			             {
			             	var labelX = x.Get<string>(context, "label");
			             	var labelY = y.Get<string>(context, "label");

			             	return labelX.CompareTo(labelY);
			             });

			// return the set
			return dataset;
		}
		#endregion
		#region Private Fields
		private readonly LoadStrategy strategy;
		#endregion
	}
}