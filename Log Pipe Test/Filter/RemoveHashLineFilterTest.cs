//using Consortio.Services.LogShipper.Filter;
//using Consortio.Services.LogShipper.Filter.Sharam;
//using Consortio.Services.LogShipper.Input;
//using Consortio.Services.LogShipper.Input.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Filter {
//    [TestClass]
//    public class RemoveHashLineFilterTest {
//        [TestMethod]
//        public void CleanReturnsNullWhenMessageStartsWithHash() {
//            const string msg = "# test";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new RemoveHashLineFilter();
//            var result = filter.Clean(input);
//            Assert.IsNull(result);
//        }

//        [TestMethod]
//        public void CleanReturnsInputWhenMessageStartsWithWhiteSpace() {
//            const string msg = "  # test";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new RemoveHashLineFilter();
//            var result = filter.Clean(input);
//            Assert.IsTrue(result.MessageLine == msg);
//        }

//        [TestMethod]
//        public void CleanReturnsInputWhenMessageDoesNotStartWithHash() {
//            const string msg = "test #";
//            var input = new InputLineInfo(msg, null, null, null);
//            var filter = new RemoveHashLineFilter();
//            var result = filter.Clean(input);
//            Assert.IsTrue(result.MessageLine == msg);
//        }
//    }
//}