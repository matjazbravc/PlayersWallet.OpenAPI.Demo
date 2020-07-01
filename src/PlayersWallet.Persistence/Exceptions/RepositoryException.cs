using System;

namespace PlayersWallet.Persistence.Exceptions
{
    public class RepositoryException : ExceptionBase
    {
        public RepositoryException()
        {
        }

        public RepositoryException(string message)
            : base(message)
        {
        }

		public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}