using Reactive.Bindings;

using System;
using System.Windows.Input;

namespace Runner.ViewModels
{

    public class CommandWrapper
    {
        public static CommandWrapper Create(string name, Action execute, IObservable<bool>? canExecute = null, string? imagePath = null)
        {
            var cmd = canExecute == null
                ? new ReactiveCommand()
                : new ReactiveCommand(canExecute);
            cmd.Subscribe(execute);
            return new CommandWrapper(name, cmd, imagePath ?? string.Empty);
        }

        public static CommandWrapper Create(string name, ICommand command, string? imagePath = null)
            => new CommandWrapper(name, command, imagePath ?? string.Empty);


        public CommandWrapper(string name, ICommand command, string imagePath)
        {
            Name = name;
            Command = command;
            ImagePath = imagePath;
        }

        public string Name { get; }
        public string ImagePath { get; }
        public ICommand Command { get; }
    }

    /*
    public class SimpleCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool> _canExecute;
        public string Text { get; }
        public string? Name { get; }


        public SimpleCommand(string text, Action<object?> execute, Func<object?, bool>? canExecute = null, string? name = null)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute ?? (o => true);
            Text = text;
            Name = name;
        }
        public SimpleCommand(string text, Action execute, Func<bool>? canExecute = null, string? name = null)
            : this(text, p => execute(), p => canExecute?.Invoke() ?? true, name)
        {
        }


        bool ICommand.CanExecute(object? parameter)
            => _canExecute(parameter);
        void ICommand.Execute(object? parameter)
            => _execute(parameter);

        event EventHandler? ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    //  */
}
