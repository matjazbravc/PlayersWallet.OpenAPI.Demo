using System;

namespace PlayersWallet.Persistence.Exceptions
{
    public class RepositoryNotFoundException : ExceptionBase
    {
        public RepositoryNotFoundException()
        {
        }

        public RepositoryNotFoundException(string message)
            : base(message)
        {
        }

		public RepositoryNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}