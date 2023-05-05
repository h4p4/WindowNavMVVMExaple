using System.Windows;
using MVVM_OpenNewWindowMinimalExample.Views;
using MVVM_OpenNewWindowMinimalExample.ViewModels;
using System.Threading.Tasks;

namespace MVVM_OpenNewWindowMinimalExample {
    public partial class App : Application {

        public static readonly DisplayRootRegistry<BasicViewModel> DisplayRootRegistry = new DisplayRootRegistry<BasicViewModel>();
        private MainWindowViewModel _mainWindowViewModel;

        public App()
        {
            DisplayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindowView>();
            DisplayRootRegistry.RegisterWindowType<OtherWindowViewModel, ChildWindow>();
            DisplayRootRegistry.RegisterWindowType<DialogWindowViewModel, DialogWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindowViewModel = new MainWindowViewModel();
            await DisplayRootRegistry.ShowModalPresentation(_mainWindowViewModel);
            Shutdown();
        }
    }
}
