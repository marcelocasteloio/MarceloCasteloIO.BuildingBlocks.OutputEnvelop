using FluentAssertions;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace UnitTests;

public class OutputMessageTests
{
    [Fact]
    public void OutputMessage_Should_Created()
    {
        // Arrange
        var outputMessageTypeCollection = Enum.GetValues<OutputMessageType>();
        var code = new string('A', 50);
        var descriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = outputMessageTypeCollection.Length * descriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);
        var createdOutputMessageCollection = new List<(OutputMessage OutputMessage, OutputMessageType Type, string Code, string? Description)>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var outputMessageType in outputMessageTypeCollection)
            foreach (var description in descriptionCollection)
                createActionCollection.Add(() => {
                    var outputMessage = OutputMessage.Create(outputMessageType, code, description);
                    createdOutputMessageCollection.Add(
                        (OutputMessage: outputMessage, Type: outputMessageType, Code: code, Description: description)
                    );
                });

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().NotThrow();
        
        foreach (var createdOutputMessage in createdOutputMessageCollection)
        {
            createdOutputMessage.OutputMessage.Type.Should().Be(createdOutputMessage.Type);
            createdOutputMessage.OutputMessage.Code.Should().Be(createdOutputMessage.Code);
            createdOutputMessage.OutputMessage.Description.Should().Be(createdOutputMessage.Description);
        }
    }

    [Fact]
    public void OutputMessage_Should_Not_Created()
    {
        // Arrange
        var outputMessageTypeCollection = Enum.GetValues<OutputMessageType>();
        var codeCollection = new[]
        {
            string.Empty,
            null,
            " ",
            "  ",
        };
        var descriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = outputMessageTypeCollection.Length * codeCollection.Length * descriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var outputMessageType in outputMessageTypeCollection)
            foreach (var code in codeCollection)
                foreach (var description in descriptionCollection)
                    createActionCollection.Add(() => OutputMessage.Create(outputMessageType, code!, description));

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().Throw<Exception>();
    }
}