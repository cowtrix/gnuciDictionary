using Common;
using Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace gnuciDictionary
{
	public static class EnglishDictionary
	{
		private const int PEEK_LENGTH = 2;
		private static IDataHandler m_dataHandler;

		static EnglishDictionary()
		{
			m_dataHandler = new CompressedDataHandler(); ;
		}

		public static string GetPeekValue(string v)
		{
			string format(string str) =>
				str.PadRight(PEEK_LENGTH, '-')
					.Replace("\\", "")
					.Replace("/", "")
					.ToLowerInvariant();
			if (!char.IsLetter(v[0]))
			{
				return format("".PadRight(PEEK_LENGTH, '#'));
			}
			if ((v.Length < PEEK_LENGTH) ||
				v.Take(PEEK_LENGTH).Any(c => !char.IsLetter(c)))
			{
				return format($"{v[0]}");
			}
			return format(v.Substring(0, PEEK_LENGTH));
		}

		public static IEnumerable<Word> GetAllWords()
		{
			foreach (var peek in m_dataHandler.GetPeekValues())
			{
				var data = m_dataHandler.Load(peek);
				foreach (var val in data)
				{
					foreach (var word in val.Value)
					{
						yield return word;
					}
				}
			}
		}

		public static IEnumerable<Word> Define(string word)
		{
			word = word.Trim().ToLowerInvariant();
			var peek = GetPeekValue(word);
			var data = m_dataHandler.Load(peek);
			if (data == null || !data.TryGetValue(word, out var list))
			{
				return null;
			}
			return list.AsReadOnly();
		}
	}
}
