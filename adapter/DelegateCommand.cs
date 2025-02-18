using System.Windows.Input;

namespace Icf.ProPresenter.KidsCall
{
    internal class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Func<bool> canExecute, Action execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            this.execute?.Invoke();
        }
    }
}
