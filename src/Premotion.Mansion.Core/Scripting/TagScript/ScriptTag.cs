using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml;
using Premotion.Mansion.Core.Collections;
using Premotion.Mansion.Core.IO.Memory;
using Premotion.Mansion.Core.Scripting.ExpressionScript;

namespace Premotion.Mansion.Core.Scripting.TagScript
{
	/// <summary>
	/// Defines the base class for all script tags.
	/// </summary>
	public abstract class ScriptTag : IScript
	{
		#region Initialize Methods
		/// <summary>
		/// Initializes this script tag.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="node">The XML node fromwhich this tag is constructed.</param>
		public void Initialize(IContext context, XmlNode node)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");
			if (node == null)
				throw new ArgumentNullException("node");

			// set value
			xmlNode = node;

			// loop through all the attributes to create expression from them
			foreach (var attribute in from XmlAttribute attribute in XmlNode.Attributes where !attribute.Prefix.Equals("xmlns", StringComparison.OrdinalIgnoreCase) && !attribute.LocalName.Equals("xmlns", StringComparison.OrdinalIgnoreCase) select attribute)
				attributes.Add(attribute.LocalName, attribute.Value);
		}
		/// <summary>
		/// Initializes this tag in the correct context.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="tagScript">The script to which the tag belongs.</param>
		public virtual void InitializeContext(MansionContext context, TagScript tagScript)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (tagScript == null)
				throw new ArgumentNullException("tagScript");

