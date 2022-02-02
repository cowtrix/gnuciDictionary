using gnuciDictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace gnuciDictionary
{
	[TestClass]
	public class UnitTests
	{
		[DataTestMethod]
		[DataRow("cat", "Any animal belonging to the natural family Felidae")]
		[DataRow("dog", "A quadruped of the genus Canis")]
		[DataRow("apple", "The fleshy pome or fruit of a rosaceous tree")]
		[DataRow("Xylophagides", "A tribe or family of dipterous flies")]
		public void CanDefine(string wordString, string def)
		{
			var definitions = EnglishDictionary.Define(wordString);
			Assert.IsNotNull(definitions);
			Assert.IsTrue(definitions.Any(w => w.Definition.StartsWith(def)), 
				$"Incorrect definition: {string.Join("\n", definitions.Select(w => w.Definition))}");
		}

		[TestMethod]
		public void CanEnumerateValues()
		{
			var allValues = EnglishDictionary.GetAllWords().ToList();
			Assert.IsTrue(allValues.Count > 0);

			var all5Words = allValues
				.Select(w => w.Value)
				.Distinct()
				.Where(w => w.Length == 5)
				.ToList();

			var charValues = new Dictionary<char, int>();
			foreach(var w in all5Words)
			{
				foreach(char c in w)
				{
					if(!charValues.TryGetValue(c, out var count))
					{
						charValues[c] = 1;
					}
					else
					{
						charValues[c] = count + 1;
					}
				}
			}

			var charScores = new Dictionary<string, int>();
			foreach(var w in all5Words)
			{
				var wordScore = 0;
				var history = new HashSet<char>();
				for (int i = 0; i < w.Length; i++)
				{
					char c = w[i];
					var charScore = charValues[c];
					if(!history.Contains(c))
						wordScore += charScore;
					else
					{
						wordScore -= charScore;
					}
					history.Add(c);
				}
				charScores[w] = wordScore;
			}

			var best = charScores.OrderBy(x => x.Value)
				.ToList();

		}
	}
}
