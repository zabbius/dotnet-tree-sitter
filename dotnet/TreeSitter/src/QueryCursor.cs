using System;
using System.Runtime.InteropServices;

namespace TreeSitter;

public sealed class QueryCursor : IDisposable
{
    internal IntPtr Ptr;
    internal readonly Query Query;

    public Tree Tree { get; }
    
    internal QueryCursor(Query query, Tree tree)
    {
        Tree = tree;
        Query = query;
        Ptr = Binding.ts_query_cursor_new();
    }

    public void Dispose()
    {
        if (Ptr != IntPtr.Zero)
        {
            Binding.ts_query_cursor_delete(Ptr);
            Ptr = IntPtr.Zero;
        }
    }

    public bool DidExceedMatchLimit()
    {
        return Binding.ts_query_cursor_did_exceed_match_limit(Ptr);
    }

    public uint MatchLimit()
    {
        return Binding.ts_query_cursor_match_limit(Ptr);
    }

    public void SetMatchLimit(uint limit)
    {
        Binding.ts_query_cursor_set_match_limit(Ptr, limit);
    }

    public void SetRange(uint start, uint end)
    {
        Binding.ts_query_cursor_set_byte_range(Ptr, start * sizeof(ushort), end * sizeof(ushort));
    }

    public void SetPointRange(Point start, Point end)
    {
        Binding.ts_query_cursor_set_point_range(Ptr, start, end);
    }

    public QueryMatch NextMatch()
    {
        if (!Binding.ts_query_cursor_next_match(Ptr, out var nativeMatch))
        {
            return null;
        }

        var match = new QueryMatch(nativeMatch.PatternIndex, new QueryCapture[nativeMatch.CaptureCount]);
        
        for (var n = 0; n < nativeMatch.CaptureCount; n++)
        {
            var intPtr = nativeMatch.Captures + Marshal.SizeOf(typeof(Binding.QueryCapture)) * n;
            var nativeCapture = Marshal.PtrToStructure<Binding.QueryCapture>(intPtr);

            match.Captures[n] = new QueryCapture(nativeCapture.Index, Node.FromNative(nativeCapture.Node, Tree));
        }

        return match;
    }

    public void RemoveMatch(uint id) => Binding.ts_query_cursor_remove_match(Ptr, id);

    public QueryCapture NextCapture()
    {
        if (!Binding.ts_query_cursor_next_capture(Ptr, out var nativeMatch, out var captureIndex))
        {
            return null;
        }
        var intPtr = nativeMatch.Captures + Marshal.SizeOf(typeof(Binding.QueryCapture)) * (ushort)captureIndex;
        var nativeCapture = Marshal.PtrToStructure<Binding.QueryCapture>(intPtr);
        return new QueryCapture(nativeCapture.Index, Node.FromNative(nativeCapture.Node, Tree));

    }

}