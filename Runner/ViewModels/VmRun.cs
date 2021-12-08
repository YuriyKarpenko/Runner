using System.Linq;

using Runner.Models;

namespace Runner.ViewModels
{
    class VmRun : VmBase
    {
        private readonly Options _options;

        public VmRunItem[] RunList { get; private set; }


        public VmRun(Options options)
        {
            _options = options;

            RunList = _options.Items
                .Select(i => new VmRunItem(i))
                .ToArray();
        }
    }
}
