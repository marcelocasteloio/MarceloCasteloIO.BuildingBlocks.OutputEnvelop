using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace MarceloCasteloIO.BuildingBlocks.OutputEnvelop;

public readonly struct Output<TValue>
{
    // Constants
    public const string OUTPUT_MESSAGE_SHOULD_BE_VALID_MESSAGE = "OutputMessage should be valid";
    public const string EXCEPTION_SHOULD_BE_NOT_NULL_MESSAGE = "Exception should be not null";
    public const string OUTPUT_SHOULD_BE_VALID_MESSAGE = "Output should be valid";

    // Properties
    public OutputType Type { get; }
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public bool IsPartial { get; }

    public TValue? Value { get; }
    public bool HasValue { get; }
    public bool IsSuccessAndHasValue { get; }

    public OutputMessage[]? OutputMessageCollection { get; }
    public bool HasOutputMessage { get; }

    public Exception[]? ExceptionCollection { get; }
    public bool HasException { get; }

    public bool IsValid { get; }

    // Constructors
    private Output(
        OutputType type,
        TValue? value,
        OutputMessage[]? outputMessageCollection,
        Exception[]? exceptionCollection
    )
    {
        Type = type;

        if (type == OutputType.Success)
            IsSuccess = true;
        else if (type == OutputType.Failure)
            IsFailure = true;
        else if (type == OutputType.Partial)
            IsPartial = true;

        Value = value;
        if (value is not null)
        {
            HasValue = true;
            IsSuccessAndHasValue = type == OutputType.Success;
        }

        OutputMessageCollection = outputMessageCollection;
        HasOutputMessage = outputMessageCollection is not null;

        ExceptionCollection = exceptionCollection;
        HasException = exceptionCollection is not null;

        IsValid = true;
    }

    // Builders
    public static Output<TValue> Create(
        OutputType type,
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        // Validation
        var typeValue = (short)type;
        // Stryker disable once all
        if (typeValue <= 0 || typeValue > 3)
            // Stryker disable once all
            throw new ArgumentOutOfRangeException(nameof(type));

        if (outputMessageCollection is not null)
            for (int i = 0; i < outputMessageCollection.Length; i++)
                if (!outputMessageCollection[i].IsValid)
                    throw new InvalidOperationException(OUTPUT_MESSAGE_SHOULD_BE_VALID_MESSAGE);

        if (exceptionCollection is not null)
            for (int i = 0; i < exceptionCollection.Length; i++)
                if (exceptionCollection[i] is null)
                    throw new InvalidOperationException(EXCEPTION_SHOULD_BE_NOT_NULL_MESSAGE);

        // Process and return
        return new Output<TValue>(
            type,
            value,
            outputMessageCollection,
            exceptionCollection
        );
    }
    public static Output<TValue> Create(
        OutputType type,
        TValue? value,
        Output<TValue> output
    )
    {
        // Validate
        if (!output.IsValid)
            throw new InvalidOperationException(OUTPUT_SHOULD_BE_VALID_MESSAGE);

        // Process and return
        return Create(
            type,
            value,
            outputMessageCollection: output.OutputMessageCollection,
            exceptionCollection: output.ExceptionCollection
        );
    }
    public static Output<TValue> Create(
        OutputType type,
        TValue? value,
        Output<TValue>[] outputCollection
    )
    {
        // Validate
        for (int i = 0; i < outputCollection.Length; i++)
            if (!outputCollection[i].IsValid)
                throw new InvalidOperationException(OUTPUT_SHOULD_BE_VALID_MESSAGE);

        // Process OutputMessage
        var outputMessageCollection = default(OutputMessage[]?);
        var outputMessageCollectionCount = 0;

        for (int i = 0; i < outputCollection.Length; i++)
        {
            var output = outputCollection[i];
            if (output.OutputMessageCollection is not null)
                outputMessageCollectionCount += output.OutputMessageCollection.Length;
        }

        if (outputMessageCollectionCount > 0)
        {
            outputMessageCollection = new OutputMessage[outputMessageCollectionCount];
            var currentIndex = 0;

            for (int i = 0; i < outputCollection.Length; i++)
            {
                if (outputCollection[i].OutputMessageCollection is null)
                    continue;

                var currentOutputMessageCollection = outputCollection[i].OutputMessageCollection!;

                Array.Copy(
                    sourceArray: currentOutputMessageCollection,
                    sourceIndex: 0,
                    destinationArray: outputMessageCollection,
                    destinationIndex: currentIndex,
                    length: currentOutputMessageCollection.Length
                );

                currentIndex += currentOutputMessageCollection.Length;
            }
        }

        // Process Exception
        var exceptionCollection = default(Exception[]?);
        var exceptionCollectionCount = 0;

        for (int i = 0; i < outputCollection.Length; i++)
        {
            var output = outputCollection[i];
            if (output.ExceptionCollection is not null)
                exceptionCollectionCount += output.ExceptionCollection.Length;
        }

        if (exceptionCollectionCount > 0)
        {
            exceptionCollection = new Exception[exceptionCollectionCount];
            var currentIndex = 0;

            for (int i = 0; i < outputCollection.Length; i++)
            {
                if (outputCollection[i].ExceptionCollection is null)
                    continue;

                var currentExceptionCollection = outputCollection[i].ExceptionCollection!;

                Array.Copy(
                    sourceArray: currentExceptionCollection,
                    sourceIndex: 0,
                    destinationArray: exceptionCollection,
                    destinationIndex: currentIndex,
                    length: currentExceptionCollection.Length
                );

                currentIndex += currentExceptionCollection.Length;
            }
        }

        // Return
        return Create(
            type,
            value,
            outputMessageCollection,
            exceptionCollection
        );
    }
    public static Output<TValue> Create(
        TValue? value,
        Output<TValue>[] outputCollection
    )
    {
        // Process
        var hasSuccess = false;
        var hasFailure = false;
        var hasPartial = false;
        
        for (int i = 0; i < outputCollection.Length; i++)
        {
            if (hasPartial)
                continue;
            else if (hasSuccess && hasFailure)
                continue;

            var output = outputCollection[i];
            
            if (!output.IsValid)
                continue;

            if(output.Type == OutputType.Success)
                hasSuccess = true;
            else if (output.Type == OutputType.Failure)
                hasFailure = true;
            else if(output.Type == OutputType.Partial)
                hasPartial = true;
        }

        var type = default(OutputType);

        if (hasPartial)
            type = OutputType.Partial;
        else if(hasFailure)
            type = hasSuccess ? OutputType.Partial : OutputType.Failure;
        else
            type = OutputType.Success;

        return Create(
            type,
            value,
            outputCollection
        );
    }

    public static Output<TValue> CreateSuccess(
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        return Create(OutputType.Success, value, outputMessageCollection, exceptionCollection);
    }
    public static Output<TValue> CreateSuccess(
        TValue? value,
        OutputMessageType outputMessageType,
        string outputMessageCode,
        string? outputMessageDescription = null
    )
    {
        return Create(
            OutputType.Success,
            value,
            outputMessageCollection:
            [
                OutputMessage.Create(outputMessageType, outputMessageCode, outputMessageDescription)
            ],
            exceptionCollection: null
        );
    }
    public static Output<TValue> CreateSuccess(
        TValue? value,
        string outputMessageCode,
        string? outputMessageDescription = null
    )
    {
        return CreateSuccess(
            value,
            outputMessageType: OutputMessageType.Information,
            outputMessageCode,
            outputMessageDescription
        );
    }
    public static Output<TValue> CreateSuccess(
        TValue? value,
        Output<TValue> output
    )
    {
        return Create(
            type: OutputType.Success,
            value,
            outputMessageCollection: output.OutputMessageCollection,
            exceptionCollection: output.ExceptionCollection
        );
    }
    public static Output<TValue> CreateFailure(
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        return Create(OutputType.Failure, value, outputMessageCollection, exceptionCollection);
    }
    public static Output<TValue> CreateFailure(
        TValue? value,
        OutputMessageType outputMessageType,
        string outputMessageCode,
        string? outputMessageDescription = null
    )
    {
        return Create(
            OutputType.Failure,
            value,
            outputMessageCollection:
            [
                OutputMessage.Create(outputMessageType, outputMessageCode, outputMessageDescription)
            ],
            exceptionCollection: null
        );
    }
    public static Output<TValue> CreateFailure(
        TValue? value,
        string outputMessageCode,
        string? outputMessageDescription = null
    )
    {
        return CreateFailure(
            value,
            outputMessageType: OutputMessageType.Error,
            outputMessageCode,
            outputMessageDescription
        );
    }
    public static Output<TValue> CreateFailure(
        TValue? value,
        Output<TValue> output
    )
    {
        return Create(
            type: OutputType.Failure,
            value,
            outputMessageCollection: output.OutputMessageCollection,
            exceptionCollection: output.ExceptionCollection
        );
    }
    public static Output<TValue> CreatePartial(
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        return Create(OutputType.Partial, value, outputMessageCollection, exceptionCollection);
    }
}
