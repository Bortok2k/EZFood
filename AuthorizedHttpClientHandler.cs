namespace EZFood
{
    public class AuthorizedHttpMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(AuthState.Token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", AuthState.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}