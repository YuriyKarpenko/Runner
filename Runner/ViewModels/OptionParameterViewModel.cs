using System;

using ReactiveUI;

using Runner.Models;

namespace Runner.ViewModels
{
    public class OptionParameterViewModel : VmBase
	{
		public static ParameterType[] ParameterTypes { get; } = (ParameterType[])Enum.GetValues(typeof(ParameterType));


		public OptionParameterViewModel(ParameterItem parameterItem)
		{
			ParameterItem = parameterItem;
		}
		

		public ParameterItem ParameterItem { get; }
	}
}
