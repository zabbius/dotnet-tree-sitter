using System;
using System.Runtime.InteropServices;

namespace TreeSitter;

internal static class Binding
{
    [StructLayout(LayoutKind.Sequential)]
    public record struct LoggerData
    {
        public IntPtr Payload;
        internal LogCallback Log;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LogCallback(IntPtr payload, LogType logType,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

    [StructLayout(LayoutKind.Sequential)]
    public record struct TreeCursor
    {
        public IntPtr Tree;
        public IntPtr Id;
        public uint Context0;
        public uint Context1;
        public uint Context2;
    }


    [StructLayout(LayoutKind.Sequential)]
    public record struct Node
    {
        public uint Context0;
        public uint Context1;
        public uint Context2;
        public uint Context3;
        public IntPtr Id;
        public IntPtr Tree;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct QueryCapture
    {
        public Node Node;
        public uint Index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public record struct QueryMatch
    {
        public uint Id;
        public ushort PatternIndex;
        public ushort CaptureCount;
        public IntPtr Captures;
    }


    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_delete(IntPtr query);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_query_pattern_count(IntPtr query);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_query_capture_count(IntPtr query);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_query_string_count(IntPtr query);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_query_start_byte_for_pattern(IntPtr query, uint patternIndex);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
    public static extern QueryPredicateStep[] ts_query_predicates_for_pattern(IntPtr query, uint patternIndex, out uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_query_is_pattern_rooted(IntPtr query, uint patternIndex);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_query_is_pattern_non_local(IntPtr query, uint patternIndex);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_query_is_pattern_guaranteed_at_step(IntPtr query, uint byteOffset);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_query_capture_name_for_id(IntPtr query, uint id, out uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Quantifier ts_query_capture_quantifier_for_id(IntPtr query, uint patternId, uint captureId);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_query_string_value_for_id(IntPtr query, uint id, out uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_disable_capture(IntPtr query,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string captureName, uint captureNameLength);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_disable_pattern(IntPtr query, uint patternIndex);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern TreeCursor ts_tree_cursor_new(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_tree_cursor_delete(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_tree_cursor_reset(ref TreeCursor cursor, Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_tree_cursor_current_node(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_tree_cursor_current_field_name(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort ts_tree_cursor_current_field_id(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_tree_cursor_goto_parent(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_tree_cursor_goto_next_sibling(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_tree_cursor_goto_first_child(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern long ts_tree_cursor_goto_first_child_for_byte(ref TreeCursor cursor, uint byteOffset);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern long ts_tree_cursor_goto_first_child_for_point(ref TreeCursor cursor, Point point);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern TreeCursor ts_tree_cursor_copy(ref TreeCursor cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_query_new(IntPtr language, [MarshalAs(UnmanagedType.LPUTF8Str)] string source,
        uint source_len, out uint error_offset, out QueryError error_type);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_language_symbol_count(IntPtr language);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_language_symbol_name(IntPtr language, ushort symbol);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort ts_language_symbol_for_name(IntPtr language,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length, bool is_named);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_language_field_count(IntPtr language);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_language_field_name_for_id(IntPtr language, ushort fieldId);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort ts_language_field_id_for_name(IntPtr language,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string str, uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern SymbolType ts_language_symbol_type(IntPtr language, ushort symbol);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_language_version(IntPtr language);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_node_type(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort ts_node_symbol(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_node_start_byte(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Point ts_node_start_point(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_node_end_byte(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Point ts_node_end_point(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_node_string(Node node);

    public static void ts_node_string_free(IntPtr str) => free(str);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void free(IntPtr str);



    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_is_null(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_is_named(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_is_missing(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_is_extra(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_has_changes(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_has_error(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_is_error(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_parent(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_child(Node node, uint index);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_node_field_name_for_child(Node node, uint index);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_node_child_count(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_named_child(Node node, uint index);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_node_named_child_count(Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_child_by_field_name(Node self,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string field_name, uint field_name_length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_child_by_field_id(Node self, ushort fieldId);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_next_sibling(Node self);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_prev_sibling(Node self);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_next_named_sibling(Node self);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_prev_named_sibling(Node self);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_first_child_for_byte(Node self, uint byteOffset);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_first_named_child_for_byte(Node self, uint byteOffset);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_descendant_for_byte_range(Node self, uint startByte, uint endByte);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_descendant_for_point_range(Node self, Point startPoint, Point endPoint);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_named_descendant_for_byte_range(Node self, uint startByte, uint endByte);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_node_named_descendant_for_point_range(Node self, Point startPoint, Point endPoint);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_node_eq(Node node1, Node node2);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_parser_new();

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_parser_delete(IntPtr parser);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_parser_set_language(IntPtr parser, IntPtr language);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_parser_language(IntPtr parser);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_parser_set_included_ranges(IntPtr parser,
        [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] Range[] ranges, uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
    public static extern Range[] ts_parser_included_ranges(IntPtr parser, out uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_parser_parse_string(IntPtr parser, IntPtr oldTree,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string input, uint length);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_parser_parse_string_encoding(IntPtr parser, IntPtr oldTree,
        [MarshalAs(UnmanagedType.LPWStr)] string input, uint length, InputEncoding encoding);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_parser_reset(IntPtr parser);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_parser_set_timeout_micros(IntPtr parser, ulong timeout);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ts_parser_timeout_micros(IntPtr parser);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_parser_set_cancellation_flag(IntPtr parser, ref IntPtr flag);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_parser_cancellation_flag(IntPtr parser);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_parser_set_logger(IntPtr parser, LoggerData logger);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_query_cursor_new();

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_delete(IntPtr cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_exec(IntPtr cursor, IntPtr query, Node node);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_query_cursor_did_exceed_match_limit(IntPtr cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ts_query_cursor_match_limit(IntPtr cursor);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_set_match_limit(IntPtr cursor, uint limit);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_set_byte_range(IntPtr cursor, uint start_byte, uint end_byte);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_set_point_range(IntPtr cursor, Point start_point, Point end_point);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ts_query_cursor_next_match(IntPtr cursor, out QueryMatch match);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_query_cursor_remove_match(IntPtr cursor, uint id);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool  ts_query_cursor_next_capture(IntPtr cursor, out QueryMatch match, out uint capture_index);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_tree_copy(IntPtr tree);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_tree_delete(IntPtr tree);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_tree_root_node(IntPtr tree);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern Node ts_tree_root_node_with_offset(IntPtr tree, uint offsetBytes, Point offsetPoint);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_tree_language(IntPtr tree);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_tree_included_ranges(IntPtr tree, out uint length);

    public static void ts_tree_included_ranges_free(IntPtr ranges) => free(ranges);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ts_tree_edit(IntPtr tree, ref InputEdit edit);

    [DllImport("tree-sitter", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ts_tree_get_changed_ranges(IntPtr old_tree, IntPtr new_tree, out uint length);
}
