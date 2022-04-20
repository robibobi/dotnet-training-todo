using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApplication.Converters;

namespace TodoApplication.UnitTest.Converters
{
    [TestClass]
    public class StringToUpperConverterTests
    {
        [TestMethod]
        public void Converter_ValueIsAStringWithLowerCaseLetters_StringIsConverterToUpperCase()
        {
            // Arrange
            var converter = CreateSut();
            // Act
            var result = converter.Convert("some string", typeof(string), null, CultureInfo.CurrentCulture);
            // Assert
            result.ShouldBe("SOME STRING");
        }

        [TestMethod]
        public void Converter_ValueIsNull_ReturnsAnEmtpyString()
        {
            // Arrange
            var converter = CreateSut();
            // Act
            var result = converter.Convert(null, typeof(string), null, CultureInfo.CurrentCulture);
            // Assert
            result.ShouldBe("");
        }

        private StringToUpperConverter CreateSut()
        {
            return new StringToUpperConverter();
        }
    }
}
