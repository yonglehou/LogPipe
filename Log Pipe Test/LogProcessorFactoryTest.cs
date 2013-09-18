//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Consortio.Services.LogShipper.Configuration.Sharam;
//using Consortio.Services.LogShipper.Sharam;
//using Consortio.Services.LogShipper.Test.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test {
//    [TestClass]
//    public class LogProcessorFactoryTest {
//        [TestMethod]
//        public void TestMethod2() {
//            var document = XmlFileHelper.CreateStaticDoc();
//            var configXmlReader = new ConfigXmlReader(document);
//            var configSettingDictionary = new ConfigSettingDictionary(configXmlReader);
//            var dic = configSettingDictionary.CreateDictionary();
//            var typelessOutd = configSettingDictionary.GetTypelessOutPuts();
//            var logProcessorFactory = new LogProcessorFactory(configSettingDictionary);
//            var logs = logProcessorFactory.Create();
//            Assert.IsTrue(dic.Count == logs.Count);
//        }
//    }
//}