//using System;
//using Consortio.Services.LogShipper.Filter;
//using Consortio.Services.LogShipper.Filter.Sharam;
//using Consortio.Services.LogShipper.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Filter {
//    [TestClass]
//    public class EventTimeFilterTest {
//        [TestMethod]
//        public void ProcessUpdatesTimestampWhenMessageContainsCorrectFormattedUtcDate() {
//            var now = DateTime.UtcNow;
//            var logDate = DateTime.UtcNow.AddDays(-10).AddYears(-2);
//            var messageDate = new DateTime(logDate.Year, logDate.Month, logDate.Day, logDate.Hour, logDate.Minute, logDate.Second, logDate.Kind).ToUniversalTime();
//            var msg = "test test  " + messageDate.ToString("yyyy-MM-dd HH:mm:ssZ") + "  tes test";
//            var input = new LogEventInfo(msg, "test1", "testsource1", now);
//            var filter = new EventTimeFilter();
//            var result = filter.Process(input);
//            Assert.IsTrue(result.Timestamp == messageDate);
//            Assert.IsTrue(result.Fields.ContainsKey("event_time"));
//        }

//        [TestMethod]
//        public void ProcessUpdatesTimestampWhenMessageContainsCorrectFormattedLocalDate() {
//            var now = DateTime.UtcNow;
//            var logDate = DateTime.Now.AddDays(-10).AddYears(-2);
//            var messageDate = new DateTime(logDate.Year, logDate.Month, logDate.Day, logDate.Hour, logDate.Minute, logDate.Second, logDate.Kind);
//            var msg = "test test  " + messageDate.ToString("yyyy-MM-dd HH:mm:ss") + "  tes test";
//            var input = new LogEventInfo(msg, "test1", "testsource1", now);
//            var filter = new EventTimeFilter();
//            var result = filter.Process(input);
//            Assert.IsTrue(result.Timestamp == messageDate.ToUniversalTime());
//            Assert.IsTrue(result.Fields.ContainsKey("event_time"));
//        }

//        [TestMethod]
//        public void ProcessDoesNotUpdatesTimestampWhenMessageContainsWrongFormattedDate() {
//            var now = DateTime.UtcNow;
//            var logDate = DateTime.UtcNow.AddDays(-10).AddYears(-2);
//            var messageDate = new DateTime(logDate.Year, logDate.Month, logDate.Day, logDate.Hour, logDate.Minute, logDate.Second, logDate.Kind).ToUniversalTime();
//            var msg = "test test  " + messageDate.ToString("yyyy-MM-dd nnn HH:mm:ssZ") + "  tes test";
//            var input = new LogEventInfo(msg, "test1", "testsource1", now);
//            var filter = new EventTimeFilter();
//            var result = filter.Process(input);
//            Assert.IsTrue(result.Timestamp == now);
//            Assert.IsTrue(!result.Fields.ContainsKey("event_time"));
//        }
//    }
//}
