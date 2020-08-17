using gnucide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace gnucide
{
	[TestClass]
	public class UnitTests
	{
		/*[TestMethod]
		public void LoadCIDE()
		{
			var cide = CIDEUtility.LoadFromCIDEFiles(@"C:\Users\gbfinnes\Documents\repos\gnuciDictionary\raw");
			new JsonDataHandler(@"C:\Users\gbfinnes\Documents\repos\gnuciDictionary\gnuciDictionary\data\json")
				.Save(cide);
			new CompressedDataHandler(@"C:\Users\gbfinnes\Documents\repos\gnuciDictionary\gnuciDictionary\data\gzip")
				.Save(cide);
		}*/

		[DataTestMethod]
		[DataRow("cat", "Any animal belonging to the natural family Felidae")]
		[DataRow("dog", "A quadruped of the genus Canis")]
		[DataRow("apple", "The fleshy pome or fruit of a rosaceous tree")]
		[DataRow("Xylophagides", "A tribe or family of dipterous flies")]
		public void CanDefine(string wordString, string def)
		{
			WordDictionary dict = new WordDictionary();
			var definitions = dict.Define(wordString);
			Assert.IsNotNull(definitions);
			Assert.IsTrue(definitions.Any(w => w.Definition.StartsWith(def)), 
				$"Incorrect definition: {string.Join("\n", definitions.Select(w => w.Definition))}");
		}
	}
}
