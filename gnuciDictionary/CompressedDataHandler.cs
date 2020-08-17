using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace gnuciDictionary
{
	internal class CompressedDataHandler : IDataHandler
	{
		public CompressedDataHandler()
		{
		}

		public Dictionary<string, List<Word>> Load(string peek)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var rscName = $"{nameof(gnuciDictionary)}.data.gzip.dict_{peek}.dat";
			var rscString = assembly.GetManifestResourceStream(rscName);
			if(rscString == null)
			{
				return null;
			}
			string result = null;
			var uniEncode = new UnicodeEncoding();
			using (var fs = rscString)
			using (var fd = new MemoryStream())
			using (var csStream = new GZipStream(fs, CompressionMode.Decompress))
			{
				byte[] buffer = new byte[1024];
				int nRead;
				while ((nRead = csStream.Read(buffer, 0, buffer.Length)) > 0)
				{
					fd.Write(buffer, 0, nRead);
				}
				result = uniEncode.GetString(fd.GetBuffer());
			}
			
			return JsonConvert.DeserializeObject<Dictionary<string, List<Word>>>(result);
		}

		/*public void Save(Dictionary<string, Dictionary<string, List<Word>>> data)
		{
			if (Directory.Exists(DataPath))
			{
				Directory.Delete(DataPath, true);
			}
			Directory.CreateDirectory(DataPath);
			foreach (var kvp in data)
			{
				var path = Path.Combine(DataPath, $"dict_{kvp.Key}.dat");
				var str = JsonConvert.SerializeObject(kvp.Value);
				try
				{
					UnicodeEncoding uniEncode = new UnicodeEncoding();
					byte[] bytesToCompress = uniEncode.GetBytes(str);
					var xstr = uniEncode.GetString(bytesToCompress);
					if(str != xstr)
					{
						throw new Exception();
					}
					using (FileStream fileToCompress = File.Create(path))
					{
						using (GZipStream compressionStream = new GZipStream(fileToCompress, CompressionMode.Compress))
						{
							compressionStream.Write(bytesToCompress, 0, bytesToCompress.Length);
						}
					}
				}
				catch (Exception e)
				{
					Logger.Exception(e, $"Failed to serialize to {path}");
				}
			}
		}*/

	}
}
