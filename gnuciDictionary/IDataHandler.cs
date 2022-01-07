using System.Collections.Generic;

namespace gnuciDictionary
{
	internal interface IDataHandler
	{
		Dictionary<string, List<Word>> Load(string peek);

		IEnumerable<string> GetPeekValues();
	}
}
