using System;

namespace PlayersWallet.Persistence.Exceptions
{
    public class TransactionsListEmptyException : ExceptionBase
    {
        public TransactionsListEmptyException()
        {
        }

        public TransactionsListEmptyException(string message)
            : base(message)
        {
        }

		public TransactionsListEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}