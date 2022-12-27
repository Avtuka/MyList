using System;

namespace MyList
{
    internal class MoreThanOneMatchingElementException : Exception
    {
        public MoreThanOneMatchingElementException(string text) : base(text)
        {
        }
    }
}