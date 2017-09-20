using ArgumentParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ArgumentParserUnitTests
{
    [TestClass]
    public class ParserUnitTests
    {
        [TestMethod]
        public void ShouldParseValidQuery()
        {
            var args = new string[] { "--arg1", "blah", "this is a random value", "-b", "bleh", "bleh", "--arg3" };

            var parser = new Parser();

            parser.AddOrderedValue("randovalue");
            parser.AddOrderedValue("anotherrandovalue");
            parser.AddParameter("arg1", "a", true);
            parser.AddParameter("arg2", "b", true);
            parser.AddParameter("arg3", "c", false);

            var dict = parser.Parse(args);

            foreach (var kvp in dict)
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
        }

        [TestMethod]
        public void NonPresentNoValueParameterShouldBeFalse()
        {
            var args = new string[] { "--stuff", "and", "--things" };
            var parser = new Parser();

            parser.AddParameter("stuff", null, true);
            parser.AddParameter("things");
            parser.AddParameter("this-should-be-false");

            var dict = parser.Parse(args);
            
            Assert.IsFalse((bool) dict["this-should-be-false"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldFailWhenParameterIsValue()
        {
            var args = new string[] { "--arg1", "-b", "bleh", "--arg3" };

            var parser = new Parser();

            parser.AddParameter("arg1", "a", true);

            parser.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldFailIfArgumentHasNoValue()
        {
            var args = new string[] { "--arg-with-no-value" };
            var parser = new Parser();
            parser.AddParameter("arg-with-no-value", "n", true);
            parser.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldFailIfRequiredArgumentHasNoValue()
        {
            var args = new string[] { "--not-required" };

            var parser = new Parser();

            parser.AddParameter("required", "r", true, true);

            parser.Parse(args);
        }
    }
}
