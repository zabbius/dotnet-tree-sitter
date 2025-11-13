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
}
