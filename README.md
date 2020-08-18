# gnuciDictionary

A nuget port of the [GNU Collaborative International Dictionary of English](https://gcide.gnu.org.ua/)

Defining word is simple:

```
IEnumerable<gnuciDictionary.Word> definitions = gnuciDictionary.Dictionary.Define("cat");
Console.WriteLine(definitions.First());
```

This will output:

```
cat: Any animal belonging to the natural family Felidae, and in particular to the various species of the genera Felis, Panthera, and Lynx.
```

### [Nuget Package](https://www.nuget.org/packages/gnuciDictionary)