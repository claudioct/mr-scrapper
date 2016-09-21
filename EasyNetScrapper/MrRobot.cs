using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetScrapper
{
    public class MrRobot
    {
        public MrRobotWebClient MrRobotWebClient { get; set; }


        /// <summary>
        /// Métodos para efetuar chamadas via GET.
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <returns>HtmlDocumento - utilizado para facilitar o parse do Html.</returns>
        public HtmlDocument HttpGet (string url)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(MrRobotWebClient.DownloadString(url));            
            return htmlDocument;
        }

        /// <summary>
        /// Método para efetuar chamadas via POST
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <param name="parametros">Parâmetros da página.</param>
        /// <returns>HtmlDocumento - utilizado para facilitar o parse do Html.</returns>
        public HtmlDocument HttpPost (string url, NameValueCollection paramCollection)
        {
            var htmlDocument = new HtmlDocument();
            byte[] pagina = MrRobotWebClient.UploadValues(url, paramCollection);
            htmlDocument.LoadHtml(Encoding.Default.GetString(pagina, 0, pagina.Count()));

            return htmlDocument;
        }
    }
}
