using System;

namespace Keyfactor.Extensions.Orchestrator.Fortanix.API
{
    class FortanixException : ApplicationException
    {
        public FortanixException(string message) : base(message)
        { }

        public FortanixException(string message, Exception ex) : base(message, ex)
        { }

        public static string FlattenExceptionMessages(Exception ex, string message)
        {
            message += ex.Message + Environment.NewLine;
            if (ex.InnerException != null)
                message = FlattenExceptionMessages(ex.InnerException, message);

            return message;
        }
    }
}
