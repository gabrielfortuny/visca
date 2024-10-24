using Xunit;
using VISCACommandSet.Utilities;

namespace VISCACommandSet.Tests.Utilities
{
    public class ResponseBufferTests
    {

        // Helper class to capture events for assertion
        private class EventCapture
        {
            public List<string> CapturedResponses { get; } = new List<string>();

            // Event handler to capture the event data
            public void OnResponseReceived(object? sender, DataEventArgs e)
            {
                CapturedResponses.Add(e.CompleteMessage);
            }
        }

        [Fact]
        public void ExtractResponses_ShouldNotReturnIncompleteCommands()
        {
            // Arrange
            var buffer = new ResponseBuffer('\xFF', 100);
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
            var buffer = new ResponseBuffer('\xFF', 100);

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
            var buffer = new ResponseBuffer('\xFF', 100);

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
            var buffer = new ResponseBuffer('\xFF', 100);

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
            var buffer = new ResponseBuffer('\xFF', 100);

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
            var buffer = new ResponseBuffer('\xFF', 100);

            // Act
            buffer.Add("\x00\xFF\x00");
            var responses = buffer.ExtractResponses();

            // Assert
            Assert.Single(responses);
            Assert.Equal("\x00\xFF", responses[0]);
        }
    }
}