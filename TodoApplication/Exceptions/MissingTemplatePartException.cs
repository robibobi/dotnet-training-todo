using System;

namespace TodoApplication.Exceptions
{
    internal class MissingTemplatePartException : Exception
    {
        public MissingTemplatePartException(string message) : base(message)
        {
        }
    }
}
