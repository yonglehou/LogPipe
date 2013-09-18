//using System;
//using System.Collections.Generic;
//using System.IO;
//using Consortio.Services.LogShipper.Input;
//using Consortio.Services.LogShipper.Input.Sharam;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Consortio.Services.LogShipper.Test.Input {
//    [TestClass]
//    public class SourceFileStateRepositoryTest {
//        private string stateFileFullName;

//        [TestMethod]
//        public void CorrectJsonSerialization() {
//            var sourceFiles = CreateSourceFiles();
//            stateFileFullName = CreateFileFullName("state1.txt");
//            var fileStateRepository = new SourceFileStateRepository(stateFileFullName);
//            fileStateRepository.Save(sourceFiles);
//            var fileStateRepository2 = new SourceFileStateRepository(stateFileFullName);

//            sourceFiles.ForEach(f => Assert.IsNotNull(fileStateRepository2.Get(f.FileInfo.FullName)));
//        }

//        [TestMethod]
//        public void CreateRepositoryWhenFileFullNameIsCorrect() {
//            stateFileFullName = CreateFileFullName("state1.txt");
//            var fileStateRepository = new SourceFileStateRepository(stateFileFullName);
//            Assert.IsNotNull(fileStateRepository);
//        }

//        [TestMethod]
//        [ExpectedException(typeof (ArgumentNullException))]
//        public void ThrowsArgumentNullExceptionWhenFileFullNameIsNull() {
//            const string fileFullName = null;
//            var fileStateRepository = new SourceFileStateRepository(fileFullName);
//            Assert.IsNotNull(fileStateRepository);
//        }

//        [TestMethod]
//        [ExpectedException(typeof (ArgumentException))]
//        public void ThrowsArgumentExceptionWhenFileFullNameIsEmpty() {
//            const string fileFullName = "";
//            var fileStateRepository = new SourceFileStateRepository(fileFullName);
//            Assert.IsNotNull(fileStateRepository);
//        }

//        private string CreateFileFullName(string fileName) {
//            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
//        }

//        private List<SourceFile> CreateSourceFiles() {
//            var lastRead = DateTime.Now;
//            const long lastReadFileSize = 33;
//            var list = new List<SourceFile> {new SourceFile(CreateFileFullName("source1.txt")) , 
//                new SourceFile(CreateFileFullName("source2.txt")) };
//            return list;
//        }

//        [TestCleanup()]
//        public void Cleanup() {
//            try {
//                File.Delete(stateFileFullName);
//            } catch {}
//        }
//    }
//}