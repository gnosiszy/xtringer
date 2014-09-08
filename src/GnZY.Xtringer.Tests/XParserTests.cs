using System;
using FluentAssertions;
using NUnit.Framework;

namespace GnZY.Xtringer.Tests
{
    [TestFixture]
    public class XParserTests
    {
        [Test]
        public void Encode_EmptyString()
        {
            new XParser().Parse("").Should().Be("");
        }

        [Test]
        public void Encode_OneParameter()
        {
            dynamic parser = new XParser();

            parser.name = "teste";

            string s = parser.Parse("{name}");

            s.Should().Be("teste");
        }

        [Test]
        public void Encode_SameParameter()
        {
            dynamic parser = new XParser();

            parser.name = "teste";

            string s = parser.Parse("{name} {name}");

            s.Should().Be("teste teste");
        }

        [Test]
        public void Encode_TwoParameters()
        {
            dynamic parser = new XParser();

            parser.name = "teste";
            parser.last_name = "testado";

            string s = parser.Parse("{name} {last_name}");

            s.Should().Be("teste testado");
        }

        [Test]
        public void Encode_FormatNumber()
        {
            dynamic parser = new XParser();

            parser.color = 0xAABBCCDD;

            string s = parser.Parse("testando {color:X8}");

            s.Should().Be("testando AABBCCDD");
        }

        [Test]
        public void Encode_FormatDateTime()
        {
            dynamic parser = new XParser();

            parser.some_date = new DateTime(2014, 10, 6);

            string s = parser.Parse("testando {some_date:yyyy/dd}");

            s.Should().Be("testando 2014/06");
        }

        [Test]
        public void Encode_FormatEscapeLeftBrace()
        {
            dynamic parser = new XParser();

            parser.name = "teste";

            string s = parser.Parse(@"testando {\{} {name}");
            s.Should().Be("testando { teste");
        }

        [Test]
        public void Encode_FormatEscapeRightBrace()
        {
            dynamic parser = new XParser();

            parser.name = "teste";

            string s = parser.Parse(@"testando {\} {name}");
            s.Should().Be("testando } teste");
        }

        [Test]
        public void Encode_FormatEscapeAll()
        {
            dynamic parser = new XParser();

            parser.name = "teste";

            string s = parser.Parse(@"testando {\{} {\} {name}");
            s.Should().Be("testando { } teste");
        }
    }
}
