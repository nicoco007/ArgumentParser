using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParser
{
    public class Parser
    {
        private List<Parameter> parameters = new List<Parameter>();
        private List<OrderedValue> orderedValues = new List<OrderedValue>();

        public Parser() { }

        /// <summary>
        /// Add a value that, instead of requiring a value, is used in a certain order.
        /// </summary>
        /// <param name="name">Name of the parameter in the result dictionary.</param>
        public void AddOrderedValue(string name, bool required = false)
        {
            orderedValues.Add(new OrderedValue(name, required));
        }

        /// <summary>
        /// Adds a parameter.
        /// </summary>
        /// <param name="longName">Long name (two dashes)</param>
        /// <param name="shortName">Short name (single dash, usually one letter)</param>
        /// <param name="hasValue">Whether this parameter has a value following it or not</param>
        public void AddParameter(string longName, string shortName = null, bool hasValue = false, bool required = false)
        {
            parameters.Add(new Parameter(longName, shortName, hasValue, required));
        }

        public Dictionary<string, object> Parse(string[] args)
        {
            var totalArgs = args.Length;
            var currentOrderedValue = 0;

            // iterate through all args
            for (var i = 0; i < args.Length; i++)
            {
                var parameter = parameters.FirstOrDefault(param => "--" + param.LongName == args[i] || "-" + param.ShortName == args[i]);

                if (parameter == null && !args[i].StartsWith("-") && currentOrderedValue < orderedValues.Count)
                {
                    var value = orderedValues[currentOrderedValue];
                    value.Value = args[i];
                    currentOrderedValue++;
                }
                else if (parameter != null)
                {
                    if (parameter.HasValue)
                    {
                        if (i + 1 >= args.Length)
                            throw new ArgumentException($"Argument {args[i]} requires a value");

                        i++;
                        var value = args[i];
                        parameter.Value = value;
                    }
                    else
                    {
                        parameter.Value = true;
                    }
                }
                else
                {
                    throw new ArgumentException("Unknown argument: " + args[i]);
                }
            }

            foreach (var parameter in parameters.Where(p => !p.HasValue && p.Value == null))
                parameter.Value = false;

            Parameter requiredParameter;
            OrderedValue requiredValue;

            // make sure all requirements are fullfilled
            if ((requiredParameter = parameters.Find(p => p.Required && p.Value == null)) != null)
                throw new ArgumentException($"Parameter {requiredParameter.LongName} is required.");

            if ((requiredValue = orderedValues.Find(v => v.Required && v.Value == null)) != null)
                throw new ArgumentException($"{requiredValue.Name} is required.");

            // create & fill dictionary
            var dictionary = new Dictionary<string, object>();

            foreach (var parameter in parameters)
                dictionary.Add(parameter.LongName, parameter.Value);

            foreach (var value in orderedValues)
                dictionary.Add(value.Name, value.Value);

            // return parsed values
            return dictionary;
        }
    }
}
