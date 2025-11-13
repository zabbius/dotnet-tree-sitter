using System;
using System.Collections.Generic;

namespace TreeSitter;

public static class NodeExtensions
{
    public static IEnumerable<Node> Children(this Node node)
    {
        for (uint i = 0; i < node.ChildCount(); i++)
        {
            yield return node.Child(i);
        }
    }
    
    public static IEnumerable<Node> NamedChildren(this Node node)
    {
        for (uint i = 0; i < node.NamedChildCount(); i++)
        {
            yield return node.NamedChild(i);
        }
    }

    public static IEnumerable<Node> ChildrenByFieldName(this Node node, string fieldName)
    {
        var fieldId = node.Tree.Language.FieldIdForName(fieldName);

        if (fieldId == 0)
        {
            yield break;
        }

        var cursor = new TreeCursor(node);
        var ok = cursor.GotoFirstChild();

        while (ok)
        {
            if (cursor.CurrentField() == fieldName)
            {
                yield return cursor.CurrentNode();
            }
            ok = cursor.GotoNextSibling();
        }
    }
    
    #region TryGet
    public static bool TryGetParent(this Node node, out Node parent)
    {
        parent = node.Parent();
        return parent != null;
    }
    
    public static bool TryGetChild(this Node node, uint index, out Node child)
    {
        child = node.Child(index);
        return child != null;
    }
    
    public static bool TryGetNamedChild(this Node node, uint index, out Node child)
    {
        child = node.NamedChild(index);
        return child != null;
    }
    
    public static bool TryGetChildByFieldName(this Node node, string fieldName, out Node child)
    {   
        child = node.ChildByFieldName(fieldName);
        return child != null;
    }
        
    public static bool TryGetChildByFieldId(this Node node, ushort fieldId, out Node child)
    {
        child = node.ChildByFieldId(fieldId);
        return child != null;
    }
    
    public static bool TryGetNextSibling(this Node node, out Node sibling)
    {
        sibling = node.NextNamedSibling();
        return sibling  != null;
    }
    
    public static bool TryGetPrevSibling(this Node node, out Node sibling)
    {
        sibling = node.PrevSibling();
        return sibling  != null;
    }
    
    public static bool TryGetNextNamedSibling(this Node node, out Node sibling)
    {
        sibling = node.NextNamedSibling();
        return sibling != null;
    }
        
    public static bool TryGetPrevNamedSibling(this Node node, out Node sibling)
    {
        sibling = node.PrevNamedSibling();
        return sibling != null;
    }
    
    public static bool TryGetFirstChildForOffset(this Node node, uint offset, out Node sibling)
    {
        sibling = node.FirstChildForOffset(offset);
        return sibling != null;
    }
    
    public static bool TryGetFirstNamedChildForOffset(this Node node, uint offset, out Node sibling)
    {
        sibling = node.FirstNamedChildForOffset(offset);
        return sibling != null;
    }
    
    public static bool TryGetDescendantForOffsetRange(this Node node, uint start, uint end, out Node sibling)
    {
        sibling = node.DescendantForOffsetRange(start, end);
        return sibling != null;
    }
    
    public static bool TryGetDescendantForPointRange(this Node node, Point start, Point end, out Node sibling)
    {
        sibling = node.DescendantForPointRange(start, end);
        return sibling != null;
    }
    
    public static bool TryGetNamedDescendantForOffsetRange(this Node node, uint start, uint end, out Node sibling)
    {
        sibling = node.NamedDescendantForOffsetRange(start, end);
        return sibling != null;
    }
    
    public static bool TryGetNamedDescendantForPointRange(this Node node, Point start, Point end, out Node sibling)
    {
        sibling = node.NamedDescendantForPointRange(start, end);
        return sibling != null;
    }
    #endregion
}
