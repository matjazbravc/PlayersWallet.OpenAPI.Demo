using System.Net.Http;
using Xunit;

namespace PlayersWallet.Tests.Services
{
    /// <summary>
    /// Base Controller tests IClassFixture
    /// </summary>
    public class ControllerTestsBase : IClassFixture<WebApiTestFactory>
    {
        protected HttpClient Client;

        public ControllerTestsBase(WebApiTestFactory factory)
        {
            Client = factory.CreateClient();
        }
    }
}