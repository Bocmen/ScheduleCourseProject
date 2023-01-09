using DatabaseUI.Database.Models;

namespace Admin
{
    internal static class Setting
    {
        internal static User User { get; private set; } = new User("o_novgu_admin", "orangepi");
    }
}
