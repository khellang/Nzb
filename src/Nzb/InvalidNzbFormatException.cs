using System;

namespace Nzb
{
    /// <summary>
    /// Thrown if parsing of an NZB document fails.
    /// </summary>
    public class InvalidNzbFormatException : FormatException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNzbFormatException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidNzbFormatException(string message) : base(message)
        {
        }
    }
}
