namespace Runner.Models
{
    public class ParameterItem
    {
        public ParameterType ParameterType { get; set; }
        public string? Value { get; set; }
        public bool IsValid => !string.IsNullOrEmpty(Value);
    }
}
