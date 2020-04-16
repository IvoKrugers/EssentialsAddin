using System;
using Gtk;
using NUnit.Framework;

namespace TextBufferExtensions
{
    public class Tests
    {
        [TestFixture]
        public class GetTextFromBuffer
		{

            TextTag _debugTag;
            TextTag _indentTag;
            TextTagTable _tagTable;
            TextBuffer _buffer;

            [SetUp]
            public void Setup()
            {
                _debugTag = new TextTag("debug");
                _indentTag = new TextTag("indent") {  Indent = 4};
                _tagTable = new TextTagTable();
                _tagTable.Add(_debugTag);
                _tagTable.Add(_indentTag);
                _buffer = new TextBuffer(_tagTable);
            }

            [Test]
            public void TestBufferWithDebugLineAtBeginning()
            {
                // Arrange
                var iter = _buffer.StartIter;
                _buffer.InsertWithTags(ref iter, $"1. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"2. normal line");
                _buffer.InsertWithTags(ref iter, $"3. normal line");

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual( $"1. Debug line.\n", text);
            }

            [Test]
            public void TestBufferWithDebugLineAtEnd()
            {
                // Arrange
                var iter = _buffer.StartIter;
                
                _buffer.InsertWithTags(ref iter, $"1. normal line");
                _buffer.InsertWithTags(ref iter, $"2. normal line");
                _buffer.InsertWithTags(ref iter, $"3. Debug line.", _debugTag);

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual( $"3. Debug line.\n", text);
            }

            [Test]
            public void TestBufferWithDebugLineInTheMiddle()
            {
                // Arrange
                var iter = _buffer.StartIter;

                _buffer.InsertWithTags(ref iter, $"1. normal line");
                _buffer.InsertWithTags(ref iter, $"2. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"3. normal line");

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual( $"2. Debug line.\n", text);
            }

            [Test]
            public void TestBufferWithDebugLineInTheMiddleAndSurroundingLinesWithIndentTag()
            {
                // Arrange
                var iter = _buffer.StartIter;

                _buffer.InsertWithTags(ref iter, $"1. normal line",_indentTag);
                _buffer.InsertWithTags(ref iter, $"2. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"3. normal line", _indentTag);

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual( $"2. Debug line.\n", text);
            }

            [Test]
            public void TestBufferWithDebugLinesOnly()
            {
                // Arrange
                var iter = _buffer.StartIter;

                _buffer.InsertWithTags(ref iter, $"1. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"2. Debug line.", _debugTag);

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual($"1. Debug line.2. Debug line.\n", text);
            }

            [Test]
            public void TestBufferWithDebugLinesInTheMiddleAndSurroundingLinesWithIndentTag()
            {
                // Arrange
                var iter = _buffer.StartIter;

                _buffer.InsertWithTags(ref iter, $"1. normal line", _indentTag);
                _buffer.InsertWithTags(ref iter, $"2. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"3. Debug line.", _debugTag);
                _buffer.InsertWithTags(ref iter, $"4. normal line", _indentTag);

                // Act
                var text = EssentialsAddin.Lib.TextBufferExtensions.GetTextFromBuffer(_buffer, _debugTag);

                // Assert
                Assert.AreEqual($"2. Debug line.3. Debug line.\n",text);
            }
        }
    }
}
