using System.Net;

namespace ApiTask
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
