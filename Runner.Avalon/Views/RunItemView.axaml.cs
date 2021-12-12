using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
    public partial class RunItemView : Border
	{
		public RunItemView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
