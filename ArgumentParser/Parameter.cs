namespace ArgumentParser
{
    public class Parameter
    {
        public string LongName { get; }
        public string ShortName { get; }
        public bool HasValue { get; }
        public bool Required { get; }
        public object Value { get; set; }

        public Parameter(string longName, string shortName, bool hasValue, bool required)
        {
            LongName = longName;
            ShortName = shortName;
            HasValue = hasValue;
            Required = required;
        }
    }
}
