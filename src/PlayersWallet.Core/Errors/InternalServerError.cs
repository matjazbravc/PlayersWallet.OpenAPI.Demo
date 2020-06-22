using System.Net;
using PlayersWallet.Core.Errors.Base;
using Microsoft.AspNetCore.Http;

namespace PlayersWallet.Core.Errors
{
	public class InternalServerError : BaseError
	{
		public InternalServerError()
			: base(StatusCodes.Status500InternalServerError, HttpStatusCode.InternalServerError.ToString())
		{
		}
		
		public InternalServerError(string message)
			: base(StatusCodes.Status500InternalServerError, HttpStatusCode.InternalServerError.ToString(), message)
		{
		}
	}
}
