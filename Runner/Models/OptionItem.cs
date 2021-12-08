using System.Collections.Generic;

namespace Runner.Models
{
    public class OptionItem
    {
        public string? Description { get; set; }
        public string? ProgramPath { get; set; }
        public List<ParameterItem> Parameters { get; }


        public OptionItem()
        {
            Parameters = new List<ParameterItem>();
        }
    }
}