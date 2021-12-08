using System;
using System.IO;
using System.Reactive.Linq;

using Newtonsoft.Json;

using Reactive.Bindings;

using Runner.Models;

namespace Runner.ViewModels
{
    public class VmMainWindow : VmBase
    {
        public enum Page { Options, Runner }

        private const string OptionFileName = "Runner.config";


        private readonly Options _options;

        public VmMainWindow()
        {
            _options = GetOptions();

            SelectedPage = new ReactiveProperty<Page>(Page.Runner);

            ViewModel = SelectedPage.Select(SelectVm).ToReadOnlyReactiveProperty();
        }


        public Page[] Pages { get; } = (Page[])Enum.GetValues(typeof(Page));

        public IReactiveProperty<Page> SelectedPage { get; }

        public IReadOnlyReactiveProperty<VmBase?> ViewModel { get; }


        private Options GetOptions()
        {
            if (File.Exists(OptionFileName))
            {
                var content = File.ReadAllText(OptionFileName);
                return JsonConvert.DeserializeObject<Options>(content) ?? new Options();
            }
            return new Options();
        }

        private VmBase SelectVm(Page page)
            => page switch
            {
                Page.Options => new OptionsViewModel(_options),
                Page.Runner => new RunViewModel(_options),
                _ => throw new IndexOutOfRangeException()
            };

        public static void SaveOptions(Options options)
        {
            var content = JsonConvert.SerializeObject(options);
            File.WriteAllText(OptionFileName, content);
        }
    }
}
