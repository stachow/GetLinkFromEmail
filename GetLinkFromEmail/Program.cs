using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GetLink
{
    class Program
    {
        static void Main(string[] args)
        {
            var imapClient = EmailService.GetClient(
                ConfigurationManager.AppSettings["imapServer"],
                ConfigurationManager.AppSettings["imapServerUsername"],
                ConfigurationManager.AppSettings["imapServerPassword"]);

            var emailService = new EmailService(imapClient);

            var htmlResponse = new HtmlResponseBuilder(
                emailService,
                Convert.ToInt32(ConfigurationManager.AppSettings["indexOfLinkInCollectionOfLinksInEmailBody"]), 
                Console.WriteLine);

            SimpleServer.Start(ConfigurationManager.AppSettings["localHttpAddress"], Console.WriteLine, htmlResponse.GetLatestEmailLinkAndBuildHtmlResponse);
        }
        
    }
}
