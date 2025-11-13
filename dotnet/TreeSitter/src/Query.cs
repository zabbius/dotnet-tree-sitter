using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TreeSitter;

public class QueryException(uint errorOffset, QueryError error) : Exception
{
    public uint ErrorOffset { get; } = errorOffset;
    public QueryError Error { get; } = error;
}

public sealed class QueryCapture(uint index, Node node)
{
    public readonly uint Index = index;
    public readonly Node Node = node;
}

public sealed class QueryMatch(ushort index, QueryCapture[] captures)
{
    public readonly ushort Index = index;
    public readonly QueryCapture[] Captures = captures;
}

public sealed class Query : IDisposable
{
    internal IntPtr Ptr;

    internal Query(IntPtr ptr)
    {
        Ptr = ptr;
    }

    public Query(Language language, string source)
    {
        Ptr = Binding.ts_query_new(language.Ptr, source, (uint)source.Length, out var errorOffset, out var errorType);

        if (Ptr == IntPtr.Zero)
        {
            throw new QueryException(errorOffset / sizeof(ushort), errorType);
        }
    }

    public void Dispose()
    {
        if (Ptr != IntPtr.Zero)
        {
            Binding.ts_query_delete(Ptr);
            Ptr = IntPtr.Zero;
        }
    }

    public QueryCursor Exec(Node node)
    {
        var cursor = new QueryCursor(this, node.Tree);
        Binding.ts_query_cursor_exec(cursor.Ptr, Ptr, node.NativeNode);
        return cursor;
    }


    public uint PatternCount() => Binding.ts_query_pattern_count(Ptr);

    public uint CaptureCount() => Binding.ts_query_capture_count(Ptr);

    public uint StringCount() => Binding.ts_query_string_count(Ptr);

    public uint StartOffsetForPattern(uint patternIndex) => Binding.ts_query_start_byte_for_pattern(Ptr, patternIndex) / sizeof(ushort);

    public QueryPredicateStep[] PredicatesForPattern(uint patternIndex) => Binding.ts_query_predicates_for_pattern(Ptr, patternIndex, out _);

    public bool IsPatternRooted(uint patternIndex) => Binding.ts_query_is_pattern_rooted(Ptr, patternIndex);

    public bool IsPatternNonLocal(uint patternIndex) => Binding.ts_query_is_pattern_non_local(Ptr, patternIndex);

    public bool IsPatternGuaranteedAtOffset(uint offset) => Binding.ts_query_is_pattern_guaranteed_at_step(Ptr, offset / sizeof(ushort));

    public string CaptureNameForId(uint id) => Marshal.PtrToStringAnsi(Binding.ts_query_capture_name_for_id(Ptr, id, out _));

    public Quantifier CaptureQuantifierForId(uint patternId, uint captureId) => Binding.ts_query_capture_quantifier_for_id(Ptr, patternId, captureId);

    public string StringValueForId(uint id) => Marshal.PtrToStringAnsi(Binding.ts_query_string_value_for_id(Ptr, id, out _));

    public void DisableCapture(string captureName) => Binding.ts_query_disable_capture(Ptr, captureName, (uint)captureName.Length);

    public void DisablePattern(uint patternIndex) => Binding.ts_query_disable_pattern(Ptr, patternIndex);
}

public static class QueryUtils
{
    public static IEnumerable<QueryMatch> Matches(this Query query, Node node)
    {
        var cursor = query.Exec(node);
        while (cursor.NextMatch() is { } match)
        {
            yield return match;
        }
    }

    public static IEnumerable<QueryCapture> Captures(this Query query, Node node)
    {
        var cursor = query.Exec(node);
        while (cursor.NextCapture() is { } capture)
        {
            yield return capture;
        }
    }
    
    public static QueryCapture ByIndex( this IEnumerable<QueryCapture> captures, uint index) =>
        captures.FirstOrDefault(x => x.Index == index);

    public static QueryMatch ByIndex( this IEnumerable<QueryMatch> matches, uint index) =>
        matches.FirstOrDefault(x => x.Index == index);

    public static IEnumerable<Node> CapturedNodes(this Query query, Node node) =>
        query.Captures(node).Select(x => x.Node);
 }