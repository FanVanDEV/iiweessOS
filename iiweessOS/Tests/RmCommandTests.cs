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
    public class RmCommandTests
    {
        private FileSystemModel fs;
        private RmCommand rmCommand;

        [SetUp]
        public void SetUp()
        {
            fs = new FileSystemModel("/home/user");
            rmCommand = new RmCommand(fs);
        }

        [Test]
        public void Execute_RemovesFile_WhenFileExists()
        {
            fs.SetDirectoryListing("/test/", new List<string> { "file1.txt" });

            string result = rmCommand.Execute(new string[] { "/test/file1.txt" });

            Assert.That("", Is.EqualTo(result));

            var items = fs.ListDirectory("/test/");
            Assert.That(items, Is.Empty); 
        }

        [Test]
        public void Execute_RemovesDirectoryRecursively_WhenRecursiveFlagUsed()
        {
            fs.SetDirectoryListing("/test/", new List<string> { "file1.txt" });
            fs.SetDirectoryListing("/test/dir1/", new List<string> { "file2.txt" });

            string result = rmCommand.Execute(new string[] { "-r", "/test/dir1" });

            Assert.That("", Is.EqualTo(result));

            var items = fs.ListDirectory("/test/");
            Assert.That(!items.Contains("dir1/"));
            Assert.That(items.Contains("file1.txt"));
        }
    }
}
