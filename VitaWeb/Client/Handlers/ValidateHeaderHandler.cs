namespace VitaWeb.Client.Handlers
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(!request.Headers.Contains("Authorization"))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Header não contêm Authorization Token JWT"
                };
            }

            if (!request.Headers.Contains("tenant"))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Header não contêm tenant com o número de licença"
                };
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
