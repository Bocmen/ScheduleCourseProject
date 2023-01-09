using DatabaseUI.Database.Models;
namespace UserManagement
{
    public static class Setting
    {
        public static User User { get; private set; } = new User("o_novgu_user_management", "orangepi");
    }
}
