using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;

#if AVALONIA
using Avalonia;
using Avalonia.Controls;
#else
#endif

using Runner.Models;

namespace Runner.ViewModels
{
	public class OptionItemViewModel : BaseViewModel
	{
		public OptionItem OptionItem { get; set; }
		public OptionParameterViewModel[] Parameters { get; private set; }

		private OptionParameterViewModel? _Selected;
		public OptionParameterViewModel? Selected
		{
			get => _Selected;
			set => this.RaiseAndSetIfChanged(ref _Selected, value);
		}


		public OptionItemViewModel(OptionItem optionItem)
		{
			OptionItem = optionItem;

			InitOptions();
			InitCommands();
		}


		private void InitOptions()
		{
			Parameters = OptionItem.Parameters.Select(i => new OptionParameterViewModel(i)).ToArray();
			this.RaisePropertyChanged(nameof(Parameters));
		}

		#region actions

		public ICommand CmdOpen { get; private set; }

		public ICommand CmdRefresh { get; private set; }
		public ICommand CmdAdd { get; private set; }
		public ICommand CmdDel { get; private set; }

		protected void InitCommands()
		{
			CmdOpen = CommandWrapper.Create("Open", ActOpenFile);

			CmdRefresh = CommandWrapper.Create("Refresh", InitOptions);
			CmdAdd = CommandWrapper.Create("Add", ActAddItem);

			//CmdDel = ReactiveCommand.Create(ActDel, this.WhenAny(i => i.Selected, i => i.Value != null));
			//CmdDel = ReactiveCommand.Create(ActDel, this.ObservableForProperty(i => i.Selected, sel => sel != null));
			//CmdDel = ReactiveCommand.Create(ActDel, System.Reactive.Linq.Observable. Create(i => i.Selected, i => i.Value != null));
			var canDel = this.ObservableForProperty(i => i.Selected, sel => sel != null);
			CmdDel = CommandWrapper.Create("Del", ActDel, canDel);
		}

		private void ActAddItem()
		{
			OptionItem.Parameters.Add(new ParameterItem());
			InitOptions();
		}

		private void ActDel()
		{
			if (Selected != null)
			{
				OptionItem.Parameters.Remove(Selected.ParameterItem);
				InitOptions();
			}
		}

		private void ActOpenFile()
		{
			var filter = new Dictionary<string, List<string>>
			{
				{ "Programs", new List<string> { "exe", "py"} },
				{ "All", new List<string>{ "*"} },
			};
#if AVALONIA
			var dialog = new OpenFileDialog
			{
				//CheckPathExists = true,
				Directory = Path.GetDirectoryName(OptionItem.ProgramPath),
				Filters = filter.Select(i => new FileDialogFilter { Extensions = i.Value, Name = i.Key }).ToList(),
			};
			var files = dialog.ShowAsync(new Window()).Result;
			if (files?.Any() == true)
			{
				OptionItem.ProgramPath = files[0];
				this.RaisePropertyChanged(nameof(OptionItem));
			}
#else
			var dialog = new Microsoft.Win32.OpenFileDialog
			{
				CheckPathExists = true,
				Filter = string.Join("|", filter.Select(i => string.Join("|", i.Value.Select(e => $"*.{e}|*.{e}")))),
			};
			dialog.FileName = OptionItem.ProgramPath;
			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
			{
				OptionItem.ProgramPath = dialog.FileName;
				this.RaisePropertyChanged(nameof(OptionItem));
			}
#endif
		}

		#endregion
	}
}
