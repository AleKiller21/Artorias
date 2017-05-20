using System;
using LexerAnalyser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxAnalyser;
using SyntaxAnalyser.Exceptions;
using SyntaxAnalyser.Parser;

namespace ParserUnitTests
{
    [TestClass]
    public class ParserTests
    {
        string path = "C:\\Users\\alefe\\Documents\\Code\\C#\\Compiler\\Artorias\\ParserUnitTests\\TestFiles\\";

        [TestMethod]
        public void Using_Namespaces_KeywordsSuccess()
        {
            var stream = new FileInputStream(path + "using_keywords_and_namespaces.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            try
            {
                parser.Parse();
                Assert.AreEqual("success", "success");
            }
            catch (ParserException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Using_Namespaces_KeywordsMissingEndStatement()
        {
            var stream = new FileInputStream(path + "using_namespace_keywords_missing_endstatement.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<EndOfStatementException>(() => parser.Parse());
        }

        [TestMethod]
        public void Using_Namespaces_KeywordsMissingCurlyBraceClosed()
        {
            var stream = new FileInputStream(path + "using_namespace_keywords_missing_curlybraceclosed.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<MissingCurlyBraceClosedException>(() => parser.Parse());
        }

        [TestMethod]
        public void Using_Namespaces_KeywordsMissingIdentifier()
        {
            var stream = new FileInputStream(path + "using_namespace_keywords_missing_identifier.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<IdTokenExpectecException>(() => parser.Parse());
        }
    }
}
