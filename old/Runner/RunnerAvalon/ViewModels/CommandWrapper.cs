using System;
using System.Windows.Input;

namespace Runner.ViewModels
{

#if AVALONIA
#else
#endif

#if true || AVALONIA
	public class CommandWrapper
	{
		public static CommandWrapper<System.Reactive.Unit, System.Reactive.Unit> Create(string text, Action execute, IObservable<bool>? canExecute = null)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			return new CommandWrapper<System.Reactive.Unit, System.Reactive.Unit>(text,
				p => System.Reactive.Linq.Observable.Create((IObserver<System.Reactive.Unit> observer) =>
			{
				execute();
				observer.OnNext(System.Reactive.Unit.Default);
				observer.OnCompleted();
				return System.Reactive.Disposables.Disposable.Empty;
			}), canExecute ?? System.Reactive.Linq.Observable.Return(true));
		}

		public static CommandWrapper<TParam, System.Reactive.Unit> Create<TParam>(string text, Action<TParam> execute, IObservable<bool>? canExecute = null)
		{
			//ReactiveUI.ReactiveCommand.Create();
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			return new CommandWrapper<TParam, System.Reactive.Unit>(text,
				p => System.Reactive.Linq.Observable.Create((IObserver<System.Reactive.Unit> observer) =>
			{
				execute(p);
				observer.OnNext(System.Reactive.Unit.Default);
				observer.OnCompleted();
				return System.Reactive.Disposables.Disposable.Empty;
			}), canExecute ?? System.Reactive.Linq.Observable.Return(true));
		}
	}

	public class CommandWrapper<TParam, TRes> : ReactiveUI.ReactiveCommand<TParam, TRes>
	{
		public object Name { get; }
		public string Text { get; }

		public CommandWrapper(string text, Func<TParam, IObservable<TRes>> execute, IObservable<bool>? canExecute)
			: base (execute, canExecute, ReactiveUI.RxApp.MainThreadScheduler
#if AVALONIA
#else
	, ReactiveUI.RxApp.MainThreadScheduler
#endif
				  )
		//: base (ReactiveUI.ReactiveCommand.Create(action), text)
		{
			Text = text;
			Name = text;
		}
	}
#else
	public class CommandWrapper //: RoutedUICommand
	{
		public static ICommand Create(string text, Action execute, IObservable<bool>? canExecute = null, string? name = null)
			=> new SimpleCommand(text, execute, name: name);
		//public static ICommand Create(string text, Action execute, Func<bool> canExecute, string? name = null)
		//	=> new SimpleCommand(text, execute, canExecute, name);
		//public static ICommand Create(string text, Action<object?> execute, Func<object?, bool> canExecute, string? name = null)
		//	=> new SimpleCommand(text, execute, canExecute, name);

		//public CommandWrapper(string text, string? name = null) 
		//	: base(text, name ?? text, typeof(CommandWrapper))
		//{
		//}

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
	}
#endif
}
