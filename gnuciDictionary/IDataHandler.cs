using System.Collections.Generic;

namespace gnucide
{
	internal interface IDataHandler
	{
		Dictionary<string, List<Word>> Load(string peek);
	}
}
