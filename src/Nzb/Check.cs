using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Nzb
{
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Validates that the given argument value is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of argument.</typeparam>
        /// <param name="value">The argument value.</param>
        /// <param name="parameterName">Name of the parameter that is validated.</param>
        /// <returns>The argument value.</returns>
        /// <exception cref="ArgumentNullException">If the value is <c>null</c></exception>
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration, ValidatedNotNull] T value, [InvokerParameterName, NotNull] string parameterName)
        {
            if (!ReferenceEquals(value, null))
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName, $"The argument '{parameterName}' must not be null.");
        }

        /// <summary>
        /// Validates that the string value is not <c>null</c> or empty.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="parameterName">Name of the parameter that is validated.</param>
        /// <returns>The string value.</returns>
        /// <exception cref="ArgumentNullException">If the string value is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If the string value is empty (length is 0).</exception>
        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty([ValidatedNotNull] string value, [InvokerParameterName, NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName, $"The argument '{parameterName}' must not be null.");
            }

            var trimmedValue = value.Trim();

            if (trimmedValue.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException($"The string argument '{parameterName}' must not be empty.", parameterName);
            }

            return value;
        }

        /// <summary>
        /// When applied to a parameter, this attribute provides an indication to code analysis that the argument has been null checked.
        /// </summary>
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
