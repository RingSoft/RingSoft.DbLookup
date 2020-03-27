using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RingSoft.DbLookup.App.Library
{
    public class XmlProcessor
    {
        public XElement RootElement { get; private set; }

        public XmlProcessor(string rootName)
        {
            RootElement = new XElement(rootName);
        }

        public string GetElementValue(string name, string defaultValue)
        {
            var element = GetElement(name);
            if (element == null)
                return defaultValue;

            return element.Value;
        }

        private XElement GetElement(string name)
        {
            var element = RootElement.Element(name);
            return element;
        }

        public void SetElementValue(string name, string value)
        {
            var element = GetElement(name);
            if (element == null)
            {
                element = new XElement(name);
                RootElement.Add(element);
            }

            element.Value = value;
        }

        public void LoadFromXml(string xml)
        {
            try
            {
                RootElement = XElement.Parse(xml);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string OutputXml()
        {
            string xml;
            var stringBuilder = new StringBuilder();
            var stringWriter = new System.IO.StringWriter(stringBuilder);
            var xmlTextWriter = new XmlTextWriter(stringWriter) {Formatting = Formatting.Indented};
            RootElement.WriteTo(xmlTextWriter);
            xml = stringWriter.ToString();

            return xml;
        }
    }
}
