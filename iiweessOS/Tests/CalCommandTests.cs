using iiweessOS.Commands;
using NUnit.Framework;
using System;
using System.Globalization;

namespace iiweessOS.Tests
{
    [TestFixture]
    public class CalCommandTests
    {
        private CalCommand _calCommand;

        [SetUp]
        public void Setup()
        {
            _calCommand = new CalCommand();
        }

        [Test]
        public void Execute_ShouldReturnCalendarForCurrentMonth_WhenNoArgumentsProvided()
        {
            var now = DateTime.Now;
            var expectedMonthYearText = now.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            var result = _calCommand.Execute(new string[] { });

            Assert.That(result.Contains(expectedMonthYearText));
            Assert.That(result.Contains("Su Mo Tu We Th Fr Sa"));
        }

        [Test]
        public void Execute_ShouldReturnCalendarForSpecifiedMonthAndYear_WhenValidArgumentsProvided()
        {
            var args = new string[] { "12", "2024" };
            var expectedMonthYearText = "December 2024";

            var result = _calCommand.Execute(args);

            Assert.That(result.Contains(expectedMonthYearText));
            Assert.That(result.Contains("Su Mo Tu We Th Fr Sa"));
        }
    }

}
