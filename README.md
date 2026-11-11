# Word Window

Export cited concordance rows from a folder of text files while preserving punctuation.

## Try it

From this folder with the .NET 10 SDK installed:

```sh
dotnet run -- corpus texts bank 3
```

Arguments are folder of UTF-8 .txt files, one keyword and the context size in words per side. The report is CSV with relative document path, one-based line and column. Redirect stdout to save it.

The original exercise remains available with `dotnet run -- example.tsv`.

## When to use it

A linguistics student wants examples of financial and river senses of “bank” with citations back to the corpus.

Example result:

```text
document,line,column,left,keyword,right
"money.txt",1,5,"The ","bank"," raised rates."
"river.txt",1,11,"The river ","bank",", swollen by rain, "
```

Related course or field workflow: [AntConc corpus concordancer](https://www.laurenceanthony.net/software/antconc/).
