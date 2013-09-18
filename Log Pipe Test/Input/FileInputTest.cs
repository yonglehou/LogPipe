//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Text;
//using System.Threading;
//using Consortio.Services.LogShipper.Input;
//using Consortio.Services.LogShipper.Input.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Input {
//    [TestClass]
//    public class FileInputTest {
//        [TestMethod]
//        public void ReadsLogsCorrectly() {
//            var tempList = new List<InputLineInfo>();
//            var sourcePath = Path.GetFullPath("source1.txt");
//            var stateFilePath = Path.GetFullPath("state1.txt");
//            File.WriteAllLines(sourcePath, new List<string> {"test line 1"}, Encoding.UTF8);
//            var input = new FileInput("test", sourcePath, stateFilePath, new List<string> {"tag1"});
//            Thread.Sleep(1000);
//            File.AppendAllLines(sourcePath, new List<string> {"test line 2"}, Encoding.UTF8);
//            Thread.Sleep(1000);
//            for (int i = 3; i < 13; i++)
//                File.AppendAllLines(sourcePath, new List<string> {"test line " + i.ToString(CultureInfo.InvariantCulture)}, Encoding.UTF8);
//            Thread.Sleep(5000);
//            InputLineInfo inputLine = null;
//            while ((inputLine = input.GetOne()) != null) {
//                tempList.Add(inputLine);
//            }
//            Assert.IsTrue(tempList.Count == 12);
//        }
//    }
//}