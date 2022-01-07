using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace gnuciDictionary
{
	internal class CompressedDataHandler : IDataHandler
	{
		public CompressedDataHandler()
		{
		}

		public IEnumerable<string> GetPeekValues()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resources = assembly.GetManifestResourceNames()
				.Where(r => r.EndsWith(".dat"));
			foreach(var r in resources)
			{
				yield return r.Substring(r.Length - 6, 2);
			}
		}

		public Dictionary<string, List<Word>> Load(string peek)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var rscName = $"{nameof(gnuciDictionary)}.data.gzip.dict_{peek}.dat";
			var rscString = assembly.GetManifestResourceStream(rscName);
			if (rscString == null)
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
	}
}
