//using System.Collections.Generic;
//using System.Text;
//using System.Xml.Linq;
//using Consortio.Services.LogShipper.Configuration.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Configuration {
//    [TestClass]
//    public class ConfigXmlReaderTest {
//        [TestMethod]
//        public void GetInputFileSettingsReturnCorrectCountOfInputFileSettings()
//        {
//            var tags = new List<string> {"tags1", "tag2"};
//            const string outputFilePath = @"C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog.txt";
//            const string inputFilePath = @"C:\Consortio\Test\Logfiles\datetimetestconsole\log.log";
//            const string stateFilePath = @" C:\Consortio\Test\Logfiles\datetimetestconsole\state.txt";
//            const string inputType = "testFileInput";
//            const string readFromBiginingFalseString = "false";
//            const string readFromBiginingTrueString = "true";

//            var inputFiles = new List<string> {
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType, tags, readFromBiginingFalseString), 
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType, tags, readFromBiginingTrueString), 
//                XmlFileHelper.CreateInputFileString(inputFilePath, stateFilePath, inputType, null, null)};
//            var outFiles = new List<string> {
//              XmlFileHelper.CreateOutputFileString(outputFilePath) };
//            var document = XmlFileHelper.CreateDoc(inputFiles, outFiles);
//            var configXmlReader = new ConfigXmlReader(document);
//            var inputs = configXmlReader.GetInputFileSettings();
//            var outputs = configXmlReader.GetOutputFileSettings();
//            Assert.IsTrue(inputs.Count == 3);
//            Assert.IsTrue(outputs.Count == 1);
//        }

      
//    }
//}