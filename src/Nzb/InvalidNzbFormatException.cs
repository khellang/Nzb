using System;

namespace Nzb
{
    public class InvalidNzbFormatException : Exception
    {
        public InvalidNzbFormatException(string message) : base(message)
        {
        }
    }
}