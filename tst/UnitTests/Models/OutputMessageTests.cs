using FluentAssertions;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace UnitTests.Models;

public class OutputMessageTests
{
    [Fact]
    public void OutputMessage_Should_Created()
    {
        // Arrange
        var expectedOutputMessageTypeCollection = Enum.GetValues<OutputMessageType>();
        var expectedCode = new string('A', 50);
        var expectedDescriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = expectedOutputMessageTypeCollection.Length * expectedDescriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);
        var createdOutputMessageCollection = new List<(OutputMessage OutputMessage, OutputMessageType Type, string Code, string? Description)>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var outputMessageType in expectedOutputMessageTypeCollection)
            foreach (var description in expectedDescriptionCollection)
                createActionCollection.Add(() =>
                {
                    var outputMessage = OutputMessage.Create(outputMessageType, expectedCode, description);
                    createdOutputMessageCollection.Add(
                        (OutputMessage: outputMessage, Type: outputMessageType, Code: expectedCode, Description: description)
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
    public void OutputMessage_Should_Created_Information()
    {
        // Arrange
        var expectedOutputMessageType = OutputMessageType.Information;
        var code = new string('A', 50);
        var descriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = descriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);
        var createdOutputMessageCollection = new List<(OutputMessage OutputMessage, string Code, string? Description)>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var description in descriptionCollection)
            createActionCollection.Add(() =>
            {
                var outputMessage = OutputMessage.CreateInformation(code, description);
                createdOutputMessageCollection.Add(
                    (OutputMessage: outputMessage, Code: code, Description: description)
                );
            });

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().NotThrow();

        foreach (var createdOutputMessage in createdOutputMessageCollection)
        {
            createdOutputMessage.OutputMessage.Type.Should().Be(expectedOutputMessageType);
            createdOutputMessage.OutputMessage.Code.Should().Be(createdOutputMessage.Code);
            createdOutputMessage.OutputMessage.Description.Should().Be(createdOutputMessage.Description);
        }
    }

    [Fact]
    public void OutputMessage_Should_Created_Warning()
    {
        // Arrange
        var expectedOutputMessageType = OutputMessageType.Warning;
        var code = new string('A', 50);
        var descriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = descriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);
        var createdOutputMessageCollection = new List<(OutputMessage OutputMessage, string Code, string? Description)>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var description in descriptionCollection)
            createActionCollection.Add(() =>
            {
                var outputMessage = OutputMessage.CreateWarning(code, description);
                createdOutputMessageCollection.Add(
                    (OutputMessage: outputMessage, Code: code, Description: description)
                );
            });

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().NotThrow();

        foreach (var createdOutputMessage in createdOutputMessageCollection)
        {
            createdOutputMessage.OutputMessage.Type.Should().Be(expectedOutputMessageType);
            createdOutputMessage.OutputMessage.Code.Should().Be(createdOutputMessage.Code);
            createdOutputMessage.OutputMessage.Description.Should().Be(createdOutputMessage.Description);
        }
    }
    
    [Fact]
    public void OutputMessage_Should_Created_Error()
    {
        // Arrange
        var expectedOutputMessageType = OutputMessageType.Error;
        var code = new string('A', 50);
        var descriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = descriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);
        var createdOutputMessageCollection = new List<(OutputMessage OutputMessage, string Code, string? Description)>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var description in descriptionCollection)
            createActionCollection.Add(() =>
            {
                var outputMessage = OutputMessage.CreateError(code, description);
                createdOutputMessageCollection.Add(
                    (OutputMessage: outputMessage, Code: code, Description: description)
                );
            });

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().NotThrow();

        foreach (var createdOutputMessage in createdOutputMessageCollection)
        {
            createdOutputMessage.OutputMessage.Type.Should().Be(expectedOutputMessageType);
            createdOutputMessage.OutputMessage.Code.Should().Be(createdOutputMessage.Code);
            createdOutputMessage.OutputMessage.Description.Should().Be(createdOutputMessage.Description);
        }
    }

    [Fact]
    public void OutputMessage_Should_Description_Changed()
    {
        // Arrange
        var outputMessageCollection = new OutputMessage[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: Guid.NewGuid().ToString()),
        };
        var expectedDescriptionCollection = outputMessageCollection.Select(q => Guid.NewGuid().ToString()).ToArray();
        var changedDescriptionOutputMessageCollection = new OutputMessage[outputMessageCollection.Length];

        // Act
        for (int i = 0; i < outputMessageCollection.Length; i++)
            changedDescriptionOutputMessageCollection[i] =
                outputMessageCollection[i].ChangeDescription(
                    expectedDescriptionCollection[i]
                );

        // Assert
        for (int i = 0; i < outputMessageCollection.Length; i++)
        {
            changedDescriptionOutputMessageCollection[i].Type.Should().Be(outputMessageCollection[i].Type);
            changedDescriptionOutputMessageCollection[i].Code.Should().Be(outputMessageCollection[i].Code);
            changedDescriptionOutputMessageCollection[i].Description.Should().NotBe(outputMessageCollection[i].Description);
            changedDescriptionOutputMessageCollection[i].Description.Should().Be(expectedDescriptionCollection[i]);
        }
    }

    [Fact]
    public void OutputMessage_Should_Not_Created()
    {
        // Arrange
        var expectedOutputMessageTypeCollection = Enum.GetValues<OutputMessageType>().Select(q => (int)q).ToList()!;
        expectedOutputMessageTypeCollection.Add(0);
        expectedOutputMessageTypeCollection.Add(int.MaxValue);

        var expectedCodeCollection = new[]
        {
            string.Empty,
            null,
            " ",
            "  ",
        };
        var expectedDescriptionCollection = new[]
        {
            string.Empty,
            null,
            " ",
            new string('A', 50)
        };
        var expectedCreatedOutputMessageCount = expectedOutputMessageTypeCollection.Count * expectedCodeCollection.Length * expectedDescriptionCollection.Length;
        var createActionCollection = new List<Action>(capacity: expectedCreatedOutputMessageCount);

        // Act
        foreach (var outputMessageType in expectedOutputMessageTypeCollection)
            foreach (var code in expectedCodeCollection)
                foreach (var description in expectedDescriptionCollection)
                    createActionCollection.Add(() => OutputMessage.Create((OutputMessageType)outputMessageType, code!, description));

        // Assert
        foreach (var outputAction in createActionCollection)
            outputAction.Should().Throw<Exception>();
    }
}