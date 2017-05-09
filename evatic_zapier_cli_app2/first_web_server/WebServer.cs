//using first_web_server.external_helpdesk_ev;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace first_web_server
{
    class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;
        external_helpdesk_ev.ExternalHelpdeskServiceClient hlp = new external_helpdesk_ev.ExternalHelpdeskServiceClient();

        public WebServer(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            hlp.ClientCredentials.UserName.UserName = "evatic_user2";
            hlp.ClientCredentials.UserName.Password = "Myevaticpassword94";

            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _responderMethod(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

    


        /**public string TryApi()
        {

            
            return hlp.Echo("sdfsdf");
            //ExternalHelpdeskCreateOrUpdateRequestDataContract first_contract = new ExternalHelpdeskCreateOrUpdateRequestDataContract();
        }

        /*public string SendResponse(HttpListenerRequest request)
        {
            Console.WriteLine("body:");
            System.IO.Stream body = request.InputStream;
            StreamReader reader = new StreamReader(body, request.ContentEncoding);
            string s = reader.ReadToEnd();
            Console.WriteLine(s);

            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }*/
    }
}

