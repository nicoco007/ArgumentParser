namespace ArgumentParser
{
    public class Parameter
    {
        public string LongName { get; }
        public string ShortName { get; }
        public bool HasValue { get; }

        public Parameter(string longName, string shortName, bool hasValue)
        {
            LongName = longName;
            ShortName = shortName;
            HasValue = hasValue;
        }
    }
}
