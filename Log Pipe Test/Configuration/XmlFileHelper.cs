//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Consortio.Services.LogShipper.Test.Configuration {
//    public class XmlFileHelper {
//        public static string CreateInputFileString(string path, string sincedb, string inputType, List<string> tags, string readFromBegining) {
//            return string.Format(" <file><type>{0}</type><path>{1}</path><sincedbpath>{2}</sincedbpath>{3}{4}</file>", inputType, path, sincedb, GetTagsString(tags), GetReadFromBiginingString(readFromBegining));
//        }

//        public static string CreateOutputFileString(string path) {
//            return string.Format(" <file><path>{0}</path></file>", path);
//        }

//        public static string CreateOutputFileString(string path, string type) {
//            return string.Format(" <file><type>{0}</type><path>{1}</path></file>", type, path);
//        }

//        public static string GetReadFromBiginingString(string value) {
//            if (value == null)
//                return null;
//            return string.Format("<readFromBigining>{0}</readFromBigining>", value);
//        }

//        public static string GetTagsString(List<string> tags) {
//            if (tags == null)
//                return null;
//            var sb = new StringBuilder("<tags>");
//            tags.ForEach(t => sb.AppendFormat("<tag>{0}</tag>", t));
//            sb.Append(" </tags>");
//            return sb.ToString();
//        }

//        public static XDocument CreateDoc(List<string> inputFiles, List<string> outputStrings) {
//            const string xmlString = @"<?xml version=""1.0"" encoding=""utf-8""?><config><input>{0}</input><output>{1}</output></config>";
//            var inputs = new StringBuilder();
//            inputFiles.ForEach(t => inputs.AppendLine(t));
//            var outputs = new StringBuilder();
//            outputStrings.ForEach(t => outputs.AppendLine(t));
//            return XDocument.Parse(string.Format(xmlString, inputs, outputs));
//        }

//        public static XDocument CreateStaticDoc() {
//            const string xmlString = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<config>
//  <input>
//    <file >
//      <type>input_type_1
//      </type>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\log_1.log
//      </path>
//      <sincedbpath>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\state_1.txt
//      </sincedbpath>
//      <tags>
//        <tag>tag1</tag>
//        <tag>tag2</tag>
//      </tags>
//      <readFromBigining>false</readFromBigining>
//    </file>
//
// <file >
//      <type>input_type_2
//      </type>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\log_2.log
//      </path>
//      <sincedbpath>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\state_2.txt
//      </sincedbpath>
//      <tags>
//        <tag>tag1</tag>
//        <tag>tag2</tag>
//      </tags>
//      <readFromBigining>false</readFromBigining>
//    </file>
//
// <file >
//      <type>input_type_3
//      </type>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\log_3.log
//      </path>
//      <sincedbpath>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\state_3.txt
//      </sincedbpath>
//      <tags>
//        <tag>tag1</tag>
//        <tag>tag2</tag>
//      </tags>
//      <readFromBigining>false</readFromBigining>
//    </file>
//  </input>
//  <filter>
//    
//  </filter>
//  <output>
//    <file>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog.txt
//      </path>
//     
//    </file>
// <file>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog_1.txt
//      </path>
//      <type>
//        input_type_1
//      </type>
//    </file>
// <file>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog_2.txt
//      </path>
//      <type>
//        input_type_2
//      </type>
//    </file>
// <file>
//      <path>
//        C:\Consortio\Test\Logfiles\datetimetestconsole\outputlog_3.txt
//      </path>
//      <type>
//        input_type_3
//      </type>
//    </file>
//  </output>
//</config>";

//            return XDocument.Parse(xmlString);
//        }
//    }
//}