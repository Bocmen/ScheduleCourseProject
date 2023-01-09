namespace SharedBasePages.Models
{
    public class SelectionLoaderContentTable
    {
        public LoaderContentTable DefaultLoader { get; private set; }
        public LoaderContentTable FullLoader { get; private set; }

        public string TextButtonSwitchDefault { get; private set; }
        public string TextButtonSwitchFull { get; private set; }

        public SelectionLoaderContentTable(LoaderContentTable defaultLoader, string textButtonSwitchDefault, LoaderContentTable fullLoader, string textButtonSwitchFull)
        {
            DefaultLoader = defaultLoader;
            FullLoader = fullLoader;
            TextButtonSwitchDefault = textButtonSwitchDefault;
            TextButtonSwitchFull = textButtonSwitchFull;
        }
    }
}
