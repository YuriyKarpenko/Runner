using ReactiveUI;

using Runner.Models;

using System.IO;

namespace Runner.ViewModels
{
    class VmRunParameter : VmBase
    {
        private readonly OptionItem _optionItem;

        public ParameterItem ParameterItem;

        public string[]? List { get; private set; }

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


        public bool VisibilityCombo => ParameterItem.ParameterType == ParameterType.FileMask;
        public bool VisibilityText => ParameterItem.ParameterType == ParameterType.Const;

        public bool IsValid => !string.IsNullOrEmpty(Value);


        public VmRunParameter(OptionItem optionItem, ParameterItem parameterItem)
        {
            _optionItem = optionItem;
            ParameterItem = parameterItem;

            Refresh();
        }


        private void Refresh()
        {
            var currDir = Path.GetDirectoryName(_optionItem.ProgramPath) ?? Directory.GetCurrentDirectory();
            var hasDir = Directory.Exists(currDir);
            if (hasDir)
            {
                var mask = Path.GetFileName(ParameterItem.Value);
                List = ParameterItem.ParameterType switch
                {
                    ParameterType.FileMask => Directory.GetFiles(currDir, mask),//.Select(i => Path.GetFileName(i)).ToArray(),
                    _ => new string[0]
                };
                this.RaisePropertyChanged(nameof(List));
            }

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
