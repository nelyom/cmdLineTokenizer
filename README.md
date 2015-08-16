# cmdLineTokenizer
Replacement for string[] args which uses CommandLineToArgvW and its funny rules.


The string[] args passed in to an application is based on *CommandLineToArgvW*, and this has funny rules as noted on MSDN\*

*	2n backslashes followed by a quotation mark produce n backslashes followed by a quotation mark. 
*	(2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by a quotation mark. 
*	n backslashes not followed by a quotation mark simply produce n backslashes.

These are in fact not quite correct, and a small distinction\* can be written as follows:

*	2n backslashes followed by a quotation mark produce n backslashes followed by an *unescaped* quotation mark. 
*	(2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by an *escaped* quotation mark. 
*	An unescaped quotation mark followed by a space terminates the current parameter. Unescaped quotation marks never appear in your argument list. E.g. the parameter "a"b" yields: ab 
*	And finally (just to confuse things further) you can also escape quotes by doubling them up. 


All this is rather weird, and appears to be how it handles quoting.
If however it used SPACE as the god character, i.e. the ultimate separator between parameters (unless contained within a set of " "), things get much easier to deal with.  This little tokenizer is my basic attempt to deal with all that mess.


\*Sourced from http://weblogs.asp.net/jongalloway//_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness
