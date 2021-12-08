using System.Linq;

using Runner.Models;

namespace Runner.ViewModels
{
	class RunViewModel : BaseViewModel
	{
		private readonly Options _options;

		public RunItemViewModel[] RunList { get; private set; }

		public RunViewModel(Options options)
		{
			_options = options;

			RunList = _options.OptionItems
				.Select(i => new RunItemViewModel(i))
				.ToArray();
		}
	}
}
