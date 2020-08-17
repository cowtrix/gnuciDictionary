using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace gnuciDictionary
{
	public class Word
	{
		[Obsolete("Serializer only", true)]
		public Word() { }

		public Word(string value, string def, string plural)
		{
			Value = value;
			Definition = def;
			Plural = plural;
		}

		[JsonProperty]
		public string Value { get; set; }
		[JsonProperty]
		public string Definition { get; set; }
		[JsonProperty]
		public string Plural { get; set; }

		public override string ToString() => $"{Value}: {Definition}";
	}
}
