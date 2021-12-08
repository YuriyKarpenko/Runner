using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
    public partial class RunView : Border
    {
        public RunView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
