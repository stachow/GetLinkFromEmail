using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using S22.Imap;
using System.Net.Mail;
using Moq;
using System.Collections.Generic;
using GetLink;

namespace GetLinkTests
{
    
    [TestClass()]
    public class EmailRetrieverTest
    {
        [TestMethod()]
        [ExpectedException(typeof (ArgumentNullException), "Constructor must not receive a null")]
        public void Should_Throw_With_Null_Client()
        {
            IImapClient client = null; 
            var target = new EmailService(client);
        }

        
        [TestMethod()]
        public void Should_Return_False_And_Not_Get_Message_When_No_Message_Waiting()
        {
            var mock = new Mock<IImapClient>();
            mock.Setup(m => m.Search(It.IsAny<SearchCondition>(), null)).Returns((new uint[] {}));
            mock.Setup(m => m.GetMessage(It.IsAny<uint>(), true, null)).Throws(new Exception("Should not be called"));
            var client = mock.Object;
            
            var e = new EmailService(client);

            string body;
            var result = e.TryGetMostRecentUnreadEmailBodyForAddressAndThenSetAsRead("dummy@b.com", out body);
            
            Assert.AreEqual(result, false);
        }

        [TestMethod()]
        public void Should_Return_True_And_Get_Message_When_Message_Waiting()
        {
            var targetMessage = new MailMessage();

            var messages = new Dictionary<uint, MailMessage> { 
                { 4121343123U, targetMessage }
            };

            var mock = new Mock<IImapClient>();
            mock.Setup(m => m.Search(It.IsAny<SearchCondition>(), null)).Returns(messages.Keys);
            mock.Setup(m => m.GetMessage(It.IsAny<uint>(), false, null)).Returns((uint id, bool dummy1, string dummy2) =>  messages[id]);
            var client = mock.Object;

            var e = new EmailService(client);

            string body;
            var result = e.TryGetMostRecentUnreadEmailBodyForAddressAndThenSetAsRead("dummy@b.com", out body);

            Assert.AreEqual(result, true);
            Assert.AreEqual(body, targetMessage.Body);
        }

        [TestMethod()]
        public void Should_Return_True_And_Get_Correct_Message_When_Messages_Waiting()
        {

            var targetMessage = new MailMessage();

            var messages = new Dictionary<uint, MailMessage> { 
                { 1, new MailMessage() },
                { 2, new MailMessage() },
                { 4, targetMessage },
                { 3, new MailMessage() }
            };

            var mock = new Mock<IImapClient>();
            mock.Setup(m => m.Search(It.IsAny<SearchCondition>(), null)).Returns(messages.Keys);
            mock.Setup(m => m.GetMessage(It.IsAny<uint>(), false, null)).Returns((uint id, bool dummy1, string dummy2) => messages[id]);
            var client = mock.Object;

            var e = new EmailService(client);

            string body;
            var result = e.TryGetMostRecentUnreadEmailBodyForAddressAndThenSetAsRead("dummy@b.com", out body);

            Assert.AreEqual(result, true);
            Assert.AreEqual(body, targetMessage.Body);
        }
    }
}
