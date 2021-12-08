using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
	public class OptionParameterViewModel : BaseViewModel
	{
		public static ParameterType[] ParameterTypes { get; } = Enum.GetValues<ParameterType>();

		public ParameterItem ParameterItem { get; }

		public OptionParameterViewModel(ParameterItem parameterItem)
		{
			ParameterItem = parameterItem;
			InitCommands();
		}

		#region action

		public ICommand CmdSelectFolder { get; private set; }

		private void InitCommands()
		{
			var canSelect = this.ObservableForProperty(i => i.ParameterItem, p => p.ParameterType == ParameterType.FileMask);
			CmdSelectFolder = CommandWrapper.Create("...", ActSelectFolder, canSelect);
		}

		private void ActSelectFolder()
		{

#if AVALONIA
			var dialog = new Avalonia.Controls.OpenFolderDialog
			{
				Directory = Path.GetDirectoryName(ParameterItem.Value),
				//Title = "Select a folder",
			};

			var folder = dialog.ShowAsync(new Avalonia.Controls.Window()).Result;
			if (!string.IsNullOrEmpty(folder))
			{
				var mask = Path.GetFileName(ParameterItem.Value);
				ParameterItem.Value = Path.Combine(folder, mask);
				this.RaisePropertyChanged(nameof(ParameterItem));
			}
#else
			var dialog = new Microsoft.Win32.SaveFileDialog
			{
				CheckFileExists = false,
				CheckPathExists = true,
				CreatePrompt = false,
				InitialDirectory = Path.GetDirectoryName(ParameterItem.Value),
				FileName = "select folder",
				OverwritePrompt = false,
				Title = "Select a folder",
			};

			if (dialog.ShowDialog() == true)
			{
				var folder = Path.GetDirectoryName(dialog.FileName);
				var mask = Path.GetFileName(ParameterItem.Value);
				ParameterItem.Value = Path.Combine(folder, mask);
				this.RaisePropertyChanged(nameof(ParameterItem));
			}
#endif
		}

		#endregion
	}
}
