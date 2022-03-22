using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Marbles;

namespace MarblesTester
{
    [TestClass]
    public class UnitTests
    {
        [DataTestMethod]
        [DataRow("Bob")]
        [DataRow("B 'ob")]
        public void IsPalindrome_GivenValidString_ReturnsTrue(string name)
        {
            // Act
            var result = Marbles.Program.IsPalindrome(name);

            // Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("Bobb")]
        [DataRow("Melonfrost")]
        public void IsPalindrome_GivenNonValidString_ReturnsFalse(string name)
        {
            // Act
            var result = Marbles.Program.IsPalindrome(name);

            // Assert
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("blue")]
        [DataRow("green")]
        [DataRow("yellow")]
        public void ColorComparer_GivenSameColor_ReturnsZero(string x)
        {
            // Arrange
            var colorComparer = new ColorComparer(StringComparer.CurrentCulture);

            // Act
            var result = colorComparer.Compare(x, x);

            // Assert
            Assert.AreEqual(0, result);
        }

        [DataTestMethod]
        [DataRow("red", "orange")]
        [DataRow("orange", "yellow")]
        [DataRow("yellow", "green")]
        [DataRow("green", "blue")]
        [DataRow("blue", "indigo")]
        [DataRow("indigo", "violet")]
        public void ColorComparer_Given2ColorsInOrder_ReturnsNegativeOne(string x, string y)
        {
            // Arrange
            var colorComparer = new ColorComparer(StringComparer.CurrentCulture);

            // Act
            var result = colorComparer.Compare(x, y);

            Assert.AreEqual(-1, result);
        }

        [DataTestMethod]
        [DataRow("red", "orange")]
        [DataRow("orange", "yellow")]
        [DataRow("yellow", "green")]
        [DataRow("green", "blue")]
        [DataRow("blue", "indigo")]
        [DataRow("indigo", "violet")]
        public void ColorComparer_Given2ColorsOutOfOrder_ReturnsOne(string x, string y)
        {
            // Arrange
            var colorComparer = new ColorComparer(StringComparer.CurrentCulture);

            // Act
            var result = colorComparer.Compare(y, x);

            Assert.AreEqual(1, result);
        }
    }
}
