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
                // if we are looking at a SPACE move on.  Tokens start at the next character....
                if (cmdLineCharArray[currentPosition] == ' ')
                {
                    currentPosition++;
                }
                else
                {
                    openTokenPosition = currentPosition;

                    //Let go look for the end of this token!
                    do
                    {
                        currentPosition++;

                        // If we are at the end of the array
                        if (currentPosition == cmdLineLength)
                        {
                            closeTokenPosition = ++currentPosition;   //increment then assign.
                        }
                        else
                        {
                            // if we are at a SPACE, then check out whether we are at the end of a token
                            if (cmdLineCharArray[currentPosition] == ' ')
                            {
                                // if the start of this token was a " and the previous character (we are at a SPACE remember) was a ", 
                                // and this token actually contains something (other than the quotes), then close it off
                                // or if we didn't start with a quote and we are at a space, then close this token off.
                                if ((cmdLineCharArray[openTokenPosition] == '"' && cmdLineCharArray[currentPosition - 1] == '"' && currentPosition - openTokenPosition > 2) ||
                                    (cmdLineCharArray[openTokenPosition] != '"')
                                    )
                                {
                                    closeTokenPosition = currentPosition++;  // saves the current position and then increments afterwards.
                                }

                            }
                        }

                    } while (closeTokenPosition == 0);


                    //grab our token, and remove any leading/trailing whitespace (this only deals with the scenario where we open a quoted token, but dont close it and have trail spaces)
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
