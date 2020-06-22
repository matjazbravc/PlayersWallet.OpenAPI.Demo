using System.Net;
using PlayersWallet.Core.Errors.Base;
using Microsoft.AspNetCore.Http;

namespace PlayersWallet.Core.Errors
{
	public class NotFoundError : BaseError
	{
		public NotFoundError()
			: base(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString())
		{
		}
		
		public NotFoundError(string message)
			: base(StatusCodes.Status404NotFound, HttpStatusCode.NotFound.ToString(), message)
		{
		}
	}
}
