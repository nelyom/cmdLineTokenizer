using System;
using cmdLineTokenizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Tokenizer.Tests
{
    [TestClass]
    public class CommandLineNoArguments
    {

        [TestMethod]
        public void ExecutableInShortNameFolderReturnsEmptyStringArray()
        {
            string commandLine = "\"C:\\Tmp\\test.exe\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void ExecutableWithNoFolderReturnsEmptyStringArray()
        {
            string commandLine = "test.exe";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void ExecutableInFolderReturnsEmptyStringArray()
        {
            string commandLine = "\"C:\\Development\\cmdlineTokenizer.exe\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void ExecutableInFolderWithSpaceWithReturnsEmptyStringArray()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
        }
    }

    [TestClass]
    public class CommandLineOneArgument
    {

        [TestMethod]
        public void OneArgumentsReturnsArgument()
        {
            string commandLine = "\"C:\\Tmp\\cmdlineTokenizer.exe\" arg";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("arg", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void ExecutableInFolderWithSpaceWithOneArgumentReturnsArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" arg";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("arg", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void ArgumentWithTrailingBackSlashReturnsArgumentWithTrailingBackslash()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" C:\\tmp\\";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("C:\\tmp\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithSpacesAndTrailingBackSlashReturnsSingleArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"C:\\some file path\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("C:\\some file path\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithNoSpacesReturnsSingleArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithSingleCharArgumentReturnsSingleArgument()
        {
            string commandLine = "cmdlineTokenizer.exe \"A\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("A", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }
    }


    [TestClass]
    public class CommandLineMultipleArguments
    {
        [TestMethod]
        public void UnQuotedArgumentWithMultipleBackSlashReturnsCorrectly()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \\\\test\\\\ \\\\test\\\\";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\test\\\\", args[0].ToString());
            Assert.AreEqual("\\\\test\\\\", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithMultipleBackSlashReturnsExpected()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"\\\\this is a test\\\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\this is a test\\\\", args[0].ToString());
            Assert.AreEqual(1, args.Length, 1);
        }

        [TestMethod]
        public void QuotedMultipleArgumentWithMultipleBackSlashReturnsExpected()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"\\\\test\\\\\" \"\\\\test\\\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\test\\\\", args[0].ToString());
            Assert.AreEqual("\\\\test\\\\", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

                [TestMethod]
        public void UnquotedArgumentWithSpacesAndTrailingBackSlashReturnsMultipleArguments()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" C:\\some file path\\";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("C:\\some", args[0].ToString());
            Assert.AreEqual("file", args[1].ToString());
            Assert.AreEqual("path\\", args[2].ToString());
            Assert.AreEqual(3, args.Length);
        }

        [TestMethod]
        public void OneQuotedAndOneUnQuotedArgumentReturnCorrectly()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\" param";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param", args[0].ToString());
            Assert.AreEqual("param", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }
        
        //This is a reverse of the one above
        [TestMethod]
        public void OneUnQuotedAndOneQuotedArgumentReturnCorrectly()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" param \"param\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param", args[0].ToString());
            Assert.AreEqual("param", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void TwoArgumentsBothQuoted()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\" \"param\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param", args[0].ToString());
            Assert.AreEqual("param", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void TwoSpacesBetweenTwoUnquotedArgumentsAreIgnored()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" arg  hhes";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("arg", args[0].ToString());
            Assert.AreEqual("hhes", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void ThreeSpacesBetweenTwoUnquotedArgumentsAreIgnored()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" arg   hhes";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("arg", args[0].ToString());
            Assert.AreEqual("hhes", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void FourSpacesBetweenTwoUnquotedArgumentsAreIgnored()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" arg    hhes";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("arg", args[0].ToString());
            Assert.AreEqual("hhes", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void MultipleSpacesBeforeAndAfterMultipleArgumentsAreIgnored()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" --dry    -c -D  \"C:\\some file path\\\"      hhes     ";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("--dry", args[0].ToString());
            Assert.AreEqual("-c", args[1].ToString());
            Assert.AreEqual("-D", args[2].ToString());
            Assert.AreEqual("C:\\some file path\\", args[3].ToString());
            Assert.AreEqual("hhes", args[4].ToString());
            Assert.AreEqual(5, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentsWithForwardSlashReturnsExpected()
        {
            string commandLine = "myapp.exe --debug --file -- \"/Users/fclp/test.txt\" \"/Users/fclp/test2.txt\" \"/Users/fclp/test3.txt\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("--debug", args[0].ToString());
            Assert.AreEqual("--file", args[1].ToString());
            Assert.AreEqual("--", args[2].ToString());
            Assert.AreEqual("/Users/fclp/test.txt", args[3].ToString());
            Assert.AreEqual("/Users/fclp/test2.txt", args[4].ToString());
            Assert.AreEqual("/Users/fclp/test3.txt", args[5].ToString());
            Assert.AreEqual(6, args.Length);

        }

        [TestMethod]
        public void MultipleArgumentsWithQuotedArgumentInMiddleReturnsExpected()
        {
            string commandLine = "foo.exe --dir \"C:\\temp\\bar\\\" --sk [veris";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("--dir", args[0].ToString());
            Assert.AreEqual("C:\\temp\\bar\\", args[1].ToString());
            Assert.AreEqual("--sk", args[2].ToString());
            Assert.AreEqual("[veris", args[3].ToString());
            Assert.AreEqual(4, args.Length);
        }
    }

    [TestClass]
    //These particular tests are oddball cases, which I am not sure should pass as they stand.  The behaviour could go either way, and perhaps these ones should
    // really return multiple arguments, no argumnents, or just error (although this seems like a bad plan).
    // As it stands they do not, as SPACE is being used as the GOD separator character, and even though a Quote may be used
    // to denote the beginning and the end of a parameter, it may be desirable to include them as a potential input value under the right circumstances.
    public class QuoteArgumentCases
    {

        [TestMethod]
        public void ArgumentIsSingleQuoteReturnsNoArgument()
        {
            string commandLine = "foo.exe \"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void ArgumentIsPairQuoteReturnsNoArgument()
        {
            string commandLine = "foo.exe \"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
            //Assert.AreEqual("\"", args[0]);

        }

        [TestMethod]
        public void ArgumentIsPairQuoteWrappingSPACEReturnsNoArgument()
        {
            string commandLine = "foo.exe \" \"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual(0, args.Length);
            //Assert.AreEqual(1, args.Length);
            //Assert.AreEqual(" ", args[0]);
        }

        [TestMethod]
        public void ArgumentIsSingleQuoteFollowedBySpaceReturnsNoArgument()
        {
            string commandLine = "foo.exe \" ";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            //Assert.AreEqual("\"", args[0].ToString());
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void ExecutableWithSpaceArgumentIsSingleQuoteFollowedBySpaceReturnsNoArgument()
        {
            string commandLine = "\"C:\\f a\\foo.exe\" \" ";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            //Assert.AreEqual("\"", args[0].ToString());
            Assert.AreEqual(0, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithEscapedQuotesReturnsEscapedQuotedArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"\\\"paramquote\\\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\"paramquote\\\"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithUnescapedQuotesReturnsUnescapedQuotedArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"\"paramquote\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\"paramquote\"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithEscapedQuoteInsideReturnsSingleArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\\\"quote\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param\\\"quote", args[0].ToString());
            Assert.AreEqual(1, args.Length, 1);
        }

        [TestMethod]
        public void QuotedArgumentWithUnescapedQuoteInsideReturnsSingleArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\"quote\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param\"quote", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithEscapedQuotePairInsideReturnsSingleArgument()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\\\"\\\"quote\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param\\\"\\\"quote", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithQuotePairInsideReturnsArgumentWithSingleInternalQuote()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"param\"\"quote\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("param\"quote", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void DoubleQuotedArgumentWithSpacesEitherSideOfContentsReturnsIndividualArguments()
        {
            string commandLine = "command.exe \"\" surrounded by \"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("surrounded", args[0].ToString());
            Assert.AreEqual("by", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void MultiQuotedArgumentWith3QuotesSpacesEitherSideOfContentsReturnsSplitArgument()
        {
            string commandLine = "command.exe \"\"\" surrounded by \"\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\" surrounded by \"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void MultiQuotedArgumentWith4QuotesSpacesEitherSideOfContentsReturnsSplitArgument()
        {
            string commandLine = "command.exe \"\"\"\" surrounded by \"\"\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\"", args[0].ToString());
            Assert.AreEqual("surrounded", args[1].ToString());
            Assert.AreEqual("by", args[2].ToString());
            Assert.AreEqual("\"", args[3].ToString());
            Assert.AreEqual(4, args.Length);
        }

        [TestMethod]
        public void MultiQuotedArgumentWithSpacesEitherSideOfContentsReturnsMultiQuotedArgument()
        {
            string commandLine = "command.exe \"\"\"\"\" surrounded by \"\"\"\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\"\" surrounded by \"\"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void SingleQuotedArgumentWithSpacesEitherSideOfContentsReturnsArgumentWithSpaces()
        {
            string commandLine = "command.exe \" surrounded by \"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("surrounded by", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedArgumentWithSpacesEitherSideReturnsQuotedArgument()
        {
            string commandLine = "command.exe \"a\"something\"B\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("a\"something\"B", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void ArgumentWithQuoteInMiddleReturnsArgumentWithQuoteInMiddle()
        {
            string commandLine = "command.exe a\"B";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("a\"B", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }



        [TestMethod]
        public void UnclosedQuotedArgumentReturns()
        {
            string commandLine = "command.exe o p d \"a something B";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("o", args[0].ToString());
            Assert.AreEqual("p", args[1].ToString());
            Assert.AreEqual("d", args[2].ToString());
            Assert.AreEqual("a something B", args[3].ToString());
            Assert.AreEqual(4, args.Length);
        }


        [TestMethod]
        public void TwelveInternalQuotesReturnsArgumentWith12InternalQuotes()
        {
            //CommandLineToArgvW returns 4 internal quotes, even though 12 are supplied
            string commandLine = "command.exe foo\"\"\"\"\"\"\"\"\"\"\"\"bar";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"\"\"\"\"\"\"\"\"\"\"\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void QuotedTwelveInternalQuotesReturnsArgumentWith6InternalQuotes()
        {
            //CommandLineToArgvW returns 4 internal quotes, even though 12 are supplied
            string commandLine = "command.exe \"foo\"\"\"\"\"\"\"\"\"\"\"\"bar\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"\"\"\"\"\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void FiveExternalQuotesReturnExpectedTwoCount()
        {
            //CommandLineToArgvW returns 2 external quotes either side, even though 4 are supplied.
            // in our case we return 3 either side as we only take note of the 2 outermost
            string commandLine = "command.exe \"\"\"\"\"foo\"\"\"\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\"\"foo\"\"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void SixExternalQuotesReturnExpectedThreeCount()
        {
            //CommandLineToArgvW returns 2 external quotes either side, even though 4 are supplied.
            // in our case we return 3 either side as we only take note of the 2 outermost
            string commandLine = "command.exe \"\"\"\"\"\"foo\"\"\"\"\"\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\"\"\"foo\"\"\"", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }


        [TestMethod]
        public void OneInternalQuotesReturnExpectedCount()
        {
            string commandLine = "command.exe foo\"bar";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void TwoInternalQuotesReturnsArgumentWith2InternalQuotes()
        {
            string commandLine = "command.exe foo\"\"bar";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void ThreeInternalQuotesReturnsArgumentWith3InternalQuotes()
        {
            string commandLine = "command.exe foo\"\"\"bar";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"\"\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void InteralQuotesReturnsArgumentWithInternalQuotes()
        {
            string commandLine = "command.exe foo\"x\"\"bar";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("foo\"x\"\"bar", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

    }


    /// <summary>
    /// The CommandLineToArgvW does some strange things which shouldn't happen.  I think it be because it is effectively using some really special handing around " characters
    /// whereas I've chosen SPACE to be the god character, i.e. the defining character to separate arguments, and this allows much better handling of where the start and end 
    /// of a quoted variable actually is
    /// it does mean you can pass "a"B", as a parameter and you should expect to get an argument of a"B passed in.
    ///
    /// these tests are mainly sourced from complaints referenced here:
    /// http://weblogs.asp.net/jongalloway//_5B002E00_NET-Gotcha_5D00_-Commandline-args-ending-in-_5C002200_-are-subject-to-CommandLineToArgvW-whackiness
    /// </summary>
    [TestClass]
    public class CommandLineToArgvWFunnyHandling
    {


        [TestMethod]
        public void ParameterWithLeadingSlashesAndEndSlashesReturnsCorrectNumberOfSlashes()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \\\\this is a test\\\\\\\\\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\this", args[0].ToString());
            Assert.AreEqual("is", args[1].ToString());
            Assert.AreEqual("a", args[2].ToString());
            Assert.AreEqual("test\\\\\\\\\\\"", args[3].ToString());
            Assert.AreEqual(4, args.Length);
        }


        [TestMethod]
        public void ParameterWithMidQuoteReturnsParameterWithMidQuote()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"a\"B\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("a\"B", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void LeadingAndEndingSlashesReturnAsExpected()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\" \"\\\\test\\\\\" \"\\\\test\\\\\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\test\\\\", args[0].ToString());
            Assert.AreEqual("\\\\test\\\\\\", args[1].ToString());
            Assert.AreEqual(2, args.Length);
        }

        [TestMethod]
        public void LeadingAndEndingSlashesReturnAsExpected2()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\"  \\\\test\\\\";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\test\\\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void LeadingAndEndingSlashesReturnAsExpected3()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\"  \"\\\\test\\\\\\\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\\\test\\\\\\\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }

        [TestMethod]
        public void LeadingAndEndingSlashesReturnAsExpected4()
        {
            string commandLine = "\"C:\\Some Folder With A Space\\cmdlineTokenizer.exe\"  \"\\14\\2415\\\"";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            Assert.AreEqual("\\14\\2415\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);
        }
    }



    /// <summary>
    /// This is out in it's own bucket because it calls one some OS methods, so escapes the boundary of a Unit Test
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void QuotedFilePathIsValidFilePath()
        {
            string systemPath = Environment.ExpandEnvironmentVariables("%ProgramFiles%");
            // need to quotes here because we want this to be a single variable 
            // add a trailing slash to the argument, but only because of the way GetDirectoryName works later on
            string commandLine = "\"C:\\f a\\foo.exe\" \"" + systemPath + "\\\" ";
            string[] args = cmdLineTokenizer.Tokenizer.TokenizeCommandLineToStringArray(commandLine);
            
            //We should get what we need here.
            Assert.AreEqual(systemPath + "\\", args[0].ToString());
            Assert.AreEqual(1, args.Length);

            //Check to make sure GetDirectoryName returns what it should.
            Assert.AreEqual(systemPath, System.IO.Path.GetDirectoryName(args[0].ToString()));
        }
    }
}
