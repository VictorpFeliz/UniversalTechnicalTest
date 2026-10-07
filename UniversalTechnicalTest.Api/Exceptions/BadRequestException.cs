using System.Security.Cryptography.X509Certificates;

namespace UniversalTechnicalTest.Api.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
