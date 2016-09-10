using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            var parts = commandLine.Split(' ');
            StringBuilder token = new StringBuilder(255);

            for (int curPart = 0; curPart < parts.Length; curPart++)
            {

                if (parts[curPart].StartsWith("\""))
                {
                    //remove leading "
                    token.Append(parts[curPart].Substring(1));
                    int has1NQuote = 0;

                    //find out whether the current token has the required 1 leftover "
                    for (int i = parts[curPart].Length - 1; i > 0; i--)
                    {
                        if (parts[curPart][i] == '"')
                        {
                            has1NQuote += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // if we didn't have a leftover ", then we need to add some more parts to the current token
                    while (has1NQuote % 2 == 0 && (curPart != parts.Length - 1))
                    {
                        has1NQuote = 0;
                        curPart++;

                        for (int i = parts[curPart].Length - 1; i >= 0; i--)
                        {
                            if (parts[curPart][i] == '"')
                            {
                                has1NQuote += 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        token.Append(' ').Append(parts[curPart]);
                    }

                    //remove trailing " if we had a leftover (if we didn't have a leftover then we hit the end of the parts)
                    if (has1NQuote%2 != 0)
                    {
                        token.Remove(token.Length - 1, 1);
                    }
                }
                else
                {
                    token.Append(parts[curPart]);
                }

                //strip whitespace.
                if (!string.IsNullOrEmpty(token.ToString().Trim()))
                tokens.Add(token.ToString().Trim());
                
                token.Clear();
            }


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
