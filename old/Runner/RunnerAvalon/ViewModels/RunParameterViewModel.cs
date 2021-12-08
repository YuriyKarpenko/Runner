using System.IO;
using System.Linq;
using System.Windows;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
	class RunParameterViewModel : BaseViewModel
	{
		private readonly OptionItem _optionItem;

		public ParameterItem ParameterItem;

		public string[] List { get; private set; }

		private string? value;
		public string? Value
		{
			get => value;
			set
			{
				if (this.value != value)
				{
					this.value = value;
					this.RaisePropertyChanged(nameof(Value));
					this.RaisePropertyChanged(nameof(IsValid));
				}
			}
		}


#if AVALONIA
		public bool VisibilityCombo => ParameterItem.ParameterType == ParameterType.FileMask;
		public bool VisibilityText => ParameterItem.ParameterType == ParameterType.Const;
#else
		public Visibility VisibilityCombo => ParameterItem.ParameterType == ParameterType.FileMask ? Visibility.Visible : Visibility.Collapsed;
		public Visibility VisibilityText => ParameterItem.ParameterType == ParameterType.Const ? Visibility.Visible : Visibility.Collapsed;
#endif


		public bool IsValid => !string.IsNullOrEmpty(Value);

		public RunParameterViewModel(OptionItem optionItem, ParameterItem parameterItem)
		{
			_optionItem = optionItem;
			ParameterItem = parameterItem;

			Refresh();
		}

		private void Refresh()
		{
			var currDir = Path.GetDirectoryName(_optionItem.ProgramPath) ?? Directory.GetCurrentDirectory();
			if (Directory.Exists(currDir))
			{
				var mask = Path.GetFileName(ParameterItem.Value);
				List = ParameterItem.ParameterType switch
				{
					ParameterType.FileMask => Directory.GetFiles(currDir, mask),//.Select(i => Path.GetFileName(i)).ToArray(),
					_ => new string[0]
				};
				this.RaisePropertyChanged(nameof(List));

				Value = ParameterItem.ParameterType switch
				{
					ParameterType.Const => ParameterItem.Value,
					_ => null
				};
				this.RaisePropertyChanged(nameof(Value));

				this.RaisePropertyChanged(nameof(IsValid));
			}
		}
	}
}
