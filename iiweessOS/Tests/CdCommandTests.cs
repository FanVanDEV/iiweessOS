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
    public class CdCommandTests
    {
        private FileSystemModel _fs;
        private CdCommand _cdCommand;

        [SetUp]
        public void Setup()
        {
            _fs = new FileSystemModel("home/user/");
            _fs.CreateDirectory("/home/user");
            _fs.CreateDirectory("/home/user/testDir");
            _fs.ChangeDirectory("/home/user/");

            _cdCommand = new CdCommand(_fs);
        }

        [Test]
        public void Execute_ShouldChangeDirectory_WhenDirectoryExists()
        {
            var args = new[] { "testDir" };

            var result = _cdCommand.Execute(args);

            Assert.That(result, Is.Empty);
            Assert.That("home/user/testDir/", Is.EqualTo(_fs.GetCurrentDirectory()));
        }

        [Test]
        public void Execute_ShouldReturnErrorMessage_WhenDirectoryDoesNotExist()
        {
            var args = new[] { "nonExistentDir" };

            var result = _cdCommand.Execute(args);

            Assert.That("bash: cd: nonExistentDir: No such file or directory", Is.EqualTo(result));
            Assert.That("home/user/", Is.EqualTo(_fs.GetCurrentDirectory()));
        }
    }
}
