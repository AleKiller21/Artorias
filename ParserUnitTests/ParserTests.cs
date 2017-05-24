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

        [TestMethod]
        public void Interfaces_working()
        {
            var stream = new FileInputStream(path + "interfaces_working.cs");
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
        public void interface_missing_id_after_comma_inheritance()
        {
            var stream = new FileInputStream(path + "interface_missing_id_after_comma_inheritance.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<IdTokenExpectecException>(() => parser.Parse());
        }

        [TestMethod]
        public void interface_missing_type_in_body()
        {
            var stream = new FileInputStream(path + "interface_missing_type_in_body.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<IdTokenExpectecException>(() => parser.Parse());
        }

        [TestMethod]
        public void interface_missing_interface_keyword_in_namespace()
        {
            var stream = new FileInputStream(path + "interface_missing_interface_keyword_in_namespace.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<MissingCurlyBraceClosedException>(() => parser.Parse());
        }

        [TestMethod]
        public void interface_missing_curly_brace_closed_after_interface_body()
        {
            var stream = new FileInputStream(path + "interface_missing_curly_brace_closed_after_interface_body.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<MissingCurlyBraceClosedException>(() => parser.Parse());
        }

        [TestMethod]
        public void interface_missing_end_statement_after_member()
        {
            var stream = new FileInputStream(path + "interface_missing_end_statement_after_member.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<EndOfStatementException>(() => parser.Parse());
        }

        [TestMethod]
        public void enums_working()
        {
            var stream = new FileInputStream(path + "enums_working.cs");
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
        public void enums_missing_identifier_after_enum_keyword()
        {
            var stream = new FileInputStream(path + "enums_missing_identifier_after_enum_keyword.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<IdTokenExpectecException>(() => parser.Parse());
        }

        [TestMethod]
        public void enums_missing_open_curly_brace()
        {
            var stream = new FileInputStream(path + "enums_missing_open_curly_brace.cs");
            var lexer = new Lexer(stream);
            var parser = new Parser(lexer);
            Assert.ThrowsException<MissingCurlyBraceOpenException>(() => parser.Parse());
        }

        [TestMethod]
        public void enum_trailing_comma_after_members()
        {
            //It works
            var stream = new FileInputStream(path + "enum_trailing_comma_after_members.cs");
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
        public void UnityWorking()
        {
            //It works
            var stream = new FileInputStream(path + "Unity_working.cs");
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
        public void ProgramWorking()
        {
            //It works
            var stream = new FileInputStream(path + "program_working.cs");
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
        public void RevisionParser()
        {
            //It works
            var stream = new FileInputStream(path + "Ejemplo_Parse.cs");
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
    }
}