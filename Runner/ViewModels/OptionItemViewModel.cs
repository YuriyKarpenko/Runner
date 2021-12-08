using System.Linq;
using System.Reactive.Linq;

using Reactive.Bindings;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
    public class OptionItemViewModel : VmBase
    {
        public static OptionItemViewModel FromOptionItem(OptionItem optionItem)
            => new OptionItemViewModel(optionItem);


        private OptionItemViewModel(OptionItem optionItem)
        {
            OptionItem = optionItem;
            Selected = new ReactiveProperty<OptionParameterViewModel?>();

            InitParameters();
            InitCommands();
        }


        public OptionItem OptionItem { get; }
        public OptionParameterViewModel[] Parameters { get; private set; }

        public IReactiveProperty<OptionParameterViewModel?> Selected { get; }


        private void InitParameters()
        {
            Parameters = OptionItem.Parameters.Select(i => new OptionParameterViewModel(i)).ToArray();
            this.RaisePropertyChanged(nameof(Parameters));
        }

        #region actions

        //public CommandWrapper CmdOpen { get; private set; }

        public CommandWrapper CmdRefresh { get; private set; }
        public CommandWrapper CmdAdd { get; private set; }
        public CommandWrapper CmdDel { get; private set; }

        protected void InitCommands()
        {
            //CmdOpen = CommandWrapper.Create("Open", ActOpenFile);

            CmdRefresh = CommandWrapper.Create("Refresh", InitParameters);
            CmdAdd = CommandWrapper.Create("Add", ActAddItem);

            //CmdDel = ReactiveCommand.Create(ActDel, this.WhenAny(i => i.Selected, i => i.Value != null));
            //CmdDel = ReactiveCommand.Create(ActDel, this.ObservableForProperty(i => i.Selected, sel => sel != null));
            //CmdDel = ReactiveCommand.Create(ActDel, System.Reactive.Linq.Observable. Create(i => i.Selected, i => i.Value != null));
            //var canDel = this.ObservableForProperty(i => i.Selected, sel => sel != null);
            CmdDel = CommandWrapper.Create("Del", ActDel, Selected.Select(i => i != null));
        }

        private void ActAddItem()
        {
            OptionItem.Parameters.Add(new ParameterItem());
            InitParameters();
        }

        private void ActDel()
        {
            if (Selected.Value != null)
            {
                OptionItem.Parameters.Remove(Selected.Value.ParameterItem);
                InitParameters();
            }
        }

        private void ActOpenFile()
        {
            /*
			var filter = new Dictionary<string, List<string>>
			{
				{ "Programs", new List<string> { "exe", "py"} },
				{ "All", new List<string>{ "*"} },
			};
#if AVALONIA
			var dialog = new OpenFileDialog
			{
				//CheckPathExists = true,
				Directory = Path.GetDirectoryName(OptionItem.ProgramPath),
				Filters = filter.Select(i => new FileDialogFilter { Extensions = i.Value, Name = i.Key }).ToList(),
			};
			var files = dialog.ShowAsync(new Window()).Result;
			if (files?.Any() == true)
			{
				OptionItem.ProgramPath = files[0];
				this.RaisePropertyChanged(nameof(OptionItem));
			}
#else
			var dialog = new Microsoft.Win32.OpenFileDialog
			{
				CheckPathExists = true,
				Filter = string.Join("|", filter.Select(i => string.Join("|", i.Value.Select(e => $"*.{e}|*.{e}")))),
			};
			dialog.FileName = OptionItem.ProgramPath;
			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
			{
				OptionItem.ProgramPath = dialog.FileName;
				this.RaisePropertyChanged(nameof(OptionItem));
			}
#endif
			//	*/
        }

        #endregion
    }
}
