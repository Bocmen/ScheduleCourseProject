using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedBasePages.Interface
{
    public interface IPage
    {
        ContentPage GetPage();
        Task<bool> Load();
    }
}
