using System.Collections.Generic;

namespace Runner.Models
{
	public class Options
	{
		public List<OptionItem> OptionItems { get; set; }

		public Options()
		{
			OptionItems = new List<OptionItem>();
		}
	}
}
