using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace fabulous_navigation.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new fabulous_navigation.App());
        }
    }
}
