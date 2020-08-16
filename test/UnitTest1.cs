using Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
	[TestClass]
	public class UnitTest1
	{
		static string DataPath => "data\\gnucid";
		//[TestMethod]
		public void TestMethod1()
		{
			WordDictionary dict = new WordDictionary(DataPath);
			dict.LoadFromCIDEFiles();
			dict.SaveToBinary("data\\gnucid");
		}

		[TestMethod]
		public void CanDefine()
		{
			WordDictionary dict = new WordDictionary("data\\gnucid");
			Assert.IsNotNull(dict.Define("cat"));
		}
	}
}
