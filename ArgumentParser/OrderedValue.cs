namespace ArgumentParser
{
    public class OrderedValue
    {
        public string Name { get; }
        public bool Required { get; }
        public object Value { get; set; }

        public OrderedValue(string name, bool required)
        {
            Name = name;
            Required = required;
        }
    }
}
