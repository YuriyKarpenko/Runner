using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Reactive.Bindings;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
    public class OptionsViewModel : VmBase
    {
        private readonly Options _options;

        public OptionsViewModel(Options options)
        {
            _options = options ?? throw new ArgumentNullException("options");

            Selected = new ReactiveProperty<OptionItemViewModel?>();

            InitOptions();

            InitCommands();
        }


        public ObservableCollection<OptionItemViewModel> OptionItems { get; private set; }

        public IReactiveProperty<OptionItemViewModel?> Selected { get; }


        private void InitOptions()
        {
            OptionItems = new ObservableCollection<OptionItemViewModel>(_options.Items.Select(OptionItemViewModel.FromOptionItem));
            this.RaisePropertyChanged(nameof(OptionItems));
        }

        #region actions

        public CommandWrapper CmdAdd { get; private set; }
        public CommandWrapper CmdDel { get; private set; }
        public CommandWrapper CmdSave { get; private set; }

        private void InitCommands()
        {
            CmdAdd = CommandWrapper.Create("Add", ActAddItem);
            var canDel = Selected.Select(i => i != null);
            CmdDel = CommandWrapper.Create("Del", ActDel, canDel);
            CmdSave = CommandWrapper.Create("Save", SaveOptions);
        }

        private void ActAddItem()
        {
            _options.Items.Add(new OptionItem());
            InitOptions();
        }

        private void ActDel()
        {
            if (Selected.Value != null)
            {
                _options.Items.Remove(Selected.Value.OptionItem);
                InitOptions();
            }
        }

        private void SaveOptions()
        {
            VmMainWindow.SaveOptions(_options);
        }

        #endregion
    }
}
