using iiweessOS.Commands;
using iiweessOS.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Tests
{
    [TestFixture]
    public class LsCommandTests
    {
        [Test]
        public void Execute_ShouldReturnFormattedItems_WhenDirectoryExists()
        {
            var fileSystemMock = new FileSystemModel("home/user/");
            var command = new LsCommand(fileSystemMock);
            var expectedItems = new List<string> { "file1.txt", "script.sh" };

            fileSystemMock.CreateDirectory("directory");
            fileSystemMock.SetDirectoryListing(".", expectedItems);

            string result = command.Execute(new string[0]);

            Assert.That("directory  file1.txt  script.sh*", Is.EqualTo(result));
        }

        [Test]
        public void Execute_ShouldReturnErrorMessage_WhenDirectoryDoesNotExist()
        {
            var fileSystemMock = new FileSystemModel("home/user/");
            var command = new LsCommand(fileSystemMock);
            string nonexistentDir = "nonexistent";

            string result = command.Execute(new[] { nonexistentDir });

            Assert.That($"ls: cannot access '{nonexistentDir}': No such file or directory", Is.EqualTo(result));
        }
    }
}
