using SMSApi.Api;
using SMSApi.Api.Response;

namespace SmsSender
{
    public class SMS
    {
        private string _token;
        private IClient _client;
        private SMSFactory _factory;

        public SMS(string token) 
        {
            _token = token;
            _client = new ClientOAuth(_token);
            _factory = new SMSFactory(_client);
        }

        public Status Send(string clientNumber, string message) 
        {
            return _factory.ActionSend()
                .SetTo(clientNumber)
                .SetText(message)
                .Execute();
        }
    }
}
