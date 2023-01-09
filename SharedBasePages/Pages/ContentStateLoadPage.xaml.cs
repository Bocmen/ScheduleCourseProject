using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SharedBasePages.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentStateLoadPage : ContentPage
    {
        public ContentStateLoadPage()
        {
            InitializeComponent();
        }
    }
}