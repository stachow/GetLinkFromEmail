using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GetLink
{
    public static class StringUtils
    { 
        public static List<string> GetHyperlinksFromBodyText(string bodyText)
        {
            var regex = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,:]*", 
                RegexOptions.Singleline | RegexOptions.CultureInvariant);

            var list = new List<string>();

            if (regex.IsMatch(bodyText))
            {
                foreach (Match match in regex.Matches(bodyText))
                {
                    list.Add(match.Groups[0].Value);
                }
            }

            return list;
        }
    }
}
