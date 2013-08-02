using System;
using System.Xml.Linq;

namespace EzUtilities
{
    /// <summary>
    /// Provides tools for working with <see cref="System.Xml.Linq.XElement"/> objects.
    /// </summary>
    public static class XmlUtilities
    {
        /// <summary>
        /// The exception that is thrown when an element was not found.
        /// </summary>
        public class ElementNotFoundException : Exception
        {
            private readonly string _parentName;
            private readonly string _elementName;

            /// <summary>
            /// Gets the name of the parent element containing the child element.
            /// </summary>
            public string ParentName
            {
                get { return _parentName; }
            }
            /// <summary>
            /// Gets the name of the child element that was not found.
            /// </summary>
            public string ElementName
            {
                get { return _elementName; }
            }

            /// <summary>
            /// Instantiates a new instance of the <see cref="ElementNotFoundException"/> class.
            /// </summary>
            public ElementNotFoundException()
                : base("Child element was not found")
            {

            }

            /// <summary>
            /// Instantiates a new instance of the <see cref="ElementNotFoundException"/> class.
            /// </summary>
            /// <param name="parentName">The name of the parent element.</param>
            /// <param name="elementName">The name of the child element.</param>
            public ElementNotFoundException(string parentName, string elementName)
                : base("Child element '" + elementName + "' was not found in parent element '" + parentName + "'")
            {
                _parentName = parentName;
                _elementName = elementName;
            }
        }

        /// <summary>
        /// Creates an empty XDocument with the specified root node name.
        /// </summary>
        /// 
        /// <param name="rootNodeName">The name of the root node.</param>
        /// <param name="rootNode">The generated root node.</param>
        /// 
        /// <returns>The generated XDocument.</returns>
        public static XDocument CreateDocumentFromRoot(string rootNodeName, out XElement rootNode)
        {
            rootNode = new XElement(rootNodeName);
            return new XDocument(rootNode);
        }

        /// <summary>
        /// Adds a new element with the specified name and value to this element.
        /// </summary>
        /// 
        /// <param name="parent">The element to add the new child element to.</param>
        /// <param name="name">The name of the new element.</param>
        /// <param name="value">The value of the new element.</param>
        /// 
        /// <returns>The added element.</returns>
        public static XElement AddElement(this XElement parent, string name, object value)
        {
            var element = new XElement(name, value);
            parent.Add(element);
            return element;
        }

        /// <summary>
        /// Adds a new element with multiple values to this element.
        /// </summary>
        /// 
        /// <param name="parent">The element to add the new child element to.</param>
        /// <param name="name">The name of the new element.</param>
        /// <param name="values">The values of the new element.</param>
        /// 
        /// <returns>The added element.</returns>
        public static XElement AddElement(this XElement parent, string name, params object[] values)
        {
            var element = new XElement(name, values);
            parent.Add(element);
            return element;
        }

        /// <summary>
        /// Gets the value of a specified child element.
        /// </summary>
        /// 
        /// <param name="parent">The element that contains the child element.</param>
        /// <param name="name">The name of the child element to find.</param>
        /// 
        /// <returns>The value of the child element, if found.</returns>
        /// 
        /// <exception cref="ElementNotFoundException">Thrown if the child element was not found.</exception>
        public static string GetChildValue(this XElement parent, string name)
        {
            XElement element = parent.Element(name);
            if (element == null) throw new ElementNotFoundException(parent.GetName(), name);
            return element.Value;
        }

        /// <summary>
        /// Sets the value of a specified child element.
        /// </summary>
        /// 
        /// <param name="parent">The element that contains the child element.</param>
        /// <param name="name">The name of the child element whose value you want to change.</param>
        /// <param name="value">The new value of the child element.</param>
        /// 
        /// <returns>The child element.</returns>
        public static XElement SetChildValue(this XElement parent, string name, object value)
        {
            XElement element = parent.Element(name);
            if (element == null) element = parent.AddElement(name, value);
            else element.SetValue(value);
            return element;
        }

        /// <summary>
        /// Sets multiple values of a specified child element.
        /// </summary>
        /// 
        /// <param name="parent">The element that contains the child element.</param>
        /// <param name="name">The name of the child element whose value you want to change.</param>
        /// <param name="values">The new values of the child element.</param>
        /// 
        /// <returns>The child element.</returns>
        public static XElement SetChildValue(this XElement parent, string name, params object[] values)
        {
            XElement element = parent.Element(name);
            if (element == null) element = parent.AddElement(name, values);
            else element.SetValue(values);
            return element;
        }

        /// <summary>
        /// Gets an element with the specified name, creating it if it does not exist.
        /// </summary>
        /// 
        /// <param name="parent">The element that contains the child element.</param>
        /// <param name="name">The name of the child element to find.</param>
        /// 
        /// <returns>The element, if found; otherwise, a new empty element.</returns>
        public static XElement TryGetElement(this XElement parent, string name)
        {
            XElement element = parent.Element(name) ?? parent.AddElement(name);
            return element;
        }

        /// <summary>
        /// Gets an element with the specified name.
        /// </summary>
        /// 
        /// <param name="parent">The element that contains the child element.</param>
        /// <param name="name">The name of the child element to find.</param>
        /// 
        /// <returns>The element, if found.</returns>
        /// 
        /// <exception cref="ElementNotFoundException">Thrown if the element was not found.</exception>
        public static XElement GetElement(this XElement parent, string name)
        {
            XElement element = parent.Element(name);
            if (element == null) throw new ElementNotFoundException(parent.GetName(), name);
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