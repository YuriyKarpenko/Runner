using Reactive.Bindings;

using ReactiveUI;

using Runner.Models;

using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Runner.ViewModels
{
    class RunParameterViewModel : VmBase
    {
        private readonly OptionItem _optionItem;
        private readonly ParameterItem _parameterItem;

        public RunParameterViewModel(OptionItem optionItem, ParameterItem parameterItem)
        {
            _optionItem = optionItem;
            _parameterItem = parameterItem;

            IsCombo = parameterItem.ParameterType == ParameterType.FileMask;
            IsText = parameterItem.ParameterType == ParameterType.Const;

            Value = parameterItem.ParameterType switch
            {
                ParameterType.Const => new ReactiveProperty<string?>(parameterItem.Value),
                _ => new ReactiveProperty<string?>(),
            };

            IsValid = Value.Select(i => !string.IsNullOrEmpty(i)).ToReadOnlyReactiveProperty();

            Refresh();
        }


        public string[]? List { get; private set; }
        public bool IsCombo { get; }
        public bool IsText { get; }
        public IReactiveProperty<string?> Value { get; }
        public IReadOnlyReactiveProperty<bool> IsValid { get; }


        public void Refresh()
        {
            if (_parameterItem.ParameterType == ParameterType.FileMask)
            {
                var currDir = Path.GetDirectoryName(_optionItem.ProgramPath) ?? Directory.GetCurrentDirectory();
                var hasDir = Directory.Exists(currDir);
                if (hasDir)
                {
                    var mask = Path.GetFileName(_parameterItem.Value);
                    List = Directory.GetFiles(currDir, mask).Select(AdjustParameter).ToArray();
                    this.RaisePropertyChanged(nameof(List));
                }

                Value.Value = null;
            }
        }

        private string AdjustParameter(string filePath)
        {
            var result = filePath;
            if (!_parameterItem.IncludePath)
            {
                result = Path.GetFileName(result);
            }
            if (!_parameterItem.IncludeExtensions)
            {
                result = Path.GetFileNameWithoutExtension(result);
            }
            return result;
        }
    }
}
