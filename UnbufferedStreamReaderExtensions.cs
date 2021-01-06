using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ExtJsNamespaceDeployer
{
    public static class UnbufferedStreamReaderExtensions
    {
        public static void Trim(this UnbufferedStreamReader reader)
        {
            while (reader.Peek() == ' ')
            {
                reader.Read();
            }
        }

        public static string ReadToClosingBracket(this UnbufferedStreamReader reader, params char[] excludeBrackets)
        {
            ValidateStreamNotReachedEnd(reader);

            var exclude = FormatExcludeBracketsArgs(excludeBrackets);

            var sb = new StringBuilder();

            if (!CharTools.IsOpeningBracket((char)reader.Peek()))
            {
                throw new InvalidOperationException("No opening bracket at reading position.");
            }

            var bracketStack = new Stack<char>();
            bracketStack.Push((char)reader.Read());

            while (bracketStack.Count != 0 && !reader.EndOfStream)
            {
                var sym = (char)reader.Read();
                sb.Append(sym);

                if (!excludeBrackets.Contains(sym))
                {
                    if (CharTools.IsClosingBracket(sym))
                    {
                        if (CharTools.GetOpeningBracket(sym) == bracketStack.Peek())
                        {
                            bracketStack.Pop();
                        }
                        else
                        {
                            var symCount = Math.Min(sb.Length, 10);
                            var pos = sb.Length - symCount;
                            var message = $"No closing bracket to {bracketStack.Peek()}. Last: \'{sb.ToString(pos, symCount)}\'";
                        }
                    }
                    else if (CharTools.IsOpeningBracket(sym))
                    {
                        bracketStack.Push(sym);
                    }
                }
            }

            return sb.ToString();
        }

        private static HashSet<char> FormatExcludeBracketsArgs(char[] excludeBrackets)
        {
            var excludeDict = new HashSet<char>();

            foreach (var sym in excludeBrackets)
            {
                if (!excludeDict.Contains(sym))
                {
                    if (CharTools.IsOpeningBracket(sym))
                    {
                        var closing = CharTools.GetClosingBracket(sym);
                        excludeDict.Add(sym);
                        excludeDict.Add(closing);
                    }
                    else if (CharTools.IsClosingBracket(sym))
                    {
                        var opening = CharTools.GetClosingBracket(sym);
                        excludeDict.Add(sym);
                        excludeDict.Add(opening);
                    }
                    else
                    {
                        throw new ArgumentException($"{sym} isn't bracket");
                    }
                }
            }

            return excludeDict;
        }

        /// <summary>
        /// Reads all to first match excluding it.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static string ReadToFirstMatch(this UnbufferedStreamReader reader, string match)
        {
            ValidateStreamNotReachedEnd(reader);

            var sb = new StringBuilder();

            // I use it for increasing perfomance (in long streams it really can make).
            var charQueue = new Queue<char>(match.Length);

            for (int i = 0; i < match.Length; i++)
            {
                var sym = (char)reader.Read();
                charQueue.Enqueue(sym);
                sb.Append(sym);
            }

            while (!Equality(charQueue, match) && !reader.EndOfStream)
            {
                var sym = (char)reader.Read();
                charQueue.Enqueue(sym);
                charQueue.Dequeue();
                sb.Append(sym);
            }

            // Returning pointer back before match and reconcilation stream and reader postions.
            reader.BaseStream.Seek(-match.Length, SeekOrigin.Current);

            return sb.ToString(0, sb.Length - match.Length);
        }

        private static bool Equality(Queue<char> cq, string match)
        {
            int k = 0;
            foreach (var item in cq)
            {
                if (item != match[k]) return false;
                k++;
            }

            return true;
        }

        private static void ValidateStreamNotReachedEnd(UnbufferedStreamReader reader)
        {
            if (reader.EndOfStream)
            {
                throw new InvalidOperationException("Stream reached end");
            }
        }
    }
}
