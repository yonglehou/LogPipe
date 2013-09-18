//using Consortio.Services.LogShipper.Filter;
//using Consortio.Services.LogShipper.Filter.Sharam;
//using Consortio.Services.LogShipper.Input;
//using Consortio.Services.LogShipper.Input.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Filter {
//    [TestClass]
//    public class MultiLineFilterTest {
//        [TestMethod]
//        public void ComposeEventReturnsLogEventInfoWhenFilterIsEmptyAndMessageDoesNotMatch() {
//            const string matchPattern = "newEvent|new event";
//            const string msg = "test ";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new MultiLineFilterS(matchPattern);
//            var result = filter.ComposeEvent(input);
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void ComposeEventReturnsLogEventInfoWhenFilterIsNotEmptyAndMessageMatchs() {
//            const string matchPattern = "newEvent|new event";
//            const string msg = "newEvent ";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new MultiLineFilterS(matchPattern);
//            filter.ComposeEvent(input);
//            var result = filter.ComposeEvent(input);
//            Assert.IsNotNull(result);
//        }

//        [TestMethod]
//        public void ComposeEventReturnsNullWhenFilterIsNotEmptyAndMessageDoesNotMatch() {
//            const string matchPattern = "newEvent|new event";
//            const string msg = "test ";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new MultiLineFilterS(matchPattern);
//            filter.ComposeEvent(new InputLineInfo("newEvent", null, null, null));
//            var result = filter.ComposeEvent(input);
//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public void ComposeEventReturnsNullWhenInputIsNull() {
//            const string matchPattern = "newEvent|new event";
//            var filter = new MultiLineFilterS(matchPattern);
//            var result = filter.ComposeEvent(null);
//            Assert.IsNull(result);
//        }
//    }
//}