using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InitView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitPage : ContentPage
    {
        private Func<Page> _pageNextOpen;
        private Func<Task> _connect;

        public InitPage(Func<Page> pageNextOpen, Func<Task> connect)
        {
            InitializeComponent();
            _pageNextOpen = pageNextOpen;
            _connect = connect;
            Task.Run(() => Conect_Clicked(null, null));
        }
        private async void Conect_Clicked(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    errorLabel.Text = string.Empty;
                    connectIndicator.IsEnabled = true;
                    connectIndicator.IsVisible = true;
                    Conect.IsVisible = false;
                });
                await _connect.Invoke();
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = _pageNextOpen.Invoke());
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    errorLabel.Text = ex.Message;
                    connectIndicator.IsEnabled = false;
                    connectIndicator.IsVisible = false;
                    Conect.IsVisible = true;
                });
            }
        }
    }
}