using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HelseId.Samples.SamlApiClient
{
    static class Extensions
    {
        public static async Task<string> AsStringAsync(this HttpListenerRequest request)
        {
            using (var reader = new StreamReader(request.InputStream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static string IndentXml(this string xml)
        {
            if(string.IsNullOrEmpty(xml))
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
