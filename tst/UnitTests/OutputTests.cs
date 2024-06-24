using FluentAssertions;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace UnitTests;
public class OutputTests
{
    [Fact]
    public void Output_Should_Created()
    {
        // Arrange
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var outputCollection = new Output<string>[outputTypeCollection.Length];
        var expectedValue = Guid.NewGuid().ToString();
        var expectedOutputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
        };
        var expectedExceptionCollection = new[]
        {
            new Exception(),
            new Exception(),
        };

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputCollection[i] = Output<string>.Create(
                type: outputTypeCollection[i],
                value: expectedValue,
                outputMessageCollection: expectedOutputMessageCollection,
                exceptionCollection: expectedExceptionCollection
            );

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var output = outputCollection[i];

            output.Type.Should().Be(outputTypeCollection[i]);
            output.IsSuccess.Should().Be(output.Type == OutputType.Success);
            output.IsFailure.Should().Be(output.Type == OutputType.Failure);
            output.IsPartial.Should().Be(output.Type == OutputType.Partial);

            output.Value.Should().Be(expectedValue);
            output.HasValue.Should().BeTrue();

            output.IsSuccessAndHasValue.Should().Be(output.IsSuccess && output.HasValue);

            output.OutputMessageCollection.Should().BeSameAs(expectedOutputMessageCollection);
            output.HasOutputMessage.Should().BeTrue();

            output.ExceptionCollection.Should().BeSameAs(expectedExceptionCollection);
            output.HasException.Should().BeTrue();

            output.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void Output_Should_Created_With_Null_Values()
    {
        // Arrange
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var outputCollection = new Output<string>[outputTypeCollection.Length];

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputCollection[i] = Output<string>.Create(
                type: outputTypeCollection[i]
            );

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var output = outputCollection[i];

            output.Type.Should().Be(outputTypeCollection[i]);
            output.IsSuccess.Should().Be(output.Type == OutputType.Success);
            output.IsFailure.Should().Be(output.Type == OutputType.Failure);
            output.IsPartial.Should().Be(output.Type == OutputType.Partial);

            output.Value.Should().BeNull();
            output.HasValue.Should().BeFalse();

            output.IsSuccessAndHasValue.Should().Be(output.IsSuccess && output.HasValue);

            output.OutputMessageCollection.Should().BeNull();
            output.HasOutputMessage.Should().BeFalse();

            output.ExceptionCollection.Should().BeNull();
            output.HasException.Should().BeFalse();

            output.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void Output_Should_Created_Success()
    {
        // Arrange
        var expectedType = OutputType.Success;

        // Act
        var output = Output<string>.CreateSuccess();

        // Assert
        output.Type.Should().Be(expectedType);
    }

    [Fact]
    public void Output_Should_Created_Failure()
    {
        // Arrange
        var expectedType = OutputType.Failure;

        // Act
        var output = Output<string>.CreateFailure();

        // Assert
        output.Type.Should().Be(expectedType);
    }

    [Fact]
    public void Output_Should_Created_Partial()
    {
        // Arrange
        var expectedType = OutputType.Partial;

        // Act
        var output = Output<string>.CreatePartial();

        // Assert
        output.Type.Should().Be(expectedType);
    }

    [Fact]
    public void Output_Should_Not_Created_With_Invalid_Type()
    {
        // Arrange
        var wrongOutputTypeCollection = new OutputType[] { 
            0,
            (OutputType)short.MaxValue
        };
        var actionCollection = new Action[wrongOutputTypeCollection.Length];

        // Act
        for (int i = 0; i < wrongOutputTypeCollection.Length; i++)
        {
            var wrongOutputType = wrongOutputTypeCollection[i];
            actionCollection[i] = new Action(() => Output<string?>.Create(type: wrongOutputType));

        }

        // Assert
        foreach (var action in actionCollection)
            action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Output_Should_Not_Created_With_Invalid_OutputMessage()
    {
        // Arrange
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var actionCollection = new Action[outputTypeCollection.Length];
        var expectedValue = Guid.NewGuid().ToString();
        var expectedOutputMessageCollection = new OutputMessage[]
        {
            default,
            default,
        };

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var outputType = outputTypeCollection[i];

            actionCollection[i] = new Action(() => 
                Output<string>.Create(
                    type: outputType,
                    value: expectedValue,
                    outputMessageCollection: expectedOutputMessageCollection
                )
            );
        }

        // Assert
        foreach (var action in actionCollection)
            action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Output_Should_Not_Created_With_Null_Exception()
    {
        // Arrange
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var actionCollection = new Action[outputTypeCollection.Length];
        var expectedValue = Guid.NewGuid().ToString();
        var expectedOutputMessageCollection = new OutputMessage[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
        };

        var expectedExceptionCollection = new Exception[]
        {
            null!,
            null!
        };

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var outputType = outputTypeCollection[i];

            actionCollection[i] = new Action(() =>
                Output<string>.Create(
                    type: outputType,
                    value: expectedValue,
                    outputMessageCollection: expectedOutputMessageCollection,
                    exceptionCollection: expectedExceptionCollection
                )
            );
        }

        // Assert
        foreach (var action in actionCollection)
            action.Should().Throw<InvalidOperationException>();
    }
}
