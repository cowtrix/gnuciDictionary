using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Extensions
{

	public static class StringExtensions
	{
		static char[] wordsplits = new char[] { '.', '?', '!', ' ', ';', ':', ',' };
		public static int WordCount(this string str)
		{
			return str.Split(wordsplits, StringSplitOptions.RemoveEmptyEntries).Length;
		}

		public static string LowerAndAlphaNumeric(this string str)
		{
			return Regex.Replace(str, @"[^a-zA-Z\d\s:]", "").ToLowerInvariant();
		}

		public static string FormatString(string str)
		{
			var lines = str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Where(s => s.Any(c => !char.IsWhiteSpace(c)));
			var min = lines.Min(s =>
			{
				int i = 0;
				foreach (var c in s)
				{
					var increase = WhitespaceCharCount(c);
					if (increase == 0)
					{
						break;
					}
					i += increase;
				}
				return i;
			});
			if (min == 0)
			{
				return str;
			}
			var sb = new StringBuilder();
			foreach (var l in lines)
			{
				var whitespaceCounter = 0;
				var i = 0;
				for (; i < min && whitespaceCounter < min; ++i)
				{
					var increase = WhitespaceCharCount(l[i]);
					if (increase == 0)
					{
						break;
					}
					whitespaceCounter += increase;
				}
				sb.AppendLine(l.Substring(i));
			}
			return sb.ToString().Trim();
		}

		static int WhitespaceCharCount(char c)
		{
			if (c == ' ')
			{
				return 1;
			}
			if (c == '\t')
			{
				return 4;
			}
			return 0;
		}

		public static string ReplaceAll(this string str, string regex, string newValue, RegexOptions options = RegexOptions.Multiline)
		{
			if(string.IsNullOrEmpty(str))
			{
				return str;
			}
			Match match = Regex.Match(str, regex, options);
			while (match.Success)
			{
				str = str.Substring(0, match.Index) + newValue + str.Substring(match.Index + match.Length);
				match = Regex.Match(str, regex, options);
			}
			return str;
		}

		public static IEnumerable<string> ExtractBlocks(this string str, string openBlock, string closeBlock)
		{
			if (string.IsNullOrEmpty(str))
			{
				yield break;
			}
			int indentLevel = 0;
			var stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				if (i < str.Length - openBlock.Length && str.Substring(i, openBlock.Length) == openBlock)
				{
					indentLevel++;
					i += openBlock.Length - 1;
					stringBuilder.Append(openBlock);
				}
				else if (i < str.Length - closeBlock.Length && str.Substring(i, closeBlock.Length) == closeBlock)
				{
					indentLevel--;
					i += openBlock.Length - 1;
					stringBuilder.Append(closeBlock);
					if (indentLevel == 0)
					{
						yield return stringBuilder.ToString();
						stringBuilder.Clear();
					}
				}
				else if (indentLevel > 0)
				{
					stringBuilder.Append(str[i]);
				}
			}
			if(indentLevel > 0)
			{
				throw new Exception($"Expected closing tag {closeBlock}");
			}
		}

		public static string AppendIfNotNull(this string str, string toAppend)
		{
			if(str == null)
			{
				return str;
			}
			return str + toAppend;
		}
		public static string ToCamelCase(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return $"{char.ToLowerInvariant(str[0])}{str.Substring(1)}";
		}
		public static byte[] AsciiToByteArray(this string str)
		{
			return System.Text.Encoding.ASCII.GetBytes(str);
		}
		public static string ByteArrayToAsciiString(this byte[] byteArray)
		{
			return Encoding.ASCII.GetString(byteArray);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public static string StripForURL(this string s)
		{
			if(string.IsNullOrEmpty(s))
			{
				return s;
			}
			return System.Net.WebUtility.UrlEncode(Regex.Replace(s.ToLower(), @"[^a-zA-Z_\d\s:]", ""));
		}

		private static Random random = new Random();

		/// <summary>
		/// Generate a random string of the given length.
		/// WARNING: This function is NOT cryptographically secure
		/// If you are using this for any kind of secure purpose,
		/// DON'T - use KeyGenerator.GetUniqueKey instead
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			lock(random)	// Random is not thread sage
			{
				return new string(Enumerable.Repeat(chars, length)
					.Select(s => s[random.Next(s.Length)]).ToArray());
			}
		}
	}
}
