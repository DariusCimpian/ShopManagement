namespace Shop.App.Models
{
    public  class CurrentUser
    {
        public static int Id { get; set; }
        public static string Username { get; set; } = string.Empty;
        public static string Role { get; set; } = string.Empty; 
        public static string Email { get; set; } = string.Empty;
        public static bool IsLoggedIn => Id != 0;

        public static void Logout()
        {
            Id = 0;
            Username = string.Empty;
            Role = string.Empty;
            Email = string.Empty;
        }
    }
}