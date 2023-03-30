using Humanizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AppCore.Utilities
{
    public static class StringUtilities
    {
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>

        public static string Truncate(this string text, int length)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            else if (text.Length > length)
            {
                int spaceIndex = text.IndexOf(' ', length);
                if (spaceIndex > -1)
                    return text.Substring(0, spaceIndex) + "...";
                else
                    return text;
            }
            else
                return text;
        }
        public static string DasherizeString(string value)
        {
            return value.Dehumanize().Underscore().Dasherize();
        }

        public static Dictionary<string, string> TextToDectionary(this string text)
        {
            string[] items = text.Replace("\r\n", "\n").Replace("<br/>", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string item in items)
            {
                if (item.Contains("|") || item.Contains(";"))
                    dictionary.Add(
                        item.Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries)[0],
                        item.Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                else
                    dictionary.Add(item, string.Empty);
            }
            return dictionary;
        }

        public static string TextToListItems(this string text)
        {
            List<string> items = TextToListItemsArray(text);
            StringBuilder sb = new StringBuilder(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                sb.Append("<li>").Append(items[i]).Append("</li>");
            }
            return sb.ToString();
        }

        public static string TruncateListItems(this string text, int count, int maximumCharacters)
        {
            // clean up a bit
            text = text.Replace("<p>", string.Empty).Replace("</p>", string.Empty).Replace("<br /><br />", "<br />"); ;
            string[] items;
            if (text.IndexOf("<li>") == -1)
                items = text.Replace("<br />", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            else
            {
                text = text.Replace("<ul>", string.Empty).Replace("</ul>", string.Empty);
                items = text.Replace("<li>", "|").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            }

            StringBuilder sb = new StringBuilder(items.Length);
            int totalChars = 0;
            for (int i = 0; i < count && i < items.Length; i++)
            {
                totalChars += items[i].Length;
                if (maximumCharacters > 0 && totalChars > maximumCharacters)
                {
                    break;
                }
                if (items[i].Trim().Length > 0)
                    sb.Append("<li>").Append(items[i]).Append("</li>");
            }
            return sb.ToString();
        }

        public static string StripInvalidCharacters(this string statement)
        {
            return statement.Trim().Replace("\'", "&#039").Replace("\"", "&#034");
        }

        public static List<string> TextToListItemsArray(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<string>();

            }
            return text.Replace("<br/>", "\n").Replace("<br />", "\n").Replace("\r\n", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> TextToListItemsArray2(this string text)
        {
            string[] items = text.Replace("<br/>", "\n").Replace("\r\n", "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(items[i]);
            }
            return list;
        }

        public static List<string> TextToListItemsArray3(this string text)
        {
            string[] items = text.Replace("<br/>", "\n").Replace("\r\n", "\n").Split("##", StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(items[i]);
            }
            return list;
        }
        public static List<string> TextToListItemsWithSpecialCharacter(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<string>();

            }
            return text.Replace("<br/>", "\n").Replace("<br />", "\n").Replace("\r\n", "\n").Split("##", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GetValidUrl(this string s)
        {
            Regex regex = new Regex(@"\\s+");
            string s2 = regex.Replace(s.Trim().Replace("'s", "s"), "-").ToLower();
            regex = new Regex(@"[^(a-z0-9)]+");
            return regex.Replace(s2, "-");
        }

        public static string ValidateWebsite(this string website)
        {
            if (!website.ToString().Contains("http://"))
            {
                return "http://" + website.ToString();
            }
            else return website.ToString();
        }

        public static string GetValidFileName(this string fileName)
        {
            foreach (char badChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(badChar, '_');
            }
            return fileName;
        }

        public static string ToJson<T>(this T obejctToSerialize)
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(obejctToSerialize);
        }

        public static object FromJson(this string jsonString)
        {

            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
        }

        public static string CreateMd5Hash(this string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GetFirstNonEmpty(this string[] segments)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                if (string.IsNullOrEmpty(segments[i]) == false || string.IsNullOrWhiteSpace(segments[i]) == false)
                {
                    return segments[i];
                }
            }
            return string.Empty;
        }

        public static List<Tuple<string, string>> StringsToTuples(this string text)
        {
            List<Tuple<string, string>> tupples = new List<Tuple<string, string>>();
            var lines = text.Split(new string[] { "\n", "\r", "<br>", "<br/>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.Contains("|"))
                    tupples.Add(new Tuple<string, string>(line.Split('|')[0], line.Split('|')[1]));
                else
                    tupples.Add(new Tuple<string, string>("", line));
            }
            return tupples;
        }

        public static string CreatePassword(this int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i <= length; i++)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string ToCurrency(this decimal value)
        {
            if(value!=0)
            return value.ToString("###,###,###.##"); //


            return "0";
        }

        public static List<(string Title, string Description)> GetKeyValueItems(this string text)
        {
            List<(string Title, string Description)> result = new List<(string Title, string Description)>();
            if (!string.IsNullOrEmpty(text))
            {
                var list = TextToListItemsWithSpecialCharacter(text).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var segments = item.Split( "|" , StringSplitOptions.RemoveEmptyEntries);
                        if (segments.Length > 1)
                        {
                            result.Add((segments[0], segments[1]));
                        }
                    }
                }
            }
            return result;
        }

        public static List<(string Title, string Description)> GetKeyValueItems2(this string text)
        {
            List<(string Title, string Description)> result = new List<(string Title, string Description)>();
            if (!string.IsNullOrEmpty(text))
            {
                var list = TextToListItemsArray3(text);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var segments = item.Split("|", StringSplitOptions.RemoveEmptyEntries);
                        if (segments.Length > 1)
                        {
                            result.Add((segments[0], segments[1]));
                        }
                    }
                }
            }
            return result;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}