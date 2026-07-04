namespace EZFood
{
    public static class AuthState
    {
        public static string Token { get; set; } = "";
        public static string UserName { get; set; } = "";
        public static int Rol { get; set; } = 0;

        public static bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public static bool IsAdmin => Rol == 1;
        public static bool IsEmployee => Rol == 2;

        public static void Logout()
        {
            Token = "";
            UserName = "";
            Rol = 0;
        }
    }
}