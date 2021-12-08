using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
	class RunItemViewModel : BaseViewModel
	{
		private readonly OptionItem _optionItem;
		private System.Diagnostics.Process cmd;

		public string? Description => _optionItem.Description;
		public string ProgramPath => _optionItem.ProgramPath;
		public System.Collections.ObjectModel.ObservableCollection<RunParameterViewModel> Parameters { get; private set; }
		public string RunPath { get; set; }

		public RunItemViewModel(OptionItem optionItem)
		{
			_optionItem = optionItem;
			ActRefresh();
			InitCommands();
		}

		#region actions

		public ICommand CmdRefresh { get; private set; }
		public ICommand CmdRun { get; private set; }

		private void InitCommands()
		{
			CmdRefresh = CommandWrapper.Create("Refresh", ActRefresh);
			//var canRun = this.ObservableForProperty(i => i.Parameters., sel => CanActRun());
		}

		private void ActRefresh()
		{
			Parameters = new System.Collections.ObjectModel.ObservableCollection<RunParameterViewModel>(
				_optionItem.Parameters
				.Select(i => new RunParameterViewModel(_optionItem, i))
				.ToArray()
				);

			var canRun = Observable.Concat( Parameters
				.Select(i => i.ObservableForProperty(i => i.IsValid, i => i//CanActRun()
				))
				)//.Concat();
				//.Merge()
				;

			//var canRun = Parameters
			//	.Select(i => i.ObservableForProperty(i => i.IsValid, i => CanActRun()))
			//	.Amb();

			//var canRun = Parameters.Select(vm => vm
			//	.WhenAnyValue(a => a.Value)
			//	.Select(i => CanActRun())
			//	).FirstOrDefault();

			//var canRun = Parameters.Select(vm => Observable
			//	.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(a => vm.PropertyChanged += a, a => vm.PropertyChanged -= a)
			//	.Where(i => i.EventArgs.PropertyName == nameof(RunParameterViewModel.Value))
			//	.Select(i => CanActRun())
			//	).FirstOrDefault();
			//var canRun = Parameters.ToObservable().Select(i => CanActRun());
			//var canRun = new ObservableCollection(Parameters).Select(i => CanActRun());

			//canRun.Subscribe(b => { });

			CmdRun = CommandWrapper.Create("Run!", ActRun, canRun);
			//this.RaisePropertyChanged(nameof(CmdRun));
			//System.IObservable<bool> canRun = System.Reactive.Linq.Observable.FromEventPattern(Parameters.CollectionChanged this.WhenAnyValue(i => i.Parameters., sel => CanActRun());
		}

		private void ActRun()
		{
			var psi = new System.Diagnostics.ProcessStartInfo
			{
				//Arguments = "/k " + RunPath,
				//Arguments = "/k ipconfig",
				//CreateNoWindow = false,
				FileName = "cmd.exe",
				//RedirectStandardError = true,
				RedirectStandardInput = true,
				//RedirectStandardOutput = true,
				//Verb = "runas",
				WorkingDirectory = System.IO.Path.GetDirectoryName(_optionItem.ProgramPath)
			};

			cmd = System.Diagnostics.Process.Start(psi);
			cmd.StandardInput.WriteLine(RunPath);
			//cmd.StandardInput.Flush();

			//cmd.WaitForExit();
		}

		private bool CanActRun()
		{
			var res = !string.IsNullOrEmpty(ProgramPath)
			&& Parameters.All(i => i.IsValid);

			/*if (res)
			{
				var paramsList = Parameters.Select(i => $"\"{i.Value}\"");
				//Avalonia.Threading.Dispatcher.UIThread.Invoke(
				RunPath = $"\"{_optionItem.ProgramPath}\" {string.Join(" ", paramsList)}";
				this.RaisePropertyChanged(nameof(RunPath));
			}
			*/
			return res;
		}

		#endregion

		class ObservableCollection : ObservableBase<NotifyCollectionChangedEventArgs>
		{
			private readonly INotifyCollectionChanged _notifyCollection;
			public ObservableCollection(INotifyCollectionChanged notifyCollection)
			{
				_notifyCollection = notifyCollection;
			}

			protected override IDisposable SubscribeCore(IObserver<NotifyCollectionChangedEventArgs> observer)
				=> new ObserverHandler(_notifyCollection, observer);


			class ObserverHandler: IDisposable
			{
				private readonly INotifyCollectionChanged _notifyCollection;
				private readonly IObserver<NotifyCollectionChangedEventArgs> _observer;

				public ObserverHandler(INotifyCollectionChanged notifyCollection, IObserver<NotifyCollectionChangedEventArgs> observer)
				{
					_notifyCollection = notifyCollection;
					_observer = observer;
					_notifyCollection.CollectionChanged += _notifyCollection_CollectionChanged;
				}

				private void _notifyCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
					=> _observer.OnNext(e);

				public void Dispose()
				{
					_observer.OnCompleted();
					_notifyCollection.CollectionChanged -= _notifyCollection_CollectionChanged;
				}
			}
		}
	}
}
