using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using GetLink;

namespace LUSI.TestSupport.EmailService.Tests
{
    
    [TestClass()]
    public class StringUtilsTest
    {
        [TestMethod()]
        public void Should_Return_Zero_Hyperlinks_From_Empty_Body()
        {
            string bodyText = string.Empty; 
                        
            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(0, links.Count);
        }

        [TestMethod()]
        public void Should_Return_Zero_Hyperlinks_From_Body_With_No_Link()
        {
            string bodyText = "the cat sat on the mat";

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(0, links.Count);
        }

        [TestMethod()]
        public void Should_Return_One_Hyperlink_From_Body_With_Local_Link()
        {
            string link = "http://somelinkwithnodots";
            string bodyText = string.Format("the cat sat {0} on the mat", link);

                       
            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlink_From_Body_With_Domain_Link()
        {
            string link = "http://somelink.with.dots";
            string bodyText = string.Format("the cat sat {0} on the mat", link);

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlink_From_Body_With_Domain_Link_With_Path()
        {
            string link = "http://somelink.with.dots/and/path";
            string bodyText = string.Format("the cat sat {0} on the mat", link);

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlink_From_Body_With_Domain_Link_With_Query_String()
        {
            string link = "http://somelink.with.dots?and=query";
            string bodyText = string.Format("the cat sat {0} on the mat", link);

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlink_From_Body_With_Domain_Link_With_Path_And_Query_String()
        {
            string link = "http://somelink.with.dots/and/path?and=query";
            string bodyText = string.Format("the cat sat {0} on the mat", link);

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlink_From_Body_That_Is_Just_Link()
        {
            string link = "http://somelink.with.dots/and/path?and=query";
            string bodyText = link;

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(1, links.Count);
            Assert.AreEqual(link, links.First());
        }

        [TestMethod()]
        public void Should_Return_Hyperlinks_From_Body_With_Multiple_Hyperlinks()
        {
            string link = "http://somelink.with.dots/and/path?and=query";
            string secondlink = "http://otherlink.with.dots/and/path?and=query";
            string bodyText = string.Format("the cat sat {0} on {1} the mat", link, secondlink);

            var links = StringUtils.GetHyperlinksFromBodyText(bodyText);

            Assert.AreEqual(2, links.Count);
            Assert.AreEqual(link, links.First());
        }
    }
}
