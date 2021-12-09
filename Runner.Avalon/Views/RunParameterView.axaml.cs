using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
    public partial class RunParameterView : Border
    {
        public RunParameterView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
