
using ReactiveUI;

namespace Runner.ViewModels
{
    public class VmBase : ReactiveObject
	{
		protected void Log(string message, bool isError = true)
		{
			if (message != null)
			{
				//LogItems.Add(new M.LogItem(message, isError ? Brushes.Red : Brushes.Gray));
				//OnPropertyChanged("Logs");
			}
		}
	}
}