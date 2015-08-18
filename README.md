# cmdLineTokenizer

Replacement for string[] args which uses **CommandLineToArgvW** and its funny rules.

##What is wrong with string[] args?
The string[] args passed in to an application is based on **CommandLineToArgvW**, and this has some odd usage rules as noted on [MSDN][].
These are in fact not quite correct, and a small distinction\* can be written as follows:

*	2n backslashes followed by a quotation mark produce n backslashes followed by an *unescaped* quotation mark. 
*	(2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by an *escaped* quotation mark. 
*	An unescaped quotation mark followed by a space terminates the current parameter. Unescaped quotation marks never appear in your argument list. E.g. the parameter "a"b" yields: ab 
*	And finally (just to confuse things further) you can also escape quotes by doubling them up. 

\*Sourced from this [blog post][]

This all provides some rather bizzare usage when trying to enter certain command lines, which is explained in great detail on [Windows Insider][], although the most common annoying one is when specifying a trailing backslash on a quoted file path.
e.g.
```
command.exe opt1 "c:\temp\" opt3 opt4
``` 
Will result in 2 arguments of `opt1` and `c:\temp" opt3 opt4`.
The more natural expectation is 4 arguments of `opt1`, `c:\temp\`, `opt3` and `opt4`, but to get this you would need to enter
```
command.exe opt1 "c:\temp\\" opt3 opt4
```

You can also do more odd things too.  
```
command.exe this" is all a single "param
```
But why?  It is not natural to read!.


##How can this be fixed?
Why can't it work as you'd expect with outer quotes meaning that everything inside them is part of a single string and not have to worry about escaping.  Well you can, but every method will end up with a limit, but how about making it a natural writing style limit.
The most natural style to the English language is to use SPACE as a seperator between parameters, except when contained with a set of " " double quotes.

If we look through .NET, we see that **Environment.CommandLine** is exactly what was entered on the command line, so this makes this a valuable source for writing our own tokenizer.

###What are our natural rules?

1. If you are outside quotations marks, SPACE is the separator between arguments.
2. If you are outside quotations marks, a SPACE followed by " will take you inside quotation marks.
3. If you are inside quotation marks, then a second quotation mark followed by a SPACE (or the end of the command line) will signify the end of the quoted block.
4. Enclosing quotation marks (i.e. at both ends) are not returned as part of an argument.

Seems simple enough..

Examples:
```
command.exe -o opt1 -p opt2 -q opt3
```
Will return an array with 6 arguments as expected.

```
command.exe -o "c:\temp\" -p opt2
```
Will return the expected array with 4 arguments of `-o`, `c:\temp\`, `-p` and `opt2`

```
command.exe "a"B"
```
Will return an array with 1 argument of `a"B`

```
command.exe "a"something"B"
```
Will return an array with 1 argument of `a"something"B`.

```
command.exe "a""""""""""""""B"
```
Will return an array with 1 argument of `a""""""""""""""B`.

You can also do
```
command.exe ""surrounded by""
```
Which will return an array with 1 argument of `"surrounded by"`.  Note the outer double quotes are removed, but the inner quotes are expected to be part of the argument.


However
```
command.exe "" surrounded by ""
```
Will return an array with 2 arguments of `surrounded` and `by`.  The double quotes are followed by spaces, so these end up being zero length arguments (which are dropped).

```
command.exe """"" surrounded by """""
```
Will return an array with 4 arguments of `"""`, `surrounded`, `by`, and `"""`.

```
command.exe ""
```
Will return 0 arguments.

```
command.exe "
```
Will return 0 arguments.

```
command.exe " "
```
Will return 1 arguments of a SPACE (okay this might be weird, but it is what you entered!)

An unclosed quoted argument will return a long argument.
```
command.exe a b "c d e f
```
Returns `a`, `b` and `"c d e f`



[blog post]: http://weblogs.asp.net/jongalloway//_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness
[Windows Insider]: http://www.windowsinspired.com/how-a-windows-programs-splits-its-command-line-into-individual-arguments/
[MSDN]:https://msdn.microsoft.com/en-us/library/windows/desktop/bb776391(v=vs.85).aspx