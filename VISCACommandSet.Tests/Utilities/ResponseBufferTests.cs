using Xunit;
using VISCACommandSet.Utilities;

namespace VISCACommandSet.Tests.Utilities
{
    public class ResponseBufferTests
    {
        [Fact]
        public void ExtractResponses_ShouldNotReturnIncompleteCommands()
        {
            // Arrange
            var buffer = new ResponseBuffer();
            string fragment = "\x00";

            // Act
            buffer.Add(fragment);
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Empty(responses);
        }

        [Fact]
        public void ExtractResponses_ShouldReturnWholeCommand()
        {
            // Arrange
            var buffer = new ResponseBuffer();

            // Act
            buffer.Add("\x00\xFF");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Single(responses);
            Assert.Equal("\x00\xFF", responses[0]);
        }

        [Fact]
        public void ExtractResponses_ShouldReturnCombinedFragments()
        {
            // Arrange
            var buffer = new ResponseBuffer();

            // Act
            buffer.Add("\x00");
            buffer.Add("\xFF");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Single(responses);
            Assert.Equal("\x00\xFF", responses[0]);
        }

        [Fact]
        public void ExtractResponses_ShouldReturnMultipleCommands()
        {
            // Arrange
            var buffer = new ResponseBuffer();

            // Act
            buffer.Add("\x00\xFF\x00\xFF");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Equal(2, responses.Count);
            Assert.Equal("\x00\xFF", responses[0]);
            Assert.Equal("\x00\xFF", responses[1]);
        }

        [Fact]
        public void ExtractResponses_ShouldReturnMultipleCommandsInMultipleFragments()
        {
            // Arrange
            var buffer = new ResponseBuffer();

            // Act
            buffer.Add("\x00");
            buffer.Add("\xFF");
            buffer.Add("\x11");
            buffer.Add("\xAB");
            buffer.Add("\xFF");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Equal(2, responses.Count);
            Assert.Equal("\x00\xFF", responses[0]);
            Assert.Equal("\x11\xAB\xFF", responses[1]);
        }

        [Fact]
        public void ExtractResponse_ShouldIgnoreIncompleteCommands()
        {
            // Arrange
            var buffer = new ResponseBuffer();

            // Act
            buffer.Add("\x00\xFF\x00");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Single(responses);
            Assert.Equal("\x00\xFF", responses[0]);
        }
    }
}