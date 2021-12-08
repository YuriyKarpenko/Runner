using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ReactiveUI;
#if AVALONIA
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Metadata;
#else
#endif

using Runner.Models;

namespace Runner.ViewModels
{
	public class OptionsViewModel : BaseViewModel
	{
		private readonly Options _options;

		public ObservableCollection<OptionItemViewModel> OptionItems { get; private set; }
		private OptionItemViewModel? _Selected;
		public OptionItemViewModel? Selected
		{
			get => _Selected;
			set
			{
				if (value != _Selected)
				{
					_Selected = value;
					this.RaisePropertyChanged(nameof(Selected));
				}
			}
		}

		public OptionsViewModel(Options options)
		{
			_options = options ?? throw new ArgumentNullException("options");

			InitOptions();
			InitCommands();
		}


		private void InitOptions()
		{
			OptionItems = new(_options.OptionItems.Select(i => new OptionItemViewModel(i)));
			this.RaisePropertyChanged(nameof(OptionItems));
		}

		#region actions

		public ICommand CmdAdd { get; private set; }
		public ICommand CmdDel { get; private set; }
		public ICommand CmdSave { get; private set; }

		private void InitCommands()
		{
			CmdAdd = CommandWrapper.Create("Add", ActAddItem);
			var canDel = this.ObservableForProperty(i => i.Selected, sel => sel != null);
			CmdDel = CommandWrapper.Create("Del", ActDel, canDel);
			CmdSave = CommandWrapper.Create("Save", SaveOptions);
		}

		private void ActAddItem()
		{
			_options.OptionItems.Add(new OptionItem());
			InitOptions();
		}

		private void ActDel()
		{
			if (Selected != null)
			{
				_options.OptionItems.Remove(Selected.OptionItem);
				InitOptions();
			}
		}
#if AVALONIA
		//[DependsOn(nameof(Selected))]
#else
#endif
		private void SaveOptions()
		{
			MainWindowViewModel.SaveOptions(_options);
		}

		#endregion
	}
}
