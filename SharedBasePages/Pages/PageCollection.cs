using SharedBasePages.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SharedBasePages.Pages
{
    public class PageCollection: TabbedPage
    {

        public PageCollection(IEnumerable<IPage> pages)
        {
            Children.Add(new ContentStateLoadPage());
            Task.Run(() => Init(pages));
        }
        private async Task Init(IEnumerable<IPage> pages)
        {
            foreach (var page in pages)
            {
                if (await page.Load())
                {
                    Device.BeginInvokeOnMainThread(() => Children.Add(page.GetPage()));
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Children.RemoveAt(0);
                TabIndex = 0;
            });
        }
    }
}
