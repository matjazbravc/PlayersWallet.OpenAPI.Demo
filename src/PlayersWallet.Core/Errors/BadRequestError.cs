using System.Net;
using PlayersWallet.Core.Errors.Base;
using Microsoft.AspNetCore.Http;

namespace PlayersWallet.Core.Errors
{
	public class BadRequestError : BaseError
	{
		public BadRequestError()
			: base(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString())
		{
		}

		public BadRequestError(string message)
			: base(StatusCodes.Status400BadRequest, HttpStatusCode.BadRequest.ToString(), message)
		{
		}
	}
}
