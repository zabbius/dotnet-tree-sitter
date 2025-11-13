using System;
using System.Runtime.InteropServices;

namespace TreeSitter;

public sealed class Node
{
    internal readonly Binding.Node NativeNode;

    public Tree Tree { get; }

    internal Node(Binding.Node nativeNode, Tree tree)
    {
        NativeNode = nativeNode;
        Tree = tree;
    }

    public string Type() => Marshal.PtrToStringAnsi(Binding.ts_node_type(NativeNode));
    public ushort Symbol() => Binding.ts_node_symbol(NativeNode);
    public uint StartOffset() => Binding.ts_node_start_byte(NativeNode) / sizeof(ushort);

    public Point StartPoint()
    {
        var pt = Binding.ts_node_start_point(NativeNode);
        return new Point(pt.Row, pt.Column / sizeof(ushort));
    }

    public uint EndOffset() => Binding.ts_node_end_byte(NativeNode) / sizeof(ushort);

    public Point EndPoint()
    {
        var pt = Binding.ts_node_end_point(NativeNode);
        return new Point(pt.Row, pt.Column / sizeof(ushort));
    }

    public override string ToString()
    {
        var dat = Binding.ts_node_string(NativeNode);
        var str = Marshal.PtrToStringAnsi(dat);
        Binding.ts_node_string_free(dat);
        return str;
    }

    [Obsolete("Always true for non-null references")]
    public bool IsNull() => Binding.ts_node_is_null(NativeNode);
    public bool IsNamed() => Binding.ts_node_is_named(NativeNode);

    public bool IsMissing() => Binding.ts_node_is_missing(NativeNode);

    public bool IsExtra() => Binding.ts_node_is_extra(NativeNode);

    public bool IsError() => Binding.ts_node_is_error(NativeNode);

    public bool HasChanges() => Binding.ts_node_has_changes(NativeNode);

    public bool HasError() => Binding.ts_node_has_error(NativeNode);

    public Node Parent() => FromNative(Binding.ts_node_parent(NativeNode), Tree);

    public Node Child(uint index) => FromNative(Binding.ts_node_child(NativeNode, index), Tree);

    public string FieldNameForChild(uint index) => Marshal.PtrToStringAnsi(Binding.ts_node_field_name_for_child(NativeNode, index));

    public uint ChildCount() => Binding.ts_node_child_count(NativeNode);

    public Node NamedChild(uint index) => FromNative(Binding.ts_node_named_child(NativeNode, index), Tree);

    public uint NamedChildCount() => Binding.ts_node_named_child_count(NativeNode);

    public Node ChildByFieldName(string fieldName) => FromNative(Binding.ts_node_child_by_field_name(NativeNode, fieldName, (uint)fieldName.Length), Tree);

    public Node ChildByFieldId(ushort fieldId) => FromNative(Binding.ts_node_child_by_field_id(NativeNode, fieldId), Tree);

    public Node NextSibling() => FromNative(Binding.ts_node_next_sibling(NativeNode), Tree);

    public Node PrevSibling() => FromNative(Binding.ts_node_prev_sibling(NativeNode), Tree);

    public Node NextNamedSibling() => FromNative(Binding.ts_node_next_named_sibling(NativeNode), Tree);

    public Node PrevNamedSibling() => FromNative(Binding.ts_node_prev_named_sibling(NativeNode), Tree);

    public Node FirstChildForOffset(uint offset) => FromNative(Binding.ts_node_first_child_for_byte(NativeNode, offset * sizeof(ushort)), Tree);

    public Node FirstNamedChildForOffset(uint offset) => FromNative(Binding.ts_node_first_named_child_for_byte(NativeNode, offset * sizeof(ushort)), Tree);

    public Node DescendantForOffsetRange(uint start, uint end) => FromNative(Binding.ts_node_descendant_for_byte_range(NativeNode, start * sizeof(ushort), end * sizeof(ushort)), Tree);

    public Node DescendantForPointRange(Point start, Point end) => FromNative(Binding.ts_node_descendant_for_point_range(NativeNode, start, end), Tree);

    public Node NamedDescendantForOffsetRange(uint start, uint end) => FromNative(Binding.ts_node_named_descendant_for_byte_range(NativeNode, start * sizeof(ushort), end * sizeof(ushort)), Tree);

    public Node NamedDescendantForPointRange(Point start, Point end) => FromNative(Binding.ts_node_named_descendant_for_point_range(NativeNode, start, end), Tree);

    public string Text(string data) => data[(int)StartOffset()..(int)EndOffset()];

    internal static Node FromNative(Binding.Node nativeNode, Tree tree) => nativeNode.Id == IntPtr.Zero ? null : new(nativeNode, tree);

    public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is Node other && this == other;
    public override int GetHashCode() => NativeNode.GetHashCode();

    public static bool operator == (Node a, Node b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if ((object)a == null || (object)b == null)
        {
            return false;
        }

        return Binding.ts_node_eq(a.NativeNode, b.NativeNode);
    }

    public static bool operator !=(Node a, Node b) => !(a == b);
}
