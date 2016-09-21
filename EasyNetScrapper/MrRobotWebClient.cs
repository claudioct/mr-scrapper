using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyNetScrapper
{
    public class MrRobotWebClient : WebClient
    {
        //Criei estas propriedades apenas para controle.
        //Caso preciso expor essas propriedades para alteração basta tirar o readonly e alterar para public.
        //Lembre-se os sites podem variar e muito, então expor uma variável pode deixar seu código mais flexivel.
        public CookieContainer _cookie = new CookieContainer();

        public bool _allowAutoRedirect;

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                var unboxedRequest = request as HttpWebRequest;
                unboxedRequest.ServicePoint.Expect100Continue = false;
                unboxedRequest.CookieContainer = _cookie;
                unboxedRequest.KeepAlive = false;
                unboxedRequest.AllowAutoRedirect = _allowAutoRedirect;               
            }
            return request;
        }
    }
}
