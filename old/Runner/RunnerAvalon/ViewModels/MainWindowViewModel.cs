using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

using Newtonsoft.Json;

using ReactiveUI;

#if AVALONIA
using Avalonia.Controls;
#else
#endif

using Runner.Models;

namespace Runner.ViewModels
{
	public class MainWindowViewModel : BaseViewModel
    {
		public enum Page { Options, Runner }

		private const string OptionFileName = "Runner.config";

		private readonly Options _options;

		public Page[] Pages { get; } = Enum.GetValues<Page>();

		public BaseViewModel ViewModel { get; private set; }

		private Page page;
		public Page SelectedPage
		{
			get => page;
			set
			{
				if (page != value)
				{
					page = value;
					this.RaisePropertyChanged(nameof(SelectedPage));
					ViewModel = page switch
					{
						Page.Options => new OptionsViewModel(_options),
						Page.Runner => new RunViewModel(_options),
						_ => throw new IndexOutOfRangeException()
					};
					this.RaisePropertyChanged(nameof(ViewModel));
				}
			}
		}

		public MainWindowViewModel()
		{
			_options = GetOptions();
			SelectedPage = Page.Runner;
		}


		private Options GetOptions()
		{
			if (File.Exists(OptionFileName))
			{
				var content = File.ReadAllText(OptionFileName);
				return JsonConvert.DeserializeObject<Options>(content) ?? new Options();
			}
			return new Options();
		}

		public static void SaveOptions(Options options)
		{
			var content = JsonConvert.SerializeObject(options);
			File.WriteAllText(OptionFileName, content);
		}
	}
}
