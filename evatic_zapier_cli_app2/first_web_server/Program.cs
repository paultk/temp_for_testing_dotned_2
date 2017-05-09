using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace first_web_server
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/test/");
            //Console.WriteLine(ws.TryApi());
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            string errorMessage = "";


            /*
     try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                m2MServiceAgent.ClientCredentials.UserName.UserName = "evatic_user2";
                m2MServiceAgent.ClientCredentials.UserName.Password = "Myevaticpassword94";
                var result = m2MServiceAgent.Echo("testdfghdfhdfhfg");
                //var result2 = m2MServiceAgent.GetEvaticMachine(1, new Guid("1D656EB8 - D1A3 - B38A - A942 - 09290FE1568C"));

                //var result2 = CustomerService.CustomerSearchResponseDataContract.
                Console.WriteLine(result);

                Thread.Sleep(4444);

                //Assert.IsNotNull(result);
            } catch (Exception e) { Console.WriteLine(e.Message); }
        }
         */
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                external_helpdesk_ev.ExternalHelpdeskServiceClient hlp = new external_helpdesk_ev.ExternalHelpdeskServiceClient();

                hlp.ClientCredentials.UserName.UserName = "evatic_user2";
                hlp.ClientCredentials.UserName.Password = "Myevaticpassword94";
                string echo = hlp.Echo("sdfsdf");

                Console.WriteLine("body:");
                System.IO.Stream body = request.InputStream;
                StreamReader reader = new StreamReader(body, request.ContentEncoding);
                string s = reader.ReadToEnd();
                Console.WriteLine(s);

                return string.Format("<HTML><BODY>echo:\n" + echo + ".<br>{0}</BODY></HTML>", DateTime.Now);
            }
            catch (Exception e) {
                errorMessage = e.Message;
                Console.WriteLine(e.Message); }

            return errorMessage;
        }
    }
}
