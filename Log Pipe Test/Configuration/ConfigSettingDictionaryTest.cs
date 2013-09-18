//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Consortio.Services.LogShipper.Configuration.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Configuration
//{
//    [TestClass]
//    public class ConfigSettingDictionaryTest
//    {
//        [TestMethod]
//        public void TestMethod1()
//        {
//            var tags = new List<string> { "tags1", "tag2" };
//            const string outputFilePath = @"C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog_{0}.txt";
//            const string inputFilePath = @"C:\Consortio\Test\Logfiles\datetimetestconsole\log.log";
//            const string stateFilePath = @" C:\Consortio\Test\Logfiles\datetimetestconsole\state.txt";
//            const string inputType = "testFileInput";
//            const string readFromBiginingFalseString = "false";
//            const string readFromBiginingTrueString = "true";

//            var inputFiles = new List<string> {
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType+"1", tags, readFromBiginingFalseString), 
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType+"2", tags, readFromBiginingTrueString), 
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType+"3", null, null)};
//            var outFiles = new List<string> {
//              XmlFileHelper.CreateOutputFileString(string.Format(outputFilePath,"1")), 
//            XmlFileHelper.CreateOutputFileString(string.Format(outputFilePath,"2")), 
//              XmlFileHelper.CreateOutputFileString(string.Format(outputFilePath,"4"),inputType+"3"), 
//              XmlFileHelper.CreateOutputFileString(string.Format(outputFilePath,"3"))};
//            var document = XmlFileHelper.CreateDoc(inputFiles, outFiles);
//            var configXmlReader = new ConfigXmlReader(document);
//            var configSettingDictionary = new ConfigSettingDictionary(configXmlReader);
//            var dic = configSettingDictionary.CreateDictionary();
//           var typelessOutd= configSettingDictionary.GetTypelessOutPuts();
          
//           Assert.IsTrue(dic.Count == 3);
//           Assert.IsTrue(typelessOutd.Count == 3);
//        }
//        [TestMethod]
//        public void TestMethod2()
//        {
           
//            var document = XmlFileHelper.CreateStaticDoc();
//            var configXmlReader = new ConfigXmlReader(document);
//            var configSettingDictionary = new ConfigSettingDictionary(configXmlReader);
//            var dic = configSettingDictionary.CreateDictionary();
//            var typelessOutd = configSettingDictionary.GetTypelessOutPuts();

//            Assert.IsTrue(dic.Count == 3);
//            Assert.IsTrue(typelessOutd.Count == 1);
//        }
//    }
//}
