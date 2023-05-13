using System;

namespace Vemahk.Kernel.Exceptions
{
    public class InsufficientConfigurationException : Exception
    {
        public InsufficientConfigurationException(string message) : base(message) { }
    }
}