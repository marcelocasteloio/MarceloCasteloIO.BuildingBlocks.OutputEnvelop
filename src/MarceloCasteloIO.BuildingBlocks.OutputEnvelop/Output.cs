using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;
using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;

namespace MarceloCasteloIO.BuildingBlocks.OutputEnvelop;

public readonly struct Output<TValue>
{
    // Constants
    public const string OUTPUT_MESSAGE_SHOULD_BE_VALID_MESSAGE = "OutputMessage should be valid";
    public const string EXCEPTION_SHOULD_BE_NOT_NULL_MESSAGE = "Exception should be not null";

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

        if(outputMessageCollection is not null)
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
    public static Output<TValue> CreateSuccess(
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        return Create(OutputType.Success, value, outputMessageCollection, exceptionCollection);
    }
    public static Output<TValue> CreateFailure(
        TValue? value = default,
        OutputMessage[]? outputMessageCollection = null,
        Exception[]? exceptionCollection = null
    )
    {
        return Create(OutputType.Failure, value, outputMessageCollection, exceptionCollection);
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
