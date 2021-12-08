using System.IO;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
    public partial class OptionItemView : Grid
    {
        public OptionItemView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public void OpenFile(object sender, RoutedEventArgs e)
        {
            var pargTextBox = this.FindControl<TextBox>("PathFile");

            var dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Directory = Path.GetDirectoryName(pargTextBox.Text),
                Filters = new System.Collections.Generic.List<FileDialogFilter>
                {
                    new FileDialogFilter { Extensions = {"exe" }, Name = "applications" },
                    new FileDialogFilter { Extensions = {"py" }, Name = "scripts" },
                    new FileDialogFilter { Extensions = {"*" }, Name = "all" },
                },                
            };

            var files = dialog.ShowAsync(new Window()).Result;
            if (files?.Any() == true)
            {
                pargTextBox.Text = files[0];
            }
        }
    }
}
