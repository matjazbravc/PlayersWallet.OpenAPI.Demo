namespace PlayersWallet.Core.Errors.Base
{
	public class BaseError
	{
		public BaseError(int statusCode, string statusDescription)
		{
			StatusCode = statusCode;
			StatusDescription = statusDescription;
		}

		public BaseError(int statusCode, string statusDescription, string message)
			: this(statusCode, statusDescription)
		{
			Message = message;
		}

		public string Message { get; private set; }

		public int StatusCode { get; }

		public string StatusDescription { get; }
	}
}