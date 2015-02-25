using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GetLink
{
    public class HtmlResponseBuilder
    {
        Action<string> _feedback;

        EmailService _emailService;

        int _indexOfLinkInCollectionOfLinksInEmailBody;

        public HtmlResponseBuilder(EmailService emailService, int indexOfLinkInCollectionOfLinksInEmailBody, Action<string> feedback)
        {
            _feedback = feedback;
            _indexOfLinkInCollectionOfLinksInEmailBody = indexOfLinkInCollectionOfLinksInEmailBody;
            _emailService = emailService;
        }

        public string GetLatestEmailLinkAndBuildHtmlResponse(string emailAddress)
        {
            string messageBody;

            if (!_emailService.TryGetMostRecentUnreadEmailBodyForAddressAndThenSetAsRead(emailAddress, out messageBody))
            {
                _feedback("No email found yet.");
                return string.Format("<html><meta http-equiv='refresh' content='{0}'>No email yet.<br/><br/>Waiting for {0} seconds ...", 5) ;
            }

            _feedback("Got email");

            var links = StringUtils.GetHyperlinksFromBodyText(messageBody);
            if (!links.Any())
            {
                _feedback("No url found in email");
                _feedback(messageBody);
                return "<html><meta http-equiv='refresh' content='{0}'>Email found but no hyperlink. ARE YOU SURE EVERYTHING IS OK?<br/><br/>Waiting for {0} seconds ...";
            }

            if (links.Count < (_indexOfLinkInCollectionOfLinksInEmailBody + 1))
            {
                _feedback(string.Format("We want the {0}th url but only {2} have been found", _indexOfLinkInCollectionOfLinksInEmailBody, links.Count));
                return string.Format("<html><meta http-equiv='refresh' content='{0}'>Email found but your hyperlink is not there. ARE YOU SURE EVERYTHING IS OK?<br/><br/>Waiting for {0} seconds ...", 5);
            }

            var url = links[_indexOfLinkInCollectionOfLinksInEmailBody];

            _feedback(string.Format("Found {0}", url));

            return string.Format("<html><a href='{0}' id='link'>{0}</a>", url);
        }
    }
}
