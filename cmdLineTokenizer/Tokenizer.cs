using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmdLineTokenizer
{
    public static class Tokenizer
    {
        /// <summary>
        /// Pulls apart the input string, traditionally the command line, and tokenises it to each item.  
        /// The GOD separator between each argument is SPACE.
        /// If the input contains double quoted (i.e. " ) arguments, these will be returned without quotes, unless the arument contains quotes (whether escaped or unescaped).
        /// </summary>
        /// <param name="commandLine">The command line used to launch the application.  Usually Environment.CommandLine</param>
        /// <returns>List<string></returns>
        public static List<string> TokenizeCommandLineToList(string commandLine)
        {

            List<string> tokens = new List<string>();
            int currentPosition = 0;
            int openTokenPosition = 0;
            int closeTokenPosition = 0;

            // this removes all the escaping that was in place on the command line (and does a fairly decent job of it)!
            var cmdLineCharArray = commandLine.ToCharArray();
            int cmdLineLength = cmdLineCharArray.GetUpperBound(0);

            for (; currentPosition < cmdLineLength;)
            {
                // The beginning of the token is easy because we have figured out the end point of the previous token already
                // We are at the beginning of the command line
                // or we have found a SPACE
                // or the the character is a "  (we dont need to check for a preceding space because that would have triggered entry already)
                if (currentPosition == 0
                    || (cmdLineCharArray[currentPosition] == ' ')
                    || (cmdLineCharArray[currentPosition] == '"')
                    )
                {
                    //depending on what the lead character is depends on how we mark the starting point
                    if (cmdLineCharArray[currentPosition] == '"' || currentPosition == 0)
                    {
                        openTokenPosition = currentPosition;
                    }
                    else
                    {
                        // if this is a SPACE then we can start at the next character (which might actually be a ")
                        openTokenPosition = currentPosition + 1;
                    }

                    //Let go look for the end of this token!
                    do
                    {
                        currentPosition++;

                        // If we are at the end of the array
                        // or we have hit a ", the openToken was a " and the next character is a SPACE, and the token is going to be at least 2 characters (including the quotes!)
                        // or we have hit a SPACE and the openToken is not a "
                        if (currentPosition == cmdLineLength
                            || (cmdLineCharArray[currentPosition] == '"' && cmdLineCharArray[openTokenPosition] == '"' && cmdLineCharArray[currentPosition + 1] == ' ' && currentPosition - openTokenPosition > 1)
                            || (cmdLineCharArray[currentPosition] == ' ' && cmdLineCharArray[openTokenPosition] != '"')
                            )
                        {
                            //The end position of a token depends on whether we want to include the character in the current token, and set the pointer to the next character
                            // or the current character is not part of the token and in fact is the separater that is used to determine the split between tokens.
                            if (currentPosition == cmdLineLength || cmdLineCharArray[currentPosition] == '"')
                            {
                                closeTokenPosition = currentPosition + 1;
                                currentPosition += 1;
                            }
                            else
                            {
                                closeTokenPosition = currentPosition;
                            }
                        }
                    } while (closeTokenPosition == 0);
                }

                //grab our token, and remove any leading/trailing whitespace.
                var token = commandLine.Substring(openTokenPosition, closeTokenPosition - openTokenPosition).Trim();

                // If we have a token enclosed in " " then we should strip them - this happens using the standard function CommandLineToArgvW.
                if (token.StartsWith("\"") && token.EndsWith("\""))
                {
                    // if we have a single quote as an argument, then drop it altogether.... not sure this a correct assumption yet.
                    if (token.Length == 1)
                    {
                        token = string.Empty;
                    }
                    else // we have an argument with quotes at either end so remove the outer quotes.
                    {
                        token = token.Substring(1, token.Length - 2);
                    }
                }

                tokens.Add(token);

                //Reset the closeTokenPosition tracker (only needed because it is used to determine loop
                closeTokenPosition = 0;
            }

            //remove any entries which are empty (caused by having double spaces in between tokens
            tokens.RemoveAll(val => val == string.Empty);

            // remove the first argument.  This is the executable path.
            tokens.RemoveAt(0);

            //return the array in the same format args[] usually turn up to main in.
            return tokens;
        }

        /// <summary>
        /// Pulls apart the input string, traditionally the command line, and tokenises it to each item.  
        /// The GOD separator between each argument is SPACE.
        /// If the input contains double quoted (i.e. " ) arguments, these will be returned without quotes, unless the arument contains quotes (whether escaped or unescaped).
        /// </summary>
        /// <param name="commandLine">The command line used to launch the application.  Usually Environment.CommandLine</param>
        /// <returns>string[]</returns>
        public static string[] TokenizeCommandLineToStringArray(string commandLine)
        {
            List<string> tokens = TokenizeCommandLineToList(commandLine);
            return tokens.ToArray<string>();
        }
    }
}
