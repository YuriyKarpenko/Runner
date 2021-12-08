using System.Collections.Generic;

namespace Runner.Models
{
    public class Options
    {
        public List<OptionItem> Items { get; }

        public Options()
        {
            Items = new List<OptionItem>();
        }
    }
}