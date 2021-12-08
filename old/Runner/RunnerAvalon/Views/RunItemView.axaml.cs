using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
	public partial class RunItemView : UserControl
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
