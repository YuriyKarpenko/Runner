using System.Linq;

using Runner.Models;

namespace Runner.ViewModels
{
    class RunViewModel : VmBase
    {
        private readonly Options _options;

        public RunItemViewModel[] RunList { get; }


        public RunViewModel(Options options)
        {
            _options = options;

            RunList = _options.Items
                .Select(i => new RunItemViewModel(i))
                .ToArray();
        }
    }
}
