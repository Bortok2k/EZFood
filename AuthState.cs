using Microsoft.JSInterop;

namespace EZFood
{
    public static class AuthState
    {
        private static IJSRuntime _js;

        public static void Init(IJSRuntime js)
        {
            _js = js;
        }

        public static string Token { get; set; } = "";
        public static string UserName { get; set; } = "";
        public static int Rol { get; set; } = 0;

        public static bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public static bool IsAdmin => Rol == 1;
        public static bool IsEmployee => Rol == 2;

        public static async Task SaveSession(string token, string userName, int rol)
        {
            Token = token;
            UserName = userName;
            Rol = rol;
            await _js.InvokeVoidAsync("localStorage.setItem", "ezfood_token", token);
            await _js.InvokeVoidAsync("localStorage.setItem", "ezfood_user", userName);
            await _js.InvokeVoidAsync("localStorage.setItem", "ezfood_rol", rol.ToString());
        }

        public static async Task LoadSession()
        {
            Token = await _js.InvokeAsync<string>("localStorage.getItem", "ezfood_token") ?? "";
            UserName = await _js.InvokeAsync<string>("localStorage.getItem", "ezfood_user") ?? "";
            var rolStr = await _js.InvokeAsync<string>("localStorage.getItem", "ezfood_rol") ?? "0";
            Rol = int.TryParse(rolStr, out var r) ? r : 0;
        }

        public static async Task Logout()
        {
            Token = "";
            UserName = "";
            Rol = 0;
            await _js.InvokeVoidAsync("localStorage.removeItem", "ezfood_token");
            await _js.InvokeVoidAsync("localStorage.removeItem", "ezfood_user");
            await _js.InvokeVoidAsync("localStorage.removeItem", "ezfood_rol");
        }
    }
}