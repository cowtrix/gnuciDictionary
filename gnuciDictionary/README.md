# gnuciDictionary

A nuget port of the [GNU Collaborative International Dictionary of English](https://gcide.gnu.org.ua/)

To define a word:

```
var definitions = gnuciDictionary.Dictionary.Define("cat");
```

This will return a list of word definitions for "cat".