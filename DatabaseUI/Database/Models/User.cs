namespace DatabaseUI.Database.Models
{
    public class User
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
