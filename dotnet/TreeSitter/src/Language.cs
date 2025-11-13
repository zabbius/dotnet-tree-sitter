using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TreeSitter;

public class Language : IDisposable
{
    internal readonly string[] Symbols;
    internal readonly string[] Fields;
    internal readonly Dictionary<string, ushort> FieldIds;

    internal readonly IntPtr Ptr;

    protected Language(IntPtr ptr)
    {
        Ptr = ptr;

        var symbolCount = Binding.ts_language_symbol_count(Ptr) + 1;
        var symbols = new string[symbolCount];

        for (ushort i = 0; i < symbols.Length; i++)
        {
            symbols[i] = Marshal.PtrToStringAnsi(Binding.ts_language_symbol_name(Ptr, i));
        }

        Symbols = symbols;

        var fieldCount = (int)Binding.ts_language_field_count(Ptr) + 1;
        var fields = new string[fieldCount + 1];
        var fieldIds = new Dictionary<string, ushort>();

        for (ushort i = 0; i < fields.Length; i++)
        {
            fields[i] = Marshal.PtrToStringAnsi(Binding.ts_language_field_name_for_id(Ptr, i));
            if (fields[i] != null)
            {
                fieldIds.Add(fields[i], i); // TODO: check for dupes, and throw if found
            }
        }
        
        Fields = fields;
        FieldIds = fieldIds;
    }

    public void Dispose()
    {
    }

    public string SymbolName(ushort symbol) => symbol != ushort.MaxValue ? Symbols[symbol] : "ERROR";
    public ushort SymbolForName(string str, bool isNamed) => Binding.ts_language_symbol_for_name(Ptr, str, (uint)str.Length, isNamed);
    public ushort FieldIdForName(string str) => FieldIds.GetValueOrDefault(str, (ushort)0);
    public SymbolType SymbolType(ushort symbol) => Binding.ts_language_symbol_type(Ptr, symbol);


    internal static Language FromNative(IntPtr ptr) => new(ptr);
}