using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExtJsNamespaceDeployer
{
    static class CharTools
    {
        static private readonly char[] _openingBrackets = new char[] {'[','(','<','{'};
        static private readonly char[] _closingBrackets = new char[] {']',')','>','}'};

        public static bool IsBracket(char c)
        {
            return _openingBrackets.Contains(c) || _closingBrackets.Contains(c);
        }
        
        public static bool IsOpeningBracket(char c)
        {
            return _openingBrackets.Contains(c);
        }

        public static bool IsClosingBracket(char c)
        {
            return _closingBrackets.Contains(c);
        }

        public static char GetClosingBracket(char opening)
        {
            var index = IndexOf(_openingBrackets, opening);

            if(index == null)
            {
                throw new ArgumentException($"{opening} isn't opening bracket.");
            }

            return _closingBrackets[index.Value];
        }

        public static char GetOpeningBracket(char closing)
        {
            var index = IndexOf(_closingBrackets, closing);

            if (index == null)
            {
                throw new ArgumentException($"{closing} isn't opening bracket.");
            }

            return _openingBrackets[index.Value];
        }

        private static int? IndexOf(char[] brackets, char sym)
        { 
            return Enumerable.Range(0, brackets.Length)
                .Cast<int?>()
                .FirstOrDefault(i => brackets[i.Value] == sym);
        }
    }
}
