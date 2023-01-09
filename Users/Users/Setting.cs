using DatabaseUI.Database.Models;

namespace Users
{
    public static class Setting
    {
        public static User UserStudent { get; private set; } = new User("o_novgu_student", "orangepi");
        public static User UserTeacher { get; private set; } = new User("o_novgu_teacher", "orangepi");
    }
}
