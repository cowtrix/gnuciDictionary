using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dictionary
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
		public string Value { get; }
		[JsonProperty]
		public string Definition { get; }
		[JsonProperty]
		public string Plural { get; }

		public override string ToString() => $"{Value}: {Definition}";
	}
}
