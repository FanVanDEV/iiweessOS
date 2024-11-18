using iiweessOS.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iiweessOS.Tests
{
    [TestFixture]
    public class DateCommandTests
    {
        [Test]
        public void Execute_ShouldReturnDateInDefaultFormat_WhenNoArgumentsProvided()
        {
            var command = new DateCommand();
            string defaultFormat = @"\w{3} \w{3} \d{1,2} \d{1,2}:\d{2}:\d{2} \d{4}";

            string result = command.Execute(new string[0]);

            Assert.That(Regex.IsMatch(result, defaultFormat), "The date format does not match the expected default format.");
        }

        [Test]
        public void Execute_ShouldReturnDateInCustomFormat_WhenFormatArgumentProvided()
        {
            var command = new DateCommand();
            string customFormat = "yyyy-MM-dd HH:mm";
            string[] args = { $@"+""{customFormat}""" };

            string result = command.Execute(args);
            string expectedFormat = @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}";

            Assert.That(Regex.IsMatch(result, expectedFormat), "The date format does not match the expected custom format.");
        }
    }
}
