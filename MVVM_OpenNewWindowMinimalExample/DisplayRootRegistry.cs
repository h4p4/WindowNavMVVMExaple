using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_OpenNewWindowMinimalExample
{
    public class DisplayRootRegistry<TBaseVM>
    {
        private Dictionary<Type, Type> _vmToWindowMapping = new Dictionary<Type, Type>();
        private Dictionary<object, Window> _openedWindows = new Dictionary<object, Window>();

        public void RegisterWindowType<VM, Win>() where Win : Window, new() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (_vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is already registered");
            _vmToWindowMapping[vmType] = typeof(Win);
        }

        public void UnregisterWindowType<VM>()
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot unregister interfaces");
            if (!_vmToWindowMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is not registered");
            _vmToWindowMapping.Remove(vmType);
        }

        public void ShowPresentation(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (_openedWindows.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var window = InstantiateWindowWithVM(vm);
            window.Show();
            _openedWindows[vm] = window;
        }

        public void HidePresentation(object vm)
        {
            Window window;
            if (!_openedWindows.TryGetValue(vm, out window))
                throw new InvalidOperationException("UI for this VM is not displayed");
            window.Close();
            _openedWindows.Remove(vm);
        }

        public async Task ShowModalPresentation(object vm)
        {
            var window = InstantiateWindowWithVM(vm);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await window.Dispatcher.InvokeAsync(() => window.ShowDialog());
        }

        private Window InstantiateWindowWithVM(object vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            Type windowType = null;

            var vmType = vm.GetType();
            while (vmType != null && !_vmToWindowMapping.TryGetValue(vmType, out windowType))
                vmType = vmType.BaseType;

            if (windowType == null)
                throw new ArgumentException(
                    $"No registered window type for argument type {vm.GetType().FullName}");

            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = vm;
            return window;
        }

    }
}
