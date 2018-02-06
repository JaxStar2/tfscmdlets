using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TfsCmdlets.Models
{
    [XmlRoot("gl:GLOBALLISTS", Namespace = "http://schemas.microsoft.com/VisualStudio/2005/workitemtracking/globallists")]
    public class GlobalListCollection : List<GlobalList>
    {
        public static GlobalListCollection Parse(string xml)
        {
            var serializer = new XmlSerializer(typeof(GlobalListCollection));

            return (GlobalListCollection) serializer.Deserialize(new StringReader(xml));
        }

        public string ToXml()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("gl", "http://schemas.microsoft.com/VisualStudio/2005/workitemtracking/globallists");

            var serializer = new XmlSerializer(typeof(GlobalListCollection));
            var writer = new StringWriter();

            serializer.Serialize(writer, this, ns);

            return writer.ToString();
        }
    }
}
