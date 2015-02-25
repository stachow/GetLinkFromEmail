using System;
using System.Linq;
using S22.Imap;
using System.Net.Mail;


namespace GetLink
{
    public class EmailService
    {
        private IImapClient _imapClient;

        public EmailService(IImapClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            _imapClient = client;
        }

        public bool TryGetMostRecentUnreadEmailBodyForAddressAndThenSetAsRead(string emailAddress, out string body)
        {
            // Notethat if emailAddress is empty then all emails are examined, regardless if they were 
            // to address+1@gmail or address+2@gmail.com etc.

            var messageIds = _imapClient.Search(SearchCondition.To(emailAddress).And(SearchCondition.Unseen()));
            var messageId = messageIds.OrderByDescending(id => id).FirstOrDefault();

            if (messageId <= 0)
            {
                body = null;
                return false;
            }

            var message = _imapClient.GetMessage(messageId, false, null);
            body = message.Body;

            _imapClient.SetMessageFlags(messageId, null, new MessageFlag[] { MessageFlag.Seen });

            return true;
        }

        public static IImapClient GetClient(string server, string username, string password)
        {
            return new ImapClient(server, 993, username, password, AuthMethod.Auto, true);
        }

    }
}
