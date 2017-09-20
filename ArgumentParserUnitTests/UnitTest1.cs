using ArgumentParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ArgumentParserUnitTests
{
    [TestClass]
    public class MainUnitTests
    {
        [TestMethod]
        public void ShouldParseSimpleQuery()
        {
            var args = new string[] { "--arg1", "blah", "this is a random value", "-b", "bleh", "bleh", "--arg3" };

            var parser = new Parser();

            parser.AddOrderedValue("randovalue");
            parser.AddOrderedValue("anotherrandovalue");
            parser.AddParameter("arg1", true);
            parser.AddParameter("arg2", "b", true);
            parser.AddParameter("arg3", false);

            var dict = parser.Parse(args);

            foreach (var kvp in dict)
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldFailWhenParameterIsValue()
        {
            var args = new string[] { "--arg1", "--fakearg", "-b", "bleh", "--arg3" };

            var parser = new Parser();

            parser.AddParameter("arg1", true);

            parser.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldFailIfArgumentHasNoValue()
        {
            var args = new string[] { "--arg-with-no-value" };
            var parser = new Parser();
            parser.AddParameter("arg-with-no-value", true);
            parser.Parse(args);
        }
    }
}
