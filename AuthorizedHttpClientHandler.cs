using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EZFood
{
    public class AuthorizedHttpMessageHandler : DelegatingHandler
    {
        private readonly NavigationManager _nav;
        private readonly IJSRuntime _js;
        private static bool _redirigiendo = false;

        public AuthorizedHttpMessageHandler(NavigationManager nav, IJSRuntime js)
        {
            _nav = nav;
            _js = js;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(AuthState.Token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", AuthState.Token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // 403 = token vencido, inválido o ausente (ver auth.middleware.js).
            // 401 NO dispara esto: significa "rol sin permiso", token sigue vigente.
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                && AuthState.IsAuthenticated
                && !_redirigiendo)
            {
                _redirigiendo = true;
                AuthState.Logout();

                try { await _js.InvokeVoidAsync("authStorage.clear"); }
                catch { /* la app puede estar descargándose; se ignora */ }

                _nav.NavigateTo("/login?expirada=1", forceLoad: true);
            }

            return response;
        }
    }
}