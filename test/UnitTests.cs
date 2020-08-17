using gnuciDictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	}
}
