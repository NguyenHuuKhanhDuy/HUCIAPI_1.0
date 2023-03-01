using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels
{
    public class UserVM
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserVM(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
