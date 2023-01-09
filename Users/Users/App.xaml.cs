using Xamarin.Forms;

namespace Users
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new RegisterPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
