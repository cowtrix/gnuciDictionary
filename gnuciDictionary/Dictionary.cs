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

namespace Dictionary
{
	public class WordDictionary
	{
		private const int PEEK = 3;
		private Dictionary<string, Dictionary<string, List<Word>>> m_data = 
			new Dictionary<string, Dictionary<string, List<Word>>>();
		public string DataPath { get; }

		public WordDictionary(string path)
		{
			DataPath = path;
		}

		private string GetPeekValue(string v)
		{
			string format(string str) =>
				str.PadRight(PEEK, '-')
					.Replace("\\", "")
					.Replace("/", "")
					.ToLowerInvariant();
			if (!char.IsLetter(v[0]))
			{
				return format("".PadRight(PEEK, '#'));
			}
			if ((v.Length < PEEK) ||
				v.Take(PEEK).Any(c => !char.IsLetter(c)))
			{
				return format($"{v[0]}");
			}
			return format(v.Substring(0, PEEK));
		}

		public void LoadFromCIDEFiles()
		{
			void AddWord(string v)
			{
				var lines = v.Split('\n');
				var val = Regex.Match(lines[0], "<p><ent>(.*)</ent>").Groups[1].Value;
				if(string.IsNullOrEmpty(val))
				{
					return;
				}
				var def = Regex.Match(v, "<def>(.*)</def>").Groups[1]?.Value;
				def = StringExtensions.ReplaceAll(def, "<.*>", "");
				var plural = Regex.Match(v, "<plw>(.*)</plw>").Groups[1]?.Value;
				var peek = GetPeekValue(val);
				if (!m_data.TryGetValue(peek, out var bucket))
				{
					bucket = new Dictionary<string, List<Word>>();
					m_data.Add(peek, bucket);
				}
				if(!bucket.TryGetValue(val, out var list))
				{
					list = new List<Word>();
					bucket[val] = list;
				}

				var word = new Word(val, def, plural);
				Logger.Debug($"{peek}: {word}");
				list.Add(word);
			}

			var path = @"C:\Users\gbfinnes\Downloads\gcide-0.52\gcide-0.52\";
			foreach(var file in Directory.GetFiles(path, "CIDE.*"))
			{
				Logger.Debug($"Loading {file}");
				var lines = File.ReadAllLines(file);
				StringBuilder sb = null;
				for (int i = 0; i < lines.Length; i++)
				{
					var line = lines[i];
					if(line.StartsWith("<p><ent>"))
					{
						sb = new StringBuilder(line + "\n");
					}
					if(sb != null)
					{
						if(string.IsNullOrEmpty(line.Trim()))
						{
							AddWord(sb.ToString());
							sb = null;
						}
						else
						{
							sb.AppendLine(line);
						}						
					}
				}
			}
		}

		public IEnumerable<Word> Define(string word)
		{
			word = word.Trim();
			var peek = GetPeekValue(word);
			var data = LoadFromBinary(peek);
			if(data == null || !data.TryGetValue(word, out var list))
			{
				return null;
			}
			return list.AsReadOnly();
		}

		private Dictionary<string, List<Word>> LoadFromBinary(string peek)
		{
			var dataFilePath = Path.Combine(DataPath, $"dict_{peek}.dat");
			if (!File.Exists(dataFilePath))
			{
				return null;
			}
			var sb = new StringBuilder();
			var uniEncode = new UnicodeEncoding();
			using (Stream fs = File.OpenRead(dataFilePath))
			using (Stream csStream = new GZipStream(fs, CompressionMode.Decompress))
			{
				byte[] buffer = new byte[1024];
				int nRead;
				while ((nRead = csStream.Read(buffer, 0, buffer.Length)) > 0)
				{
					//fd.Write(buffer, 0, nRead);
					sb.Append(uniEncode.GetString(buffer));
				}
			}
			return JsonConvert.DeserializeObject<Dictionary<string, List<Word>>>(sb.ToString());
		}

		public void SaveToBinary(string directory)
		{
			directory = Path.GetFullPath(directory);
			if(Directory.Exists(directory))
			{
				Directory.Delete(directory, true);
			}
			Directory.CreateDirectory(directory);
			foreach(var kvp in m_data)
			{
				var path = Path.Combine(directory, $"dict_{kvp.Key}.dat");
				var str = JsonConvert.SerializeObject(kvp.Value, new JsonSerializerSettings()
				{
				});
				try
				{
					
					UnicodeEncoding uniEncode = new UnicodeEncoding();
					byte[] bytesToCompress = uniEncode.GetBytes(str);
					using (FileStream fileToCompress = File.Create(path))
					{
						using (GZipStream compressionStream = new GZipStream(fileToCompress, CompressionMode.Compress))
						{
							compressionStream.Write(bytesToCompress, 0, bytesToCompress.Length);
						}
					}
				}
				catch(Exception e)
				{
					Logger.Exception(e, $"Failed to serialize to {path}");
				}
			}
		}
	}
}
