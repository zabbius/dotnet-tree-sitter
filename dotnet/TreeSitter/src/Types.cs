using System.Runtime.InteropServices;

namespace TreeSitter;

public enum InputEncoding
{
    InputEncodingUTF8,
    InputEncodingUTF16
}

public enum SymbolType
{
    SymbolTypeRegular,
    SymbolTypeAnonymous,
    SymbolTypeAuxiliary,
}

public enum LogType
{
    LogTypeParse,
    LogTypeLex,
}

public enum Quantifier
{
    QuantifierZero = 0, // must match the array initialization value
    QuantifierZeroOrOne,
    QuantifierZeroOrMore,
    QuantifierOne,
    QuantifierOneOrMore,
}

public enum QueryPredicateStepType
{
    QueryPredicateStepTypeDone,
    QueryPredicateStepTypeCapture,
    QueryPredicateStepTypeString,
}

public enum QueryError
{
    QueryErrorNone = 0,
    QueryErrorSyntax,
    QueryErrorNodeType,
    QueryErrorField,
    QueryErrorCapture,
    QueryErrorStructure,
    QueryErrorLanguage,
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct Point(uint row, uint column)
{
    public readonly uint Row = row;
    public readonly uint Column = column;

    public override string ToString() => $"{Row}:{Column}";

}

[StructLayout(LayoutKind.Sequential)]
public struct Range
{
    public Point StartPoint;
    public Point EndPoint;
    public uint StartByte;
    public uint EndByte;

    public override string ToString() => $"{StartPoint}-{EndPoint}";
}

[StructLayout(LayoutKind.Sequential)]
public struct InputEdit
{
    public uint start_byte;
    public uint old_end_byte;
    public uint new_end_byte;
    public Point start_point;
    public Point old_end_point;
    public Point new_end_point;
}

[StructLayout(LayoutKind.Sequential)]
public struct QueryPredicateStep
{
    public QueryPredicateStepType type;
    public uint value_id;
}

public delegate void Logger(LogType logType, string message);