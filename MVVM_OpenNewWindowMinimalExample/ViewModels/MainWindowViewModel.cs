using System;
using System.Windows;
using System.Windows.Input;

namespace MVVM_OpenNewWindowMinimalExample.ViewModels
{
    class MainWindowViewModel : BasicViewModel
    {
        // Закрытые поля команд
        private ICommand _openChildWindow;

        private ICommand _openDialogWindow;

        // Свойства доступные только для чтения для обращения к командам и их инициализации
        public ICommand OpenChildWindow
        {
            get
            {
                if (_openChildWindow == null)
                    _openChildWindow = new OpenChildWindowCommand(this);
                return _openChildWindow;
            }
        }
        public ICommand OpenDialogWindow
        {
            get
            {
                if (_openDialogWindow == null)
                    _openDialogWindow = new OpenDialogWindowCommand(this);
                return _openDialogWindow;
            }
        }
    }

    abstract class OpenWindowCommand : ICommand
    {
        protected MainWindowViewModel _mainWindowVeiwModel;

        public OpenWindowCommand(MainWindowViewModel mainWindowVeiwModel)
        {
            _mainWindowVeiwModel = mainWindowVeiwModel;
        }

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }

    class OpenChildWindowCommand : OpenWindowCommand
    {
        public OpenChildWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
        {
        }
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            var displayRootRegistry = App.DisplayRootRegistry;
            var otherWindowViewModel = new OtherWindowViewModel();
            displayRootRegistry.ShowPresentation(otherWindowViewModel);
        }
    }

    class OpenDialogWindowCommand : OpenWindowCommand
    {
        public OpenDialogWindowCommand(MainWindowViewModel mainWindowVeiwModel) : base(mainWindowVeiwModel)
        {
        }
        public override bool CanExecute(object parameter) => true;
        public override async void Execute(object parameter)
        {
            var displayRootRegistry = App.DisplayRootRegistry;
            var dialogWindowViewModel = new DialogWindowViewModel();
            await displayRootRegistry.ShowModalPresentation(dialogWindowViewModel);

        }
    }
}