using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Runner.Views
{
	public partial class OptionItemView : StackPanel
	{
		public OptionItemView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
