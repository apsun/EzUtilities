using System;
using System.Xml.Linq;

namespace EzUtilities
{
    public static class XDocumentUtilities
    {
        /// <summary>
        /// The exception that is thrown when an element was not found in an XElement.
        /// </summary>
        public class XElementNotFoundException : Exception
        {
            private readonly string _parentName;
            private readonly string _elementName;

            public string ParentName
            {
                get { return _parentName; }
            }
            public string ElementName
            {
                get { return _elementName; }
            }

            public XElementNotFoundException(string parentName, string elementName)
                : base("Element '" + elementName + "' was not found in parent element '" + parentName + "'")
            {
                _parentName = parentName;
                _elementName = elementName;
            }
        }

        /// <summary>
        /// Adds a new element to this node with the specified name and value. 
        /// Returns the added element.
        /// </summary>
        /// <param name="parentNode">The node to add the element to.</param>
        /// <param name="name">The name of the new element.</param>
        /// <param name="value">The value of the new element.</param>
        public static XElement AddElement(this XElement parentNode, string name, object value)
        {
            XElement element = new XElement(name, value);
            parentNode.Add(element);
            return element;
        }

        /// <summary>
        /// Adds a new element with multiple values to this element. 
        /// Returns the added element.
        /// </summary>
        /// <param name="parentNode">The node to add the element to.</param>
        /// <param name="name">The name of the new element.</param>
        /// <param name="values">The values of the new element.</param>
        public static XElement AddElement(this XElement parentNode, string name, params object[] values)
        {
            XElement element = new XElement(name, values);
            parentNode.Add(element);
            return element;
        }

        /// <summary>
        /// Gets the value of a specified child element.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="name">The name of the child node to find.</param>
        public static string GetChildValue(this XElement parentNode, string name)
        {
            XElement element = parentNode.Element(name);
            if (element == null) throw new XElementNotFoundException(parentNode.GetName(), name);
            return element.Value;
        }

        /// <summary>
        /// Sets the value of a specified child element. 
        /// Returns the child element.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the node whose value you want to change.</param>
        /// <param name="value">The new value of the child node.</param>
        public static XElement SetChildValue(this XElement parentNode, string nodeName, object value)
        {
            XElement element = parentNode.Element(nodeName);
            if (element == null) element = parentNode.AddElement(nodeName, value);
            else element.SetValue(value);
            return element;
        }

        /// <summary>
        /// Sets multiple values of a specified child element. 
        /// Returns the child element.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeName">The name of the node whose value you want to change.</param>
        /// <param name="values">The new values of the child node.</param>
        public static XElement SetChildValue(this XElement parentNode, string nodeName, params object[] values)
        {
            XElement element = parentNode.Element(nodeName);
            if (element == null) element = parentNode.AddElement(nodeName, values);
            else element.SetValue(values);
            return element;
        }

        /// <summary>
        /// Gets an element with the specified name, creating it if it does not exist.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="name">The name of the element to find.</param>
        public static XElement TryGetElement(this XElement parentNode, string name)
        {
            XElement element = parentNode.Element(name) ?? parentNode.AddElement(name);
            return element;
        }

        /// <summary>
        /// Gets an element with the specified name.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="name">The name of the element to find.</param>
        public static XElement GetElement(this XElement parentNode, string name)
        {
            XElement element = parentNode.Element(name);
            if (element == null) throw new XElementNotFoundException(parentNode.GetName(), name);
            return element;
        }

        /// <summary>
        /// Gets the name of this element.
        /// </summary>
        public static string GetName(this XElement element)
        {
            return element.Name.ToString();
        }
    }
}