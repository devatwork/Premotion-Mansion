﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.Nucleus;
using Premotion.Mansion.Core.Patterns.Descriptors;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core
{
	/// <summary>
	/// Defines extension methods for several types.
	/// </summary>
	public static partial class Extensions
	{
		#region Constants
		private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings());
		private static readonly string[][] HtmlNamedEntities = new[] {
			new[] {"&quot;", "\""},
			new[] {"&lt;", "<"},
			new[] {"&gt;", ">"},
			new[] {"&nbsp;", " "},
			new[] {"&iexcl;", "¡"},
			new[] {"&cent;", "¢"},
			new[] {"&pound;", "£"},
			new[] {"&curren;", "¤"},
			new[] {"&yen;", "¥"},
			new[] {"&brvbar;", "¦"},
			new[] {"&sect;", "§"},
			new[] {"&uml;", "¨"},
			new[] {"&copy;", "©"},
			new[] {"&ordf;", "ª"},
			new[] {"&laquo;", "«"},
			new[] {"&not;", "¬"},
			new[] {"&shy;", "­"},
			new[] {"&reg;", "®"},
			new[] {"&macr;", "¯"},
			new[] {"&deg;", "°"},
			new[] {"&plusmn;", "±"},
			new[] {"&sup2;", "²"},
			new[] {"&sup3;", "³"},
			new[] {"&acute;", "´"},
			new[] {"&micro;", "µ"},
			new[] {"&para;", "¶"},
			new[] {"&middot;", "·"},
			new[] {"&cedil;", "¸"},
			new[] {"&sup1;", "¹"},
			new[] {"&ordm;", "º"},
			new[] {"&raquo;", " »"},
			new[] {"&frac14;", "¼"},
			new[] {"&frac12;", "½"},
			new[] {"&frac34;", "¾"},
			new[] {"&iquest;", "¿"},
			new[] {"&Agrave;", "À"},
			new[] {"&Aacute;", "Á"},
			new[] {"&Acirc;", "Â"},
			new[] {"&Atilde;", "Ã"},
			new[] {"&Auml;", "Ä"},
			new[] {"&Aring;", "Å"},
			new[] {"&AElig;", "Æ"},
			new[] {"&Ccedil;", "Ç"},
			new[] {"&Egrave;", "È"},
			new[] {"&Eacute;", "É"},
			new[] {"&Ecirc;", "Ê"},
			new[] {"&Euml;", "Ë"},
			new[] {"&Igrave;", "Ì"},
			new[] {"&Iacute;", "Í"},
			new[] {"&Icirc;", "Î"},
			new[] {"&Iuml;", "Ï"},
			new[] {"&ETH;", "Ð"},
			new[] {"&Ntilde;", "Ñ"},
			new[] {"&Ograve;", "Ò"},
			new[] {"&Oacute;", "Ó"},
			new[] {"&Ocirc;", "Ô"},
			new[] {"&Otilde;", "Õ"},
			new[] {"&Ouml;", "Ö"},
			new[] {"&times;", "×"},
			new[] {"&Oslash;", "Ø"},
			new[] {"&Ugrave;", "Ù"},
			new[] {"&Uacute;", "Ú"},
			new[] {"&Ucirc;", "Û"},
			new[] {"&Uuml;", "Ü"},
			new[] {"&Yacute;", "Ý"},
			new[] {"&THORN;", "Þ"},
			new[] {"&szlig;", "ß"},
			new[] {"&agrave;", "à"},
			new[] {"&aacute;", "á"},
			new[] {"&acirc;", "â"},
			new[] {"&atilde;", "ã"},
			new[] {"&auml;", "ä"},
			new[] {"&aring;", "å"},
			new[] {"&aelig;", "æ"},
			new[] {"&ccedil;", "ç"},
			new[] {"&egrave;", "è"},
			new[] {"&eacute;", "é"},
			new[] {"&ecirc;", "ê"},
			new[] {"&euml;", "ë"},
			new[] {"&igrave;", "ì"},
			new[] {"&iacute;", "í"},
			new[] {"&icirc;", "î"},
			new[] {"&iuml;", "ï"},
			new[] {"&eth;", "ð"},
			new[] {"&ntilde;", "ñ"},
			new[] {"&ograve;", "ò"},
			new[] {"&oacute;", "ó"},
			new[] {"&ocirc;", "ô"},
			new[] {"&otilde;", "õ"},
			new[] {"&ouml;", "ö"},
			new[] {"&divide;", "÷"},
			new[] {"&oslash;", "ø"},
			new[] {"&ugrave;", "ù"},
			new[] {"&uacute;", "ú"},
			new[] {"&ucirc;", "û"},
			new[] {"&uuml;", "ü"},
			new[] {"&yacute;", "ý"},
			new[] {"&thorn;", "þ"},
			new[] {"&yuml;", "ÿ"},
			new[] {"&OElig;", "Œ"},
			new[] {"&oelig;", "œ"},
			new[] {"&Scaron;", "Š"},
			new[] {"&scaron;", "š"},
			new[] {"&Yuml;", "Ÿ"},
			new[] {"&fnof;", "ƒ"},
			new[] {"&circ;", "ˆ"},
			new[] {"&tilde;", "˜"},
			new[] {"&Alpha;", "Α"},
			new[] {"&Beta;", "Β"},
			new[] {"&Gamma;", "Γ"},
			new[] {"&Delta;", "Δ"},
			new[] {"&Epsilon;", "Ε"},
			new[] {"&Zeta;", "Ζ"},
			new[] {"&Eta;", "Η"},
			new[] {"&Theta;", "Θ"},
			new[] {"&Iota;", "Ι"},
			new[] {"&Kappa;", "Κ"},
			new[] {"&Lambda;", "Λ"},
			new[] {"&Mu;", "Μ"},
			new[] {"&Nu;", "Ν"},
			new[] {"&Xi;", "Ξ"},
			new[] {"&Omicron;", "Ο"},
			new[] {"&Pi;", "Π"},
			new[] {"&Rho;", "Ρ"},
			new[] {"&Sigma;", "Σ"},
			new[] {"&Tau;", "Τ"},
			new[] {"&Upsilon;", "Υ"},
			new[] {"&Phi;", "Φ"},
			new[] {"&Chi;", "Χ"},
			new[] {"&Psi;", "Ψ"},
			new[] {"&Omega;", "Ω"},
			new[] {"&alpha;", "α"},
			new[] {"&beta;", "β"},
			new[] {"&gamma;", "γ"},
			new[] {"&delta;", "δ"},
			new[] {"&epsilon;", "ε"},
			new[] {"&zeta;", "ζ"},
			new[] {"&eta;", "η"},
			new[] {"&theta;", "θ"},
			new[] {"&iota;", "ι"},
			new[] {"&kappa;", "κ"},
			new[] {"&lambda;", "λ"},
			new[] {"&mu;", "μ"},
			new[] {"&nu;", "ν"},
			new[] {"&xi;", "ξ"},
			new[] {"&omicron;", "ο"},
			new[] {"&pi;", "π"},
			new[] {"&rho;", "ρ"},
			new[] {"&sigmaf;", "ς"},
			new[] {"&sigma;", "σ"},
			new[] {"&tau;", "τ"},
			new[] {"&upsilon;", "υ"},
			new[] {"&phi;", "φ"},
			new[] {"&chi;", "χ"},
			new[] {"&psi;", "ψ"},
			new[] {"&omega;", "ω"},
			new[] {"&thetasym;", "ϑ"},
			new[] {"&upsih;", "ϒ"},
			new[] {"&piv;", "ϖ"},
			new[] {"&ensp;", " "},
			new[] {"&emsp;", " "},
			new[] {"&thinsp;", " "},
			new[] {"&zwnj;", "‌"},
			new[] {"&zwj;", "‍"},
			new[] {"&lrm;", "‎"},
			new[] {"&rlm;", "‏"},
			new[] {"&ndash;", "–"},
			new[] {"&mdash;", "—"},
			new[] {"&lsquo;", "‘"},
			new[] {"&rsquo;", "’"},
			new[] {"&sbquo;", "‚"},
			new[] {"&ldquo;", "“"},
			new[] {"&rdquo;", "”"},
			new[] {"&bdquo;", "„"},
			new[] {"&dagger;", "†"},
			new[] {"&Dagger;", "‡"},
			new[] {"&bull;", "•"},
			new[] {"&hellip;", "…"},
			new[] {"&permil;", "‰"},
			new[] {"&prime;", "′"},
			new[] {"&Prime;", "″"},
			new[] {"&lsaquo;", "‹"},
			new[] {"&rsaquo;", "›"},
			new[] {"&oline;", "‾"},
			new[] {"&frasl;", "⁄"},
			new[] {"&euro;", "€"},
			new[] {"&image;", "ℑ"},
			new[] {"&weierp;", "℘"},
			new[] {"&real;", "ℜ"},
			new[] {"&trade;", "™"},
			new[] {"&alefsym;", "ℵ"},
			new[] {"&larr;", "←"},
			new[] {"&uarr;", "↑"},
			new[] {"&rarr;", "→"},
			new[] {"&darr;", "↓"},
			new[] {"&harr;", "↔"},
			new[] {"&crarr;", "↵"},
			new[] {"&lArr;", "⇐"},
			new[] {"&uArr;", "⇑"},
			new[] {"&rArr;", "⇒"},
			new[] {"&dArr;", "⇓"},
			new[] {"&hArr;", "⇔"},
			new[] {"&forall;", "∀"},
			new[] {"&part;", "∂"},
			new[] {"&exist;", "∃"},
			new[] {"&empty;", "∅"},
			new[] {"&nabla;", "∇"},
			new[] {"&isin;", "∈"},
			new[] {"&notin;", "∉"},
			new[] {"&ni;", "∋"},
			new[] {"&prod;", "∏"},
			new[] {"&sum;", "∑"},
			new[] {"&minus;", "−"},
			new[] {"&lowast;", "∗"},
			new[] {"&radic;", "√"},
			new[] {"&prop;", "∝"},
			new[] {"&infin;", "∞"},
			new[] {"&ang;", "∠"},
			new[] {"&and;", "∧"},
			new[] {"&or;", "∨"},
			new[] {"&cap;", "∩"},
			new[] {"&cup;", "∪"},
			new[] {"&int;", "∫"},
			new[] {"&there4;", "∴"},
			new[] {"&sim;", "∼"},
			new[] {"&cong;", "≅"},
			new[] {"&asymp;", "≈"},
			new[] {"&ne;", "≠"},
			new[] {"&equiv;", "≡"},
			new[] {"&le;", "≤"},
			new[] {"&ge;", "≥"},
			new[] {"&sub;", "⊂"},
			new[] {"&sup;", "⊃"},
			new[] {"&nsub;", "⊄"},
			new[] {"&sube;", "⊆"},
			new[] {"&supe;", "⊇"},
			new[] {"&oplus;", "⊕"},
			new[] {"&otimes;", "⊗"},
			new[] {"&perp;", "⊥"},
			new[] {"&sdot;", "⋅"},
			new[] {"&lceil;", "⌈"},
			new[] {"&rceil;", "⌉"},
			new[] {"&lfloor;", "⌊"},
			new[] {"&rfloor;", "⌋"},
			new[] {"&lang;", "〈"},
			new[] {"&rang;", "〉"},
			new[] {"&loz;", "◊"},
			new[] {"&spades;", "♠"},
			new[] {"&clubs;", "♣"},
			new[] {"&hearts;", "♥"},
			new[] {"&diams;", "♦"},
			new[] {"&amp;", "&"}
		};
		#endregion
		#region Extensions of IEnumerable
		/// <summary>
		/// Performans an topilogical sort on the given <paramref name="source"/>.
		/// </summary>
		/// <typeparam name="TSource">The type of item which to sort.</typeparam>
		/// <param name="source">The source values.</param>
		/// <param name="predicate">The predicate which determines if the value is suitable for yielding.</param>
		/// <returns>Returns the sorted values.</returns>
		/// <remarks>
		/// Please note that this method does not prevent endless loops, handle with care.
		/// </remarks>
		public static IEnumerable<TSource> TopologicalSort<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
		{
			// validate arguments
			if (source == null)
				throw new ArgumentNullException("source");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			// get the values, the keys, the number of items, an array of unyielded result indices and a pointer
			var values = source.ToArray();
			var count = values.Length;
			var notYieldedIndexes = Enumerable.Range(0, count).ToArray();
			var valuesToGo = count;

			// keep looping while there are values to process
			while (valuesToGo > 0)
			{
				// loop over all the unyielded indices
				foreach (var notYieldedIndex in notYieldedIndexes.Where(candidate => candidate >= 0))
				{
					// get the item
					var item = values[notYieldedIndex];

					// check if the item is not suitable for yielding yet using the predicate
					if (!predicate(item))
						continue;

					// yield the item and update the not-yielded index
					yield return values[notYieldedIndex];
					notYieldedIndexes[notYieldedIndex] = -1;
					valuesToGo--;
				}
			}
		}
		#endregion
		#region Extensions of JArray
		/// <summary>
		/// Turns the given <paramref name="array"/> into a <see cref="Dataset"/>.
		/// </summary>
		/// <param name="array">The <see cref="JArray"/>.</param>
		/// <returns>Returns the <see cref="Dataset"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static Dataset ToDataset(this JArray array)
		{
			// validate arguments
			if (array == null)
				throw new ArgumentNullException("array");

			// create the dataset
			var dataset = new Dataset();
			foreach (JObject obj in array.Children())
				dataset.AddRow(obj.ToPropertyBag());
			return dataset;
		}
		#endregion
		#region Extensions of JObject
		/// <summary>
		/// Turns the given <paramref name="obj"/> into a <see cref="IPropertyBag"/>.
		/// </summary>
		/// <param name="obj">The <see cref="JObject"/>.</param>
		/// <returns>Returns the <see cref="IPropertyBag"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static IPropertyBag ToPropertyBag(this JObject obj)
		{
			// validate arguments
			if (obj == null)
				throw new ArgumentNullException("obj");

			// create the property bag
			var properties = new PropertyBag();
			foreach (var property in obj.Properties())
				properties.Set(property.Name, property.Value.Value<string>());

			// TODO: map child objects as well?

			return properties;
		}
		#endregion
		#region Extensions of XContainer
		/// <summary>
		/// Gets the first (in document order) child element with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XContainer"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns the <see cref="XContainer"/> with the specified <paramref name="name"/>.</returns>
		/// <exception cref="InvalidOperationException">Thrown when an element with <paramref name="name"/> is not found in <paramref name="element"/>.</exception>
		public static XElement RequiredElement(this XContainer element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			var child = element.Element(name);
			if (child == null)
				throw new InvalidOperationException(String.Format("Expected element of name '{0}' in '{1}'", name, element));
			return child;
		}
		/// <summary>
		/// Checks whether <paramref name="element"/> has an element with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XContainer"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns true when the element is found otherwise false.</returns>
		public static bool HasElement(this XContainer element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			return element.Element(name) != null;
		}
		/// <summary>
		/// Appends the <paramref name="node"/> to the <paramref name="container"/>.
		/// </summary>
		/// <typeparam name="TNode">The type of <see cref="XNode"/> which will be added.</typeparam>
		/// <param name="container">The <see cref="XContainer"/> to which to append the <paramref name="node"/></param>
		/// <param name="node">The <see cref="XNode"/> which to append.</param>
		/// <returns>Returns the appended node.</returns>
		public static TNode Append<TNode>(this XContainer container, TNode node) where TNode : XNode
		{
			// validate arguments
			if (container == null)
				throw new ArgumentNullException("container");
			if (node == null)
				throw new ArgumentNullException("node");

			// add to the container
			container.Add(node);
			return node;
		}
		#endregion
		#region Extensions of XElement
		/// <summary>
		/// Gets attribute with the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="element">The <see cref="XElement"/> which to search.</param>
		/// <param name="name">The <see cref="XName"/> to match.</param>
		/// <returns>Returns the <see cref="XElement"/> with the specified <paramref name="name"/>.</returns>
		/// <exception cref="InvalidOperationException">Thrown when an attribute with <paramref name="name"/> is not found in <paramref name="element"/>.</exception>
		public static XAttribute RequiredAttribute(this XElement element, XName name)
		{
			// validate arguments
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");

			// try to find the element
			var attribute = element.Attribute(name);
			if (attribute == null)
				throw new InvalidOperationException(String.Format("Expected attribute of name '{0}' in '{1}'", name, element));
			return attribute;
		}
		#endregion
		#region Extensions of Assembly
		/// <summary>
		/// Checks wether the givens <paramref name="candidate"/> is in fact a manion assembly.
		/// </summary>
		/// <param name="candidate">The <see cref="Assembly"/>.</param>
		/// <returns>Returns true when the <paramref name="candidate"/> is a mansion assembly, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static bool IsMansionAssembly(this Assembly candidate)
		{
			// validate arguments
			if (candidate == null)
				throw new ArgumentNullException("candidate");

			// check if the assembly is or references 'Premotion.Mansion.Core'
			const string coreAssemblyName = "Premotion.Mansion.Core";
			return candidate.GetName().Name.Equals(coreAssemblyName) || candidate.GetReferencedAssemblies().Any(referenced => referenced.Name.Equals(coreAssemblyName));
		}
		/// <summary>
		/// Checks wether the givens <paramref name="candidate"/> is in fact a manion assembly.
		/// </summary>
		/// <param name="candidate">The <see cref="AssemblyName"/>.</param>
		/// <returns>Returns true when the <paramref name="candidate"/> is a mansion assembly, otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static bool IsMansionAssembly(this AssemblyName candidate)
		{
			// validate arguments
			if (candidate == null)
				throw new ArgumentNullException("candidate");

			try
			{
				return Assembly.Load(candidate).IsMansionAssembly();
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Gets the <see cref="Type"/>s from the given <paramref name="assembly"/> safely.
		/// </summary>
		/// <param name="assembly">The <see cref="Assembly"/> from which to load the types.</param>
		/// <returns>Returns the loaded <see cref="Type"/>s.</returns>
		/// <seealso href="http://haacked.com/archive/2012/07/23/get-all-types-in-an-assembly.aspx"/>
		public static IEnumerable<Type> GetTypesSafe(this Assembly assembly)
		{
			// validate arguments
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			// try to get the type
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types;
			}
		}
		#endregion
		#region Extensions of INucleus
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <returns>Returns the resolved contract type.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="nucleus"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown the <typeparamref name="TContract"/> instance could not be resolved.</exception>
		public static TContract ResolveSingle<TContract>(this INucleus nucleus) where TContract : class
		{
			//  validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// resolve the object to an instance or throw an exception
			TContract result;
			if (!nucleus.TryResolveSingle(out result))
				throw new InvalidOperationException(String.Format("Missing a dependency of type '{0}'", typeof (TContract)));
			return result;
		}
		/// <summary>
		/// Resolves an single instance of the component implementing <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The type of contract the component must implement in order to be returned by this method. Must be a reference type.</typeparam>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <param name="namespaceUri">The namespace in which the component lives.</param>
		/// <param name="name">The name of the component.</param>
		/// <returns>Returns the resolved contract type.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="nucleus"/>, <paramref name="namespaceUri"/> or <paramref name="name"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown the <typeparamref name="TContract"/> instance could not be resolved.</exception>
		public static TContract ResolveSingle<TContract>(this INucleus nucleus, string namespaceUri, string name) where TContract : class
		{
			//  validate arguments
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");
			if (String.IsNullOrEmpty(namespaceUri))
				throw new ArgumentNullException("namespaceUri");
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// assemble the name
			var strongName = StrongNameGenerator.Generate(namespaceUri, name);

			// resolve the object to an instance or throw an exception
			TContract result;
			if (!nucleus.TryResolveSingle(strongName, out result))
				throw new InvalidOperationException(String.Format("Missing a dependency of type '{0}' with namespace '{1}' and name '{2}'", typeof (TContract), namespaceUri, name));
			return result;
		}
		/// <summary>
		/// Creates an instance of type <typeparamref name="TContract"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/>.</typeparam>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <returns>Returns the created object.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nucleus"/> is null.</exception>
		public static TContract CreateInstance<TContract>(this INucleus nucleus) where TContract : class
		{
			return typeof (TContract).CreateInstance<TContract>(nucleus);
		}
		#endregion
		#region Extensions of ITypeDefinition
		/// <summary>
		/// Finds the <typeparamref name="TDescriptor"/> in the type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the descriptors.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static IEnumerable<TDescriptor> GetDescriptorsInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// find all the matching descriptors in the hierarchy
			return typeDefinition.Hierarchy.SelectMany(type => type.GetDescriptors<TDescriptor>());
		}
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		/// <summary>
		/// Tries to find the <typeparamref name="TDescriptor"/> in the reverse type hierarchy of <paramref name="typeDefinition"/>.
		/// </summary>
		/// <typeparam name="TDescriptor">The type of <see cref="IDescriptor"/>.</typeparam>
		/// <param name="typeDefinition">The <see cref="ITypeDefinition"/> for which to get the <paramref name="descriptor"/>.</param>
		/// <param name="predicate">The <see cref="Predicate{TDescriptor}"/> which can filter the returned descriptor.</param>
		/// <param name="descriptor">The instance of <typeparamref name="TDescriptor"/>.</param>
		/// <returns>Returns true when found, otherwise false.</returns>
		public static bool TryFindDescriptorInHierarchy<TDescriptor>(this ITypeDefinition typeDefinition, Predicate<TDescriptor> predicate, out TDescriptor descriptor) where TDescriptor : class, IDescriptor
		{
			// validate arguments
			if (typeDefinition == null)
				throw new ArgumentNullException("typeDefinition");
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			// loop through all the types in the hierarchy
			foreach (var type in typeDefinition.HierarchyReverse)
			{
				if (type.TryGetDescriptor(out descriptor) && predicate(descriptor))
					return true;
			}

			// descriptor not found
			descriptor = default(TDescriptor);
			return false;
		}
		/// <summary>
		/// Finds the common ancestor of the given <paramref name="types"/>.
		/// </summary>
		/// <param name="types">The <see cref="ITypeDefinition"/>s for which to find the common ancestor.</param>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <returns>Returns the <see cref="ITypeDefinition"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> or <paramref name="context"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Throw if no common ancestor type could be found.</exception>
		public static ITypeDefinition FindCommonAncestor(this IEnumerable<ITypeDefinition> types, IMansionContext context)
		{
			// validate arguments
			if (types == null)
				throw new ArgumentNullException("types");
			if (context == null)
				throw new ArgumentNullException("context");

			// get the type service
			var typeService = context.Nucleus.ResolveSingle<ITypeService>();

			// get the type array
			var typeArray = types.ToArray();

			// if there is no type, assume the root type
			if (typeArray.Length == 0)
				return typeService.LoadRoot(context);

			// if there is one type, it is the root type
			if (typeArray.Length == 1)
				return typeArray[0];

			// find the type
			var rootType = typeArray.First().HierarchyReverse.FirstOrDefault(candidate => typeArray.All(x => x.IsAssignable(candidate)));

			// if there is no root type, throw exception
			if (rootType == null)
				throw new InvalidOperationException(string.Format("Could not find a common base type for types {0}", string.Join(", ", typeArray.Select(type => type.Name))));

			// return the root type
			return rootType;
		}
		#endregion
		#region Extensions of String
		/// <summary>
		/// Checks whether the <paramref name="input"/> is a number.
		/// </summary>
		/// <param name="input">The input which to check.</param>
		/// <returns>Returns true when the <paramref name="input"/> is a number otherwise false.</returns>
		public static bool IsNumber(this string input)
		{
			// validate arguments
			return !string.IsNullOrEmpty(input) && input.All(Char.IsDigit);
		}
		/// <summary>
		/// Strips the HTML from the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to strip.</param>
		/// <returns>Returns the stripped <paramref name="input"/>.</returns>
		/// <remarks>http://www.codeproject.com/KB/MCMS/htmlTagStripper.aspx</remarks>
		public static string StripHtml(this string input)
		{
			if (input == null)
				return null;
			input = input.Trim();
			if (input == String.Empty)
				return String.Empty;

			var bodyStartTagIdx = input.IndexOf("<body", StringComparison.CurrentCultureIgnoreCase);
			var bodyEndTagIdx = input.IndexOf("</body>", StringComparison.CurrentCultureIgnoreCase);

			int startIdx = 0, endIdx = input.Length - 1;
			if (bodyStartTagIdx >= 0)
				startIdx = bodyStartTagIdx;
			if (bodyEndTagIdx >= 0)
				endIdx = bodyEndTagIdx;

			bool insideTag = false,
			     insideAttributeValue = false,
			     insideHtmlComment = false,
			     insideScriptBlock = false,
			     insideNoScriptBlock = false,
			     insideStyleBlock = false;
			var attributeValueDelimiter = '"';

			var sb = new StringBuilder(input.Length);
			for (var i = startIdx; i <= endIdx; i++)
			{
				// html comment block
				if (!insideHtmlComment)
				{
					if (i + 3 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '!' &&
					    input[i + 2] == '-' &&
					    input[i + 3] == '-')
					{
						i += 3;
						insideHtmlComment = true;
						continue;
					}
				}
				else // inside html comment
				{
					if (i + 2 < input.Length &&
					    input[i] == '-' &&
					    input[i + 1] == '-' &&
					    input[i + 2] == '>')
					{
						i += 2;
						insideHtmlComment = false;
					}
					continue;
				}

				// noscript block
				if (!insideNoScriptBlock)
				{
					if (i + 9 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 'n' || input[i + 1] == 'N') &&
					    (input[i + 2] == 'o' || input[i + 2] == 'O') &&
					    (input[i + 3] == 's' || input[i + 3] == 'S') &&
					    (input[i + 4] == 'c' || input[i + 4] == 'C') &&
					    (input[i + 5] == 'r' || input[i + 5] == 'R') &&
					    (input[i + 6] == 'i' || input[i + 6] == 'I') &&
					    (input[i + 7] == 'p' || input[i + 7] == 'P') &&
					    (input[i + 8] == 't' || input[i + 8] == 'T') &&
					    (Char.IsWhiteSpace(input[i + 9]) || input[i + 9] == '>'))
					{
						i += 9;
						insideNoScriptBlock = true;
						continue;
					}
				}
				else // inside noscript block
				{
					if (i + 10 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 'n' || input[i + 2] == 'N') &&
					    (input[i + 3] == 'o' || input[i + 3] == 'O') &&
					    (input[i + 4] == 's' || input[i + 4] == 'S') &&
					    (input[i + 5] == 'c' || input[i + 5] == 'C') &&
					    (input[i + 6] == 'r' || input[i + 6] == 'R') &&
					    (input[i + 7] == 'i' || input[i + 7] == 'I') &&
					    (input[i + 8] == 'p' || input[i + 8] == 'P') &&
					    (input[i + 9] == 't' || input[i + 9] == 'T') &&
					    (Char.IsWhiteSpace(input[i + 10]) || input[i + 10] == '>'))
					{
						if (input[i + 10] != '>')
						{
							i += 9;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 10;
						insideNoScriptBlock = false;
					}
					continue;
				}

				// script block
				if (!insideScriptBlock)
				{
					if (i + 7 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 's' || input[i + 1] == 'S') &&
					    (input[i + 2] == 'c' || input[i + 2] == 'C') &&
					    (input[i + 3] == 'r' || input[i + 3] == 'R') &&
					    (input[i + 4] == 'i' || input[i + 4] == 'I') &&
					    (input[i + 5] == 'p' || input[i + 5] == 'P') &&
					    (input[i + 6] == 't' || input[i + 6] == 'T') &&
					    (Char.IsWhiteSpace(input[i + 7]) || input[i + 7] == '>'))
					{
						i += 6;
						insideScriptBlock = true;
						continue;
					}
				}
				else // inside script block
				{
					if (i + 8 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 's' || input[i + 2] == 'S') &&
					    (input[i + 3] == 'c' || input[i + 3] == 'C') &&
					    (input[i + 4] == 'r' || input[i + 4] == 'R') &&
					    (input[i + 5] == 'i' || input[i + 5] == 'I') &&
					    (input[i + 6] == 'p' || input[i + 6] == 'P') &&
					    (input[i + 7] == 't' || input[i + 7] == 'T') &&
					    (Char.IsWhiteSpace(input[i + 8]) || input[i + 8] == '>'))
					{
						if (input[i + 8] != '>')
						{
							i += 7;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 8;
						insideScriptBlock = false;
					}
					continue;
				}

				// style block
				if (!insideStyleBlock)
				{
					if (i + 7 < input.Length &&
					    input[i] == '<' &&
					    (input[i + 1] == 's' || input[i + 1] == 'S') &&
					    (input[i + 2] == 't' || input[i + 2] == 'T') &&
					    (input[i + 3] == 'y' || input[i + 3] == 'Y') &&
					    (input[i + 4] == 'l' || input[i + 4] == 'L') &&
					    (input[i + 5] == 'e' || input[i + 5] == 'E') &&
					    (Char.IsWhiteSpace(input[i + 6]) || input[i + 6] == '>'))
					{
						i += 5;
						insideStyleBlock = true;
						continue;
					}
				}
				else // inside script block
				{
					if (i + 8 < input.Length &&
					    input[i] == '<' &&
					    input[i + 1] == '/' &&
					    (input[i + 2] == 's' || input[i + 2] == 'S') &&
					    (input[i + 3] == 't' || input[i + 3] == 'C') &&
					    (input[i + 4] == 'y' || input[i + 4] == 'R') &&
					    (input[i + 5] == 'l' || input[i + 5] == 'I') &&
					    (input[i + 6] == 'e' || input[i + 6] == 'P') &&
					    (Char.IsWhiteSpace(input[i + 7]) || input[i + 7] == '>'))
					{
						if (input[i + 7] != '>')
						{
							i += 7;
							while (i < input.Length && input[i] != '>')
								i++;
						}
						else
							i += 7;
						insideStyleBlock = false;
					}
					continue;
				}

				if (!insideTag)
				{
					if (i < input.Length &&
					    input[i] == '<')
					{
						insideTag = true;
						continue;
					}
				}
				else // inside tag
				{
					if (!insideAttributeValue)
					{
						if (input[i] == '"' || input[i] == '\'')
						{
							attributeValueDelimiter = input[i];
							insideAttributeValue = true;
							continue;
						}
						if (input[i] == '>')
						{
							insideTag = false;
							sb.Append(' '); // prevent words from different tags (<td>s for example) from joining together
							continue;
						}
					}
					else // inside tag and inside attribute value
					{
						if (input[i] == attributeValueDelimiter)
						{
							insideAttributeValue = false;
							continue;
						}
					}
					continue;
				}

				sb.Append(input[i]);
			}

			foreach (var htmlNamedEntity in HtmlNamedEntities)
				sb.Replace(htmlNamedEntity[0], htmlNamedEntity[1]);

			for (var i = 0; i < 512; i++)
				sb.Replace("&#" + i + ";", ((char) i).ToString());

			return sb.ToString();
		}
		/// <summary>
		/// Strips the UTF-8 characters from the <paramref name="input"/>.
		/// </summary>
		/// <param name="input">The input which to strip.</param>
		/// <returns>Returns the stripped <paramref name="input"/>.</returns>
		/// <remarks>http://stackoverflow.com/a/135473/1255744</remarks>
		public static string StripUTF8Characters(this string input)
		{
			// validate arguments
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			// replaces all ascii charachters
			return Encoding.ASCII.GetString(
				Encoding.Convert(
					Encoding.UTF8,
					Encoding.GetEncoding(
						Encoding.ASCII.EncodingName,
						new EncoderReplacementFallback(string.Empty),
						new DecoderExceptionFallback()
						),
					Encoding.UTF8.GetBytes(input)
					)
				);
		}
		/// <summary>
		/// Replaces each format item in a specified string with the text equivalent of a corresponding object's value.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format. </param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static string Format(this string format, params object[] args)
		{
			// validate arguments
			if (format == null)
				throw new ArgumentNullException("format");
			if (args == null)
				throw new ArgumentNullException("format");

			// return the formatted string
			return string.Format(format, args);
		}
		/// <summary>
		/// Splits the given <paramref name="haystack"/> csv into a set of values.
		/// </summary>
		/// <param name="haystack">The haystack which to split.</param>
		/// <param name="seperator">The seperator which to use to seperate the entries in the list.</param>
		/// <returns>Retutnrs the splitted entries.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="seperator"/> is null or empty.</exception>
		public static IEnumerable<string> Split(this string haystack, string seperator = ",")
		{
			// validate arguments
			if (string.IsNullOrEmpty(seperator))
				throw new ArgumentNullException("seperator");

			return (haystack ?? string.Empty).Split(new[] {seperator}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
		}
		/// <summary>
		/// Appends the given needle to the haystack.
		/// </summary>
		/// <param name="needle">The needle which to remove.</param>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <param name="seperator">The seperator which to use to seperate the entries in the list.</param>
		/// <returns>Returns the modified list.</returns>
		public static string AppendNeedle(this string haystack, string needle, string seperator = ",")
		{
			// validate arguments
			if (string.IsNullOrEmpty(needle))
				throw new ArgumentNullException("needle");

			// split the lists
			var list = haystack.Split(seperator);

			// reassemble the list with the needle
			return list.Union(new[] {needle}, StringComparer.OrdinalIgnoreCase).Join(seperator);
		}
		/// <summary>
		/// Removes the given needle from the haystack.
		/// </summary>
		/// <param name="haystack">The haystack in which to look.</param>
		/// <param name="needle">The needle which to remove.</param>
		/// <param name="seperator">The seperator which to use to seperate the entries in the list.</param>
		/// <returns>Returns the modified list.</returns>
		public static string RemoveNeedle(this string haystack, string needle, string seperator = ",")
		{
			// validate arguments
			if (string.IsNullOrEmpty(needle))
				throw new ArgumentNullException("needle");

			// split the lists
			var list = haystack.Split(seperator);

			// reassemble the list without the needle
			return list.Except(new[] {needle}, StringComparer.OrdinalIgnoreCase).Join(seperator);
		}
		/// <summary>
		/// Joins the given <paramref name="values"/> into a csv.
		/// </summary>
		/// <param name="values">The <see cref="IEnumerable{T}"/> of the values.</param>
		/// <param name="seperator">The seperator which to use to seperate the entries in the list.</param>
		/// <returns>Returns the joined string.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is null or if <paramref name="seperator"/> is null or empty.</exception>
		public static string Join(this IEnumerable<string> values, string seperator = ",")
		{
			// validate arguments
			if (values == null)
				throw new ArgumentNullException("values");
			if (string.IsNullOrEmpty(seperator))
				throw new ArgumentNullException("seperator");

			// assemble the csv
			return string.Join(seperator, values.Distinct(StringComparer.OrdinalIgnoreCase).Select(x => x.Trim()));
		}
		#endregion
		#region Extensions of StringBuilder
		/// <summary>
		///  Determines whether the end of <paramref name="this"/> matches <paramref name="value"/> when compared using the specified comparison option.
		/// </summary>
		/// <param name="this">The <see cref="StringBuilder"/>.</param>
		/// <param name="value">The string to compare to the substring at the end of this instance.</param>
		/// <param name="comparisonType">One of the enumeration values that determines how <paramref name="this"/> and <paramref name="value"/> are compared. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
		/// <returns>Returns true if the <paramref name="value"/> parameter matches the end of this string; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is null.</exception>
		public static bool EndsWith(this StringBuilder @this, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
		{
			// validate arguments
			if (@this == null)
				throw new ArgumentNullException("this");
			if (string.IsNullOrEmpty(value))
				return false;

			// if the buffer is smaller than the value length, the buffer can't possibly contain the value
			var valueLength = value.Length;
			if (@this.Length < valueLength)
				return false;

			// get the last substring
			var substring = @this.ToString(@this.Length - valueLength, valueLength);

			// check if the substring is equal to the value
			return substring.Equals(value, comparisonType);
		}
		#endregion
		#region Extensions of Type
		/// <summary>
		/// Creates an instance of type <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/>.</typeparam>
		/// <param name="type">The <see cref="Type"/> from which to construct an instance.</param>
		/// <param name="nucleus">The <see cref="INucleus"/>.</param>
		/// <returns>Returns the created object.</returns>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="type"/> or <paramref name="nucleus"/> is null.</exception>
		public static TContract CreateInstance<TContract>(this Type type, INucleus nucleus) where TContract : class
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");
			if (nucleus == null)
				throw new ArgumentNullException("nucleus");

			// select the factory function
			var nucleusParameterExpression = Expression.Parameter(typeof (INucleus), "nucleus");
			var factory = ObjectFactoryCache.GetOrAdd(type, key => Expression.Lambda<Func<INucleus, object>>(Expression.Invoke(CreateInstanceFactory<TContract>(key), nucleusParameterExpression), nucleusParameterExpression).Compile());

			// create the instance and return it
			return (TContract) factory(nucleus);
		}
		/// <summary>
		/// Creates an instance factory for the <paramref name="type"/>.
		/// </summary>
		/// <typeparam name="TContract">The contract <see cref="Type"/>.</typeparam>
		/// <param name="type">The <see cref="Type"/> for which to factory will be created.</param>
		/// <returns>Returns the created factory.</returns>
		public static Expression<Func<INucleus, TContract>> CreateInstanceFactory<TContract>(this Type type) where TContract : class
		{
			// validate arguments
			if (type == null)
				throw new ArgumentNullException("type");

			// make sure the type actually implements the contract
			if (!typeof (TContract).IsAssignableFrom(type))
				throw new InvalidOperationException(String.Format("Type '{0}' does not implement contract type '{1}'", type, typeof (TContract)));

			// get the constructor
			var constructorMethodInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			if (constructorMethodInfos.Length != 1)
				throw new InvalidOperationException(String.Format("Could not find an unambiguous public constructor on type '{0}'", type));
			var constructorMethodInfo = constructorMethodInfos[0];
			var constructorParameterInfos = constructorMethodInfo.GetParameters();

			// create the expression list
			var bodyExpressions = new List<Expression>();

			// construct the nucleus param
			var nucleusType = typeof (INucleus);
			var tryResolveSingleMethodInfo = nucleusType.GetMethods().Single(candidate => "TryResolveSingle".Equals(candidate.Name) && candidate.GetParameters().Length == 1);
			if (tryResolveSingleMethodInfo == null)
				throw new InvalidOperationException(String.Format("Could not find mehtod TryResolveSingle with one parameter on type '{0}'", nucleusType));
			var resolveMethodInfo = nucleusType.GetMethods().Single(candidate => "Resolve".Equals(candidate.Name) && candidate.GetParameters().Length == 0);
			if (resolveMethodInfo == null)
				throw new InvalidOperationException(String.Format("Could not find mehtod Resolve with one parameter on type '{0}'", nucleusType));
			var nucleusParameterExpression = Expression.Parameter(nucleusType, "nucleus");

			// construct the parameters for the constructor
			var parameterTypes = constructorParameterInfos.Select(parameter => Expression.Parameter(parameter.ParameterType, parameter.Name)).ToList();
			bodyExpressions.AddRange(constructorParameterInfos.Select((parameterInfo, index) => {
				// get the type
				var injectedType = parameterInfo.ParameterType;

				// check for single or multiple value injection
				if (injectedType.IsGenericType && typeof (IEnumerable<>) == injectedType.GetGenericTypeDefinition())
				{
					// get the type
					var innerType = injectedType.GetGenericArguments()[0];

					// bake the method call
					var resolveCallExpression = Expression.Call(nucleusParameterExpression, resolveMethodInfo.MakeGenericMethod(innerType));

					// set the value
					var assignResultExpression = Expression.Assign(parameterTypes[index], resolveCallExpression);

					// return the block
					return Expression.Block(new Expression[] {assignResultExpression});
				}
				else
				{
					// define the out parameter
					var outParameter = Expression.Variable(injectedType, "out");

					// bake the method call
					var tryResolveCallExpression = Expression.Call(nucleusParameterExpression, tryResolveSingleMethodInfo.MakeGenericMethod(injectedType), outParameter);

					// throw if the type could not be found
					var newDependencyNotFoundExceptionExpression = Expression.New(typeof (InvalidOperationException).GetConstructor(new[] {typeof (string)}), new[] {Expression.Constant(String.Format("Could not resolve injected type '{0}' on type '{1}' make sure it is registered properly", injectedType, type))});
					var checkResolveResultExpression = Expression.IfThen(Expression.Not(tryResolveCallExpression), Expression.Throw(newDependencyNotFoundExceptionExpression));

					// set the value
					var assignResultExpression = Expression.Assign(parameterTypes[index], outParameter);

					// return the block
					return Expression.Block(new[] {outParameter}, new Expression[] {checkResolveResultExpression, assignResultExpression});
				}
			}));

			// bake the constructor call
			bodyExpressions.Add(Expression.New(constructorMethodInfo, parameterTypes));

			// bake the method call using expressions
			return Expression.Lambda<Func<INucleus, TContract>>(Expression.Block(parameterTypes, bodyExpressions), nucleusParameterExpression);
		}
		#endregion
		#region Extensions of Dataset
		/// <summary>
		/// Writes the <paramref name="dataset"/> to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="dataset">The <see cref="Dataset"/>.</param>
		/// <param name="writer">The <see cref="JsonWriter"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void WriteAsJSonArray(this Dataset dataset, JsonWriter writer)
		{
			// validate arguments
			if (dataset == null)
				throw new ArgumentNullException("dataset");
			if (writer == null)
				throw new ArgumentNullException("writer");

			writer.WriteStartArray();

			// write the rows
			foreach (var row in dataset.Rows)
				row.WriteAsJSonObject(writer);

			writer.WriteEndArray();
		}
		#endregion
		#region Extensions of IPropertyBag
		/// <summary>
		/// Writes the <paramref name="bag"/> to the <paramref name="writer"/>.
		/// </summary>
		/// <param name="bag">The <see cref="IPropertyBag"/> which to write.</param>
		/// <param name="writer">The <see cref="JsonWriter"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		public static void WriteAsJSonObject(this IPropertyBag bag, JsonWriter writer)
		{
			// validate arguments
			if (bag == null)
				throw new ArgumentNullException("bag");
			if (writer == null)
				throw new ArgumentNullException("writer");

			writer.WriteStartObject();

			// loop over all the properties
			foreach (var property in bag)
			{
				writer.WritePropertyName(property.Key);

				// check if the value is a dataset, if so render it as such, otherwise render it as plain value
				var dataset = property.Value as Dataset;
				if (dataset != null)
					dataset.WriteAsJSonArray(writer);
				else
					Serializer.Serialize(writer, property.Value);
			}

			writer.WriteEndObject();
		}
		/// <summary>
		/// Creates a shallow copy of the given <paramref name="bag"/>.
		/// </summary>
		/// <param name="bag">The original <see cref="IPropertyBag"/> which to copy.</param>
		/// <returns>Returns the created copy.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bag"/> is null.</exception>
		public static IPropertyBag Copy(this IPropertyBag bag)
		{
			// validate arguments
			if (bag == null)
				throw new ArgumentNullException("bag");

			// return the copy
			return new PropertyBag(bag);
		}
		#endregion
		#region Private Fields
		private static readonly ConcurrentDictionary<Type, Func<INucleus, object>> ObjectFactoryCache = new ConcurrentDictionary<Type, Func<INucleus, object>>();
		#endregion
	}
	/// <summary>
	/// Provides reflection extensions.
	/// </summary>
	public static partial class Extensions
	{
		#region Extensions of Object
		/// <summary>
		/// Invokes the <paramref name="methodName"/> on the given <typeparamref name="TContract"/>.
		/// </summary>
		/// <param name="instance">The instance on which to invoke the method.</param>
		/// <param name="context"></param>
		/// <param name="methodName">The name of the method which to invoke.</param>
		/// <param name="arguments">The arguments which to pass to the method.</param>
		/// <typeparam name="TContract">The type of interface on which the method resides.</typeparam>
		/// <typeparam name="TResult">The type of result returned from the method.</typeparam>
		/// <returns>Returns the result of the method invocation.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown if an error occured while executing the method.</exception>
		public static TResult InvokeMethod<TContract, TResult>(this object instance, IMansionContext context, string methodName, IPropertyBag arguments) where TContract : class
		{
			// validate arguments
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(methodName))
				throw new ArgumentNullException("methodName");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// generate the method signature
			var methodSignature = typeof (TContract).FullName + "_" + methodName;

			// make sure the 
			var typedInstance = instance as TContract;
			if (typedInstance == null)
				throw new InvalidOperationException(string.Format("Could not cast {0} to interface {1}", instance.GetType(), typeof (TContract)));

			// look up the the method
			return (TResult) InvokeMethodOnInstanceCache.GetOrAdd(methodSignature, key => {
				// get the method
				var methodInfo = typeof (TContract).GetMethod(methodName);

				// create the compiled typed lambda
				var typedLambda = CreateInvokeMethodExpression<TContract, TResult>(methodInfo).Compile();

				return (context0, instance0, arguments0) => typedLambda(context0, (TContract) instance0, arguments0);
			})(context, instance, arguments);
		}
		/// <summary>
		/// Creates an expression which executes <paramref name="methodInfo"/> on <typeparamref name="TContract"/>.
		/// </summary>
		/// <param name="methodInfo">The <see cref="MethodInfo"/> for which to generate the invocation expression.</param>
		/// <typeparam name="TContract">The contract type on which to find the method.</typeparam>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <returns>Returns the expression.</returns>
		/// <exception cref="InvalidOperationException">Thrown if there is an ambigious method.</exception>
		private static Expression<Func<IMansionContext, TContract, IPropertyBag, TResult>> CreateInvokeMethodExpression<TContract, TResult>(MethodInfo methodInfo)
		{
			// get the type
			var contractType = typeof (TContract);

			// make sure the method is not a generic method
			if (methodInfo.IsGenericMethod)
				throw new InvalidOperationException(string.Format("Can not invoke generic method '{0}' on type '{1}'", methodInfo.Name, contractType));

			// ensure the return type can be cast
			var returnsNothing = methodInfo.ReturnType == typeof (void);
			if (!returnsNothing && !typeof (TResult).IsAssignableFrom(methodInfo.ReturnType))
				throw new InvalidOperationException(string.Format("Can not cast result of method '{0}' from '{1}' to '{2}'", methodInfo.Name, methodInfo.ReturnType, typeof (TResult)));

			// create the parameter expressions
			var contextParameterExpression = Expression.Parameter(typeof (IMansionContext));
			var instanceParameterExpression = Expression.Parameter(typeof (TContract));
			var argumentsParameterExpression = Expression.Parameter(typeof (IPropertyBag));
			var signatureParameterExpressions = new[] {contextParameterExpression, instanceParameterExpression, argumentsParameterExpression};

			var block = CreateInvokeMethodExpression(methodInfo, contextParameterExpression, instanceParameterExpression, argumentsParameterExpression);

			// create the compiled typed lambda
			return Expression.Lambda<Func<IMansionContext, TContract, IPropertyBag, TResult>>(block, signatureParameterExpressions);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <param name="contextParameterExpression">The parameter expression for the <see cref="IMansionContext"/>.</param>
		/// <param name="instanceParameterExpression">The parameter expression for the instance on which to invoke the method.</param>
		/// <param name="argumentsParameterExpression">The paremeter expression for the argument <see cref="IPropertyBag"/>.</param>
		/// <returns></returns>
		private static Expression CreateInvokeMethodExpression(MethodInfo methodInfo, Expression contextParameterExpression, Expression instanceParameterExpression, Expression argumentsParameterExpression)
		{
			// validate arguments
			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");
			if (instanceParameterExpression == null)
				throw new ArgumentNullException("instanceParameterExpression");
			if (contextParameterExpression == null)
				throw new ArgumentNullException("contextParameterExpression");
			if (argumentsParameterExpression == null)
				throw new ArgumentNullException("argumentsParameterExpression");

			// create a list of all the parameter expressions
			var parameterInfos = methodInfo.GetParameters();
			var parameterVariableList = new List<ParameterExpression>(parameterInfos.Length);
			var bodies = new List<Expression>(parameterInfos.Length + 2);
			var parameterExpressions = methodInfo.GetParameters().Select(argumentInfo => CreateArgumentExpression(argumentInfo, contextParameterExpression, argumentsParameterExpression, parameterVariableList, bodies)).ToList();

			// bake the method call
			bodies.Add(Expression.Call(instanceParameterExpression, methodInfo, parameterExpressions));

			// check for void
			var returnsNothing = methodInfo.ReturnType == typeof (void);
			if (returnsNothing)
				bodies.Add(Expression.Default(methodInfo.ReturnType));

			// construct the block
			var block = Expression.Block(parameterVariableList.ToArray(), bodies);
			return block;
		}
		/// <summary>
		/// Creates an argument expression for the given <paramref name="argumentInfo"/>.
		/// </summary>
		/// <param name="argumentInfo">The <see cref="ParameterInfo"/>.</param>
		/// <param name="contextParameterExpression">The context parameter expression.</param>
		/// <param name="argumentsParameterExpression">The arguments parameter expression.</param>
		/// <param name="parameterExpressions">List of method parameter arguments.</param>
		/// <param name="bodyExpressions">List of method parameter bodies.</param>
		/// <returns>Returns the generated <see cref="Expression"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if one of the parameters is null.</exception>
		private static Expression CreateArgumentExpression(ParameterInfo argumentInfo, Expression contextParameterExpression, Expression argumentsParameterExpression, ICollection<ParameterExpression> parameterExpressions, ICollection<Expression> bodyExpressions)
		{
			// validate arguments
			if (argumentInfo == null)
				throw new ArgumentNullException("argumentInfo");
			if (contextParameterExpression == null)
				throw new ArgumentNullException("contextParameterExpression");
			if (argumentsParameterExpression == null)
				throw new ArgumentNullException("argumentsParameterExpression");
			if (parameterExpressions == null)
				throw new ArgumentNullException("parameterExpressions");
			if (bodyExpressions == null)
				throw new ArgumentNullException("bodyExpressions");

			// check if the context is requested
			if (argumentInfo.ParameterType == typeof (IMansionContext))
				return contextParameterExpression;

			// get the tryvalueVariableExpression
			var tryGetMethodInfo = typeof (IPropertyBag).GetMethod("TryGet").MakeGenericMethod(argumentInfo.ParameterType);

			// create a literal with the argument name which is also  the property name for which to look
			var propertyNameLiteralExpression = Expression.Constant(argumentInfo.Name, typeof (string));

			// create a variable in which to store the result
			var valueVariableExpression = Expression.Variable(argumentInfo.ParameterType, "argument_" + argumentInfo.Name);

			// create a method invocation for IPropertyBag.TryGet<ArgumentInfo.ArgumentType>(context, propertyNameLiteral, out valueVariableExpression)
			var tryGetMethodCallExpression = Expression.Call(argumentsParameterExpression, tryGetMethodInfo, contextParameterExpression, propertyNameLiteralExpression, valueVariableExpression);

			// create an expression which checks if the parameter does not have a default value in which case an required argument exception is thrown
			Expression throwMissingArgumentExceptionExpression;
			if (argumentInfo.IsOptional)
				throwMissingArgumentExceptionExpression = Expression.Assign(valueVariableExpression, Expression.Constant(argumentInfo.DefaultValue, argumentInfo.ParameterType));
			else
				throwMissingArgumentExceptionExpression = Expression.Throw(Expression.New(typeof (InvalidOperationException).GetConstructor(new[] {typeof (string)}), new[] {Expression.Constant(String.Format("Argument '{0}' is required.", argumentInfo.Name))}));

			// create expression: if (tryGetMethodCallExpression) return valueVariableExpression else throw exception
			var checkForMissingArgumentExpression = Expression.IfThen(Expression.Not(tryGetMethodCallExpression), throwMissingArgumentExceptionExpression);

			// assemble the block
			bodyExpressions.Add(checkForMissingArgumentExpression);
			parameterExpressions.Add(valueVariableExpression);
			return valueVariableExpression;
		}
		#endregion
		#region Extension of IMansionContext
		/// <summary>
		/// Invokes method with name <paramref name="methodName"/> on component identified by <paramref name="componentName"/> with <paramref name="arguments"/>.
		/// </summary>
		/// <param name="context">The <see cref="IMansionContext"/>.</param>
		/// <param name="componentName">The strong name of the compoment which to resolve.</param>
		/// <param name="methodName">The name of the method which to invoke.</param>
		/// <param name="arguments">The arguments which to pass to the method.</param>
		/// <typeparam name="TResult">The type of result returned from the method.</typeparam>
		/// <returns>Returns the result.</returns>
		public static TResult InvokeMethodOnComponent<TResult>(this IMansionContext context, string componentName, string methodName, IPropertyBag arguments)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(componentName))
				throw new ArgumentNullException("componentName");
			if (string.IsNullOrEmpty(methodName))
				throw new ArgumentNullException("methodName");
			if (arguments == null)
				throw new ArgumentNullException("arguments");

			// generate the key
			var signature = componentName + "_" + methodName;

			// execute the method on the component
			return (TResult) InvokeMethodOnComponentCache.GetOrAdd(signature, key => {
				// get the component contract
				Type componentContractType;
				if (!context.Nucleus.TryResolveComponentContract(componentName, out componentContractType))
					throw new InvalidOperationException(string.Format("Could not resolve contract for component with key '{0}'", componentName));

				// create parameter expressions
				var contextParameterExpression = Expression.Parameter(typeof (IMansionContext), "context");
				var argumentsParameterExpression = Expression.Parameter(typeof (IPropertyBag), "arguments");
				var signatureParameterExpressions = new[] {contextParameterExpression, argumentsParameterExpression};

				// create the component instance variable 
				var componentInstanceVariableExpression = Expression.Variable(componentContractType, "componentInstance");
				var resolveComponentExpression = CreateResolveComponentInstanceExpression(contextParameterExpression, componentName, componentContractType, componentInstanceVariableExpression);

				// create the method call expression
				var methodInfo = componentContractType.GetMethod(methodName);
				var invokeMethodExpression = CreateInvokeMethodExpression(methodInfo, contextParameterExpression, componentInstanceVariableExpression, argumentsParameterExpression);

				// create the body
				var block = Expression.Block(new[] {componentInstanceVariableExpression}, resolveComponentExpression, invokeMethodExpression);

				// create the lambda
				var lambdaEpxression = Expression.Lambda<Func<IMansionContext, IPropertyBag, TResult>>(block, signatureParameterExpressions);

				// compile the lambda
				var typedLambda = lambdaEpxression.Compile();

				// compile and return the lambda
				return (context0, arguments0) => typedLambda(context0, arguments0);
			})(context, arguments);
		}
		/// <summary>
		/// Resolves a given component based on it's  <paramref name="componentName"/> and <paramref name="contractType"/>.
		/// </summary>
		/// <param name="contextExpression">The <see cref="IMansionContext"/> expression.</param>
		/// <param name="componentName">The strong name of the compoment which to resolve.</param>
		/// <param name="contractType">The type of contract type.</param>
		/// <param name="instanceVariableExpression">The <see cref="Expression"/> in which the instance will be stored.</param>
		/// <returns>Returns the expression.</returns>
		private static Expression CreateResolveComponentInstanceExpression(Expression contextExpression, string componentName, Type contractType, Expression instanceVariableExpression)
		{
			// create the get nucleus expression
			var getNucleusExpression = Expression.Call(contextExpression, typeof (IMansionContext).GetProperty("Nucleus").GetGetMethod());

			// create the method call to INucleus.TryResolveSingle(key, out instance)
			var resolveSingleMethod = typeof (INucleus).GetMethods().Single(candidate => candidate.Name.Equals("TryResolveSingle") && candidate.GetParameters().Length == 2).MakeGenericMethod(contractType);
			var componentKeyExpression = Expression.Constant(componentName, typeof (string));
			var resolveComponentInstanceExpression = Expression.Call(getNucleusExpression, resolveSingleMethod, componentKeyExpression, instanceVariableExpression);

			// return check for success
			return Expression.IfThen(Expression.Not(resolveComponentInstanceExpression), Expression.Throw(Expression.New(typeof (InvalidOperationException).GetConstructor(new[] {typeof (string)}), new[] {Expression.Constant(String.Format("Could not resolve component '{0}'.", componentName))})));
		}
		#endregion
		#region Private Fields
		private static readonly ConcurrentDictionary<string, Func<IMansionContext, object, IPropertyBag, object>> InvokeMethodOnInstanceCache = new ConcurrentDictionary<string, Func<IMansionContext, object, IPropertyBag, object>>(StringComparer.OrdinalIgnoreCase);
		private static readonly ConcurrentDictionary<string, Func<IMansionContext, IPropertyBag, object>> InvokeMethodOnComponentCache = new ConcurrentDictionary<string, Func<IMansionContext, IPropertyBag, object>>(StringComparer.OrdinalIgnoreCase);
		#endregion
	}
}