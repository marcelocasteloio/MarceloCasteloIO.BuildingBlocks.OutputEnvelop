using MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Enums;

namespace MarceloCasteloIO.BuildingBlocks.OutputEnvelop.Models;
public readonly struct OutputMessage
{
    // Properties
    public OutputMessageType Type { get; }
    public string Code { get; }
    public string? Description { get; }

    // Constructors
    private OutputMessage(
        OutputMessageType type, 
        string code, 
        string? description
    )
    {
        Type = type;
        Code = code;
        Description = description;
    }

    // Public Methods
    public OutputMessage ChangeDescription(string? description)
    {
        return Create(Type, Code, description);
    }

    // Builders
    public static OutputMessage Create(
        OutputMessageType type,
        string code,
        string? description = null
    )
    {
        // Validate
        var typeValue = (short)type;
        if (typeValue < 0 || typeValue > 3)
            throw new ArgumentOutOfRangeException(nameof(type));

        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        // Process and return
        return new OutputMessage(type, code, description);
    }
    public static OutputMessage CreateInformation(
        string code,
        string? description = null
    )
    {
        return Create(type: OutputMessageType.Information, code, description);
    }
    public static OutputMessage CreateWarning(
        string code,
        string? description = null
    )
    {
        return Create(type: OutputMessageType.Warning, code, description);
    }
    public static OutputMessage CreateError(
        string code,
        string? description = null
    )
    {
        return Create(type: OutputMessageType.Error, code, description);
    }
}
