using System;
using TfsCmdlets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TfsCmdlets.UnitTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        private const string SIMPLE_DATA = "foo-bar";
        private const string PATH_DATA = "/foo/bar/baz/xyz";
        private const string PATH_DATA_BS = @"\foo\bar\baz\xyz";

        [TestMethod]
        public void MatchesSimplePattern()
        {
            Assert.IsTrue(SIMPLE_DATA.IsLike("foo*"));
        }

        [TestMethod]
        public void NotMatchesInvalidPattern()
        {
            Assert.IsFalse(SIMPLE_DATA.IsLike("f00*"));
        }

        [TestMethod]
        public void MatchesPatternEqualToData()
        {
            Assert.IsTrue(SIMPLE_DATA.IsLike(SIMPLE_DATA));
        }

        [TestMethod]
        public void NotMatchesIncompleteDataWithoutAsterisk()
        {
            Assert.IsFalse(SIMPLE_DATA.IsLike(SIMPLE_DATA.Substring(0, SIMPLE_DATA.Length-1)));
        }

        [TestMethod]
        public void MatchesFixedDirVariableFile()
        {
            Assert.IsTrue(PATH_DATA.IsLike("/foo/bar/baz/x*"));
        }

        [TestMethod]
        public void MatchesVariableMidDirFixedFile()
        {
            Assert.IsTrue(PATH_DATA.IsLike("/foo/b*/baz/xyz"));
            Assert.IsTrue(PATH_DATA.IsLike("/foo/bar/b*/xyz"));
        }

        [TestMethod]
        public void MatchesVariableRootFixedPath()
        {
            Assert.IsTrue(PATH_DATA.IsLike("/f*/bar/baz/xyz"));
        }

        [TestMethod]
        public void MatchesVariableDeepPathFixedFile()
        {
            Assert.IsTrue(PATH_DATA.IsLike("/foo/**/xyz"));
            Assert.IsTrue(PATH_DATA.IsLike("/**/xyz"));
        }

        [TestMethod]
        public void MatchesVariableDeepPathVariableFile()
        {
            Assert.IsTrue(PATH_DATA.IsLike("/foo/**/*"));
            Assert.IsTrue(PATH_DATA.IsLike("/**/*"));
        }

        [TestMethod]
        public void MatchesFixedDirVariableFileBS()
        {
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\foo\\bar\\baz\\x*"));
        }

        [TestMethod]
        public void MatchesVariableMidDirFixedFileBS()
        {
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\foo\\b*\\baz\\xyz"));
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\foo\\bar\\b*\\xyz"));
        }

        [TestMethod]
        public void MatchesVariableRootFixedPathBS()
        {
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\f*\\bar\\baz\\xyz"));
        }

        [TestMethod]
        public void MatchesVariableDeepPathFixedFileBS()
        {
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\foo\\**\\xyz"));
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\**\\xyz"));
        }

        [TestMethod]
        public void MatchesVariableDeepPathVariableFileBS()
        {
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\foo\\**\\*"));
            Assert.IsTrue(PATH_DATA_BS.IsLike("\\**\\*"));
        }
    }
}
