using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace AppCore.Utilities
{
    public static class HtmlUtilities
    {
        public static string TruncateChar(this string text, int charCount)
        {
            bool inTag = false;
            int cntr = 0;
            int cntrContent = 0;

            // loop through html, counting only viewable content
            foreach (Char c in text)
            {
                if (cntrContent == charCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag) cntrContent++;
            }

            string substr = text.Substring(0, cntr);

            //search for nonclosed tags        
            MatchCollection openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            MatchCollection closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            Stack<string> opentagsStack = new Stack<string>();
            Stack<string> closedtagsStack = new Stack<string>();

            // to be honest, this seemed like a good idea then I got lost along the way 
            // so logic is probably hanging by a thread!! 
            foreach (Match tag in openedTags)
            {
                string openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    string tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr + " ...";
        }

        public static string TruncateWord(this string text, int wordCount)
        {
            bool inTag = false;
            int cntr = 0;
            int cntrWords = 0;
            Char lastc = ' ';

            // loop through html, counting only viewable content
            foreach (Char c in text)
            {
                if (cntrWords == wordCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag)
                {
                    // do not count double spaces, and a space not in a tag counts as a word
                    if (c == 32 && lastc != 32)
                        cntrWords++;
                }
            }

            string substr = text.Substring(0, cntr);

            //search for nonclosed tags        
            MatchCollection openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            MatchCollection closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            Stack<string> opentagsStack = new Stack<string>();
            Stack<string> closedtagsStack = new Stack<string>();

            foreach (Match tag in openedTags)
            {
                string openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    string tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr + " ...";
        }

        public static string TruncateXML(this string text, int charCount)
        {
            // your data, probably comes from somewhere, or as params to a methodint 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            // create a navigator, this is our primary tool
            XPathNavigator navigator = xml.CreateNavigator();
            XPathNavigator breakPoint = null;

            // find the text node we need:
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {
                string lastText = navigator.Value.Substring(0, Math.Min(charCount, navigator.Value.Length));
                charCount -= navigator.Value.Length;
                if (charCount <= 0)
                {
                    // truncate the last text. Here goes your "search word boundary" code:        
                    navigator.SetValue(lastText);
                    breakPoint = navigator.Clone();
                    break;
                }
            }

            // first remove text nodes, because Microsoft unfortunately merges them without asking
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent, then move the rest
            navigator.MoveTo(breakPoint);
            while (navigator.MoveToFollowing(XPathNodeType.Element))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent
            // then remove *all* empty nodes to clean up (not necessary):
            // TODO, add empty elements like <br />, <img /> as exclusion
            navigator.MoveToRoot();
            while (navigator.MoveToFollowing(XPathNodeType.Element))
            {
                while (!navigator.HasChildren && (navigator.Value ?? "").Trim() == "")
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent
            navigator.MoveToRoot();
            return navigator.InnerXml + " ...";
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(this string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(this string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string GetYouTubeVideoId(this string url)
        {
            var uri = new Uri(url);
            // you can check host here => uri.Host <= "www.youtube.com"
            var query = HttpUtility.ParseQueryString(uri.Query);

            if (query.AllKeys.Contains("v"))
            {
                return query["v"];
            }

            return uri.Segments.Last();
        }

    }
}