using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StuffSender.Utilities;
using System.IO;

namespace StuffSender.Test
{
    [TestClass]
    public class TodoistTest
    {
        // Make a Rebuild before running this test
        string FilePath = Directory.GetCurrentDirectory() + @"\image.jpg";


        [TestMethod]
        public void ICanUploadSomething()
        {
            var todoist = new Todoist();
            var response = todoist.UploadFile(FilePath);

            Assert.AreEqual(Path.GetFileName(FilePath), response.file_name);


        }

    }
}
