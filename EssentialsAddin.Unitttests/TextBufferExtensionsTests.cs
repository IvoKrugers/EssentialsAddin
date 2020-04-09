using System;
using Gtk;
using NUnit.Framework;
namespace EssentialsAddin.Unitttests.TextBufferExtensionsTests
{
    public class Tests
    {
        [TestFixture]
        public class GetTextFromBuffer
		{
            [Test]
            public void TestOne()
            {
                // Arrange

                var buffer = new TextBuffer(new TextTagTable());

                //EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(buffer, new TextTag("Ivo"));

                // Act


                // Assert

                Assert.IsTrue(false);
            }

		}
    }
}
