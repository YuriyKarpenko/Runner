using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
    public partial class OptionParameterView : Border
    {
        public OptionParameterView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
