using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace gnucide
{
	internal class JsonDataHandler : IDataHandler
	{
		public string DataPath { get; }

		public JsonDataHandler(string dataPath)
		{
			DataPath = Path.GetFullPath(dataPath);
		}

		public Dictionary<string, List<Word>> Load(string peek)
		{
			var dataFilePath = Path.Combine(DataPath, $"dict_{peek}.json");
			if (!File.Exists(dataFilePath))
			{
				return null;
			}
			return JsonConvert.DeserializeObject<Dictionary<string, List<Word>>>(File.ReadAllText(dataFilePath));
		}

		public void Save(Dictionary<string, Dictionary<string, List<Word>>> data)
		{
			if (Directory.Exists(DataPath))
			{
				Directory.Delete(DataPath, true);
			}
			Directory.CreateDirectory(DataPath);
			foreach (var kvp in data)
			{
				var path = Path.Combine(DataPath, $"dict_{kvp.Key}.json");
				var str = JsonConvert.SerializeObject(kvp.Value, new JsonSerializerSettings() { Formatting = Formatting.Indented });
				File.WriteAllText(path, str);
			}
		}
	}
}
