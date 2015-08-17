# cmdLineTokenizer
Replacement for string[] args which uses **CommandLineToArgvW** and its funny rules.


The string[] args passed in to an application is based on **CommandLineToArgvW**, and this has funny rules as noted on MSDN\*

*	2n backslashes followed by a quotation mark produce n backslashes followed by a quotation mark. 
*	(2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by a quotation mark. 
*	n backslashes not followed by a quotation mark simply produce n backslashes.

These are in fact not quite correct, and a small distinction\* can be written as follows:

*	2n backslashes followed by a quotation mark produce n backslashes followed by an *unescaped* quotation mark. 
*	(2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by an *escaped* quotation mark. 
*	An unescaped quotation mark followed by a space terminates the current parameter. Unescaped quotation marks never appear in your argument list. E.g. the parameter "a"b" yields: ab 
*	And finally (just to confuse things further) you can also escape quotes by doubling them up. 
  
  
All this is rather weird, and appears to be how **CommandLineToArgvW** handles quoting.
If, however, it used SPACE as the ultimate separator between parameters (unless contained within a set of " " double quotes), things are much easier to deal with.  

This tokenizer, uses SPACE as the separator character between options, and a little bit of look behind to determine the start and end of quoted parameters.  The evaluation of parameters is left to right on the CommandLine.
You simply provide it the Environment.CommandLine.

Examples:
```
command.exe -o opt1 -p opt2 -q opt3
```
Will return an array with 6 arguments as expected.

```
command.exe -o "c:\temp\" -p opt2
```
Will return the expected array with 4 arguments of `-o`, `c:\temp\`, `-p` and `opt2`
CommandLineToArgvW will return 2 arguments of `-o` and `c:\temp" -p opt2`, which is wrong!

```
command.exe "a"B"
```
Will return an array with 1 argument of `a"B`, rather than the `aB` argument you get with CommandLineToArgvW

```
command.exe "a"something"B"
```
Will return an array with 1 argument of `a"something"B`.


You can also do
```
command.exe ""surrounded by""
```
Which will return an array with 1 argument of `"surrounded by"`.  Note the outer double quotes are removed, but the inner quotes are expected to be part of the argument.

```
command.exe "" surrounded by ""
```
Will return an array with 1 argument of `" surrounded by "`.  The outer spaces are enclosed by double ", so they are all part of the same argument.

However

```
command.exe ""
```
Will return 0 arguments.
```
command.exe "
```
Will return 0 arguments.

But
```
command.exe " "
```
Will return 1 arguments of a SPACE

And an unclosed quoted argument will return a long argument, including the quote.
```
command.exe a b "c d e f
```
Returns `a`, `b` and `"c d e f`


\*Sourced from http://weblogs.asp.net/jongalloway//_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness
