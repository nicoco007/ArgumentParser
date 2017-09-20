using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    public class Parser
    {
        private List<Parameter> parameters = new List<Parameter>();
        private List<string> orderedValues = new List<string>();

        public Parser() { }

        public void AddOrderedValue(string name)
        {
            orderedValues.Add(name);
        }

        public void AddParameter(string longName, bool hasValue)
        {
            parameters.Add(new Parameter(longName, null, hasValue));
        }

        public void AddParameter(string longName, string shortName, bool hasValue)
        {
            parameters.Add(new Parameter(longName, shortName, hasValue));
        }

        public Dictionary<string, object> Parse(string[] args)
        {
            var values = new Dictionary<string, object>();
            var totalArgs = args.Length;
            var totalOrderedValues = orderedValues.Count;
            var orderedValuesCount = 0;

            for (var i = 0; i < args.Length; i++)
            {
                var parameter = parameters.FirstOrDefault(param => "--" + param.LongName == args[i] || "-" + param.ShortName == args[i]);

                if (parameter == null)
                {
                    if (orderedValuesCount < totalOrderedValues)
                    {
                        values.Add(orderedValues[orderedValuesCount], args[i]);
                        orderedValuesCount++;
                    }
                    else
                    {
                        throw new ArgumentException("Unknown argument: " + args[i]);
                    }
                }
                else
                {
                    if (parameter.HasValue)
                    {
                        if (i + 1 >= args.Length)
                            throw new ArgumentException($"Argument {args[i]} requires a value");

                        i++;
                        var value = args[i];
                        values.Add(parameter.LongName, value);
                    }
                    else
                    {
                        values.Add(parameter.LongName, true);
                    }
                }
            }

            return values;
        }
    }
}