			// initialize al children
			foreach (var child in childTags)
				child.InitializeContext(context, tagScript);
			foreach (var child in alternativeBranches)
				child.InitializeContext(context, tagScript);
		}
		#endregion
		#region Child Tag Methods
		/// <summary>
		/// Adds a child tag to this tag.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="childTag">The child tag.</param>
		public void Add(IContext context, ScriptTag childTag)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (childTag == null)
				throw new ArgumentNullException("childTag");

			// check if the child tag is an alternative branch
			var alternativeTag = childTag as AlternativeScriptTag;
			if (alternativeTag != null)
				alternativeBranches.Add(alternativeTag);
			else
				childTags.Add(childTag);
		}
		/// <summary>
		/// Gets the child tag with the specified name.
		/// </summary>
		/// <typeparam name="TTagType">The type of <see cref="AlternativeScriptTag"/>.</typeparam>
		/// <param name="branch">The <see cref="AlternativeScriptTag"/> of type <typeparamref name="TTagType"/>.</param>
		/// <returns>Returns true when the branch tag was found, otherwise false.</returns>
		protected bool TryGetAlternativeChildTag<TTagType>(out TTagType branch) where TTagType : AlternativeScriptTag
		{
			// get the tag
			branch = (from candidate in alternativeBranches
			          where candidate is TTagType
			          select (TTagType) candidate).SingleOrDefault();

			// check if a branch was found
			return branch != null;
		}
		/// <summary>
		/// Gets the child tag of the specified type.
		/// </summary>
		/// <typeparam name="TTagType">The type of tag which to get</typeparam>
		/// <returns>Returns the <typeparamref name="TTagType"/> instance.</returns>
		/// <exception cref="InvalidOperationException">Thrown when no tag of type <typeparamref name="TTagType"/> could be found.</exception>
		protected TTagType GetAlternativeChildTag<TTagType>() where TTagType : AlternativeScriptTag
		{
			// get the tag
			var branch = (from candidate in alternativeBranches
			              where candidate is TTagType
			              select (TTagType) candidate).SingleOrDefault();

			// check if a branch was found
			if (branch == null)
				throw new InvalidOperationException(string.Format("Could not find an alternative tag of type '{0}'", typeof (TTagType)));

			// return the alternative branche
			return branch;
		}
		/// <summary>
		/// Gets the child tags of the specified type.
		/// </summary>
		/// <typeparam name="TTagType">The type of tag which to get</typeparam>
		/// <returns></returns>
		protected IEnumerable<TTagType> GetAlternativeChildren<TTagType>() where TTagType : AlternativeScriptTag
		{
			return (from candidate in alternativeBranches
			        where candidate is TTagType
			        select (TTagType) candidate);
		}
		#endregion
		#region Attribute Methods
		/// <summary>
		/// Gets the value of the attribute.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="attributeName">The name of the attribute from which to get the value.</param>
		/// <returns>Returns the value.</returns>
		public TValue GetAttribute<TValue>(MansionContext context, string attributeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			// check if the tag does not have the attribute
			string attributeValue;
			if (!attributes.TryGetValue(attributeName, out attributeValue))
				return default(TValue);

			// get the expression script parser and parse the expression
			var expressionScriptParser = context.Nucleus.Get<IExpressionScriptService>(context);
			var expression = expressionScriptParser.Parse(context, new LiteralResource(attributeValue));

			// return the parsed expression
			return expression.Execute<TValue>(context);
		}
		/// <summary>
		/// Gets the value of the attribute.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="attributeName">The name of the attribute from which to get the value.</param>
		/// <param name="defaultValue">The default value of this attribute.</param>
		/// <returns>Returns the value.</returns>
		protected TValue GetAttribute<TValue>(MansionContext context, string attributeName, TValue defaultValue)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			// check if the tag does not have the attribute
			string attributeValue;
			if (!attributes.TryGetValue(attributeName, out attributeValue))
				return defaultValue;

			// get the expression script parser and parse the expression
			var expressionScriptParser = context.Nucleus.Get<IExpressionScriptService>(context);
			var expression = expressionScriptParser.Parse(context, new LiteralResource(attributeValue));

			// return the parsed expression
			return expression.Execute<TValue>(context);
		}
		/// <summary>
		/// Gets the value of the attribute.
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <param name="attributeName">The name of the attribute from which to get the value.</param>
		/// <returns>Returns the value.</returns>
		/// <exception cref="AttributeNotSpecifiedException">Thrown when an attribute with the name <paramref name="attributeName"/> is not found.</exception>
		/// <exception cref="AttributeNullException">Thrown when the value of <paramref name="attributeName"/> is null.</exception>
		protected TValue GetRequiredAttribute<TValue>(MansionContext context, string attributeName)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (string.IsNullOrEmpty(attributeName))
				throw new ArgumentNullException("attributeName");

			// check if the tag does not have the attribute
			string attributeValue;
			if (!attributes.TryGetValue(attributeName, out attributeValue))
				throw new AttributeNotSpecifiedException(attributeName, this);

			// get the expression script parser and parse the expression
			var expressionScriptParser = context.Nucleus.Get<IExpressionScriptService>(context);
			var expression = expressionScriptParser.Parse(context, new LiteralResource(attributeValue));

			// get the parsed expression
			var value = expression.Execute<TValue>(context);

			// check for null value
			if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)) && !typeof (TValue).IsValueType)
				throw new AttributeNullException(attributeName, this);
			if (typeof (TValue) == typeof (String) && string.IsNullOrEmpty(value as string))
				throw new AttributeNullException(attributeName, this);

			// return the value
			return value;
		}
		/// <summary>
		/// Gets all the evaluated attributes of this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns></returns>
		protected IPropertyBag GetAttributes(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// get all the attributes
			var attributeValues = new PropertyBag();
			foreach (var attributeName in attributes.Keys)
				attributeValues.Set(attributeName, GetAttribute<object>(context, attributeName));

			return attributeValues;
		}
		#endregion
		#region Content Methods
		/// <summary>
		/// Gets the evaluated content of this tag.
		/// </summary>
		/// <typeparam name="TValue">The type of value which to get.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the value.</returns>
		protected TValue GetContent<TValue>(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// check if the first element is a CDTATA section
			var content = (XmlNode.HasChildNodes && XmlNode.FirstChild.NodeType == XmlNodeType.CDATA) ? XmlNode.FirstChild.InnerText : XmlNode.InnerText;

			// get the expression script parser and parse the expression
			var expressionScriptParser = context.Nucleus.Get<IExpressionScriptService>(context);
			var expression = expressionScriptParser.Parse(context, new LiteralResource(content));

			// return the parsed expression
			return expression.Execute<TValue>(context);
		}
		#endregion
		#region Execute Methods
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public virtual void Execute(MansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// default behavior
			try
			{
				DebugTag(context);
				DoExecute(context);
			}
			catch (ThreadAbortException)
			{
				// thread is aborted, so don't throw any new exceptions
			}
			catch (ScriptTagException ex)
			{
				// limit the stack trace depth
				if (ex.ScriptStackTrace.Count > 20)
					throw;

				// add this tag to the stack trace
				ex.ScriptStackTrace.Add(this);
				throw;
			}
			catch (Exception ex)
			{
				throw new ScriptTagException(this, ex);
			}
		}
		/// <summary>
		/// Executes this script.
		/// </summary>
		/// <typeparam name="TResult">The result type.</typeparam>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the result of this script expression.</returns>
		/// <exception cref="ScriptExecutionException">Thrown when an exception occured while executing this script.</exception>
		public TResult Execute<TResult>(MansionContext context)
		{
			throw new NotSupportedException();
		}
		/// <summary>
		/// Executes this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected abstract void DoExecute(MansionContext context);
		/// <summary>
		/// Executes the child tags of this tag.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		protected void ExecuteChildTags(MansionContext context)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");

			// loop through all the child tags
			foreach (var childTag in ChildTags.TakeWhile(childTag => !context.HaltExecution))
				childTag.Execute(context);
		}
		#endregion
		#region Debug Methods
		/// <summary>
		/// Halts the current execution flow and breaks into the debugger.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		[Conditional("DEBUG"), DebuggerHidden]
		private void DebugTag(MansionContext context)
		{
			// validate argument
			if (context == null)
				throw new ArgumentNullException("context");

			// check if debugger attribute is not set
			if (!GetAttribute<bool>(context, "debug"))
				return;

			// attach debugger when needed
			if (!Debugger.IsAttached)
				Debugger.Launch();

			// breakpoint
			Debugger.Break();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the default child tags of this tag.
		/// </summary>
		private IEnumerable<ScriptTag> ChildTags
		{
			get { return childTags; }
		}
		/// <summary>
		/// Gets the XML node of this tag.
		/// </summary>
		private XmlNode XmlNode
		{
			get { return xmlNode; }
		}
		/// <summary>
		/// Gets the <see cref="TagInfo"/> of this tag.
		/// </summary>
		public TagInfo Info
		{
			get { return tagInfo; }
		}
		#endregion
		#region Private Fields
		private readonly ICollection<AlternativeScriptTag> alternativeBranches = new List<AlternativeScriptTag>();
		private readonly IDictionary<string, string> attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		private readonly ICollection<ScriptTag> childTags = new List<ScriptTag>();
		private readonly TagInfo tagInfo = new TagInfo();
		private XmlNode xmlNode;
		#endregion
	}
}