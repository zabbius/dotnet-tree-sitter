using System;

namespace TreeSitter;

public sealed class Parser : IDisposable
{
    private IntPtr Ptr { get; set; }
    public Language Language { get; private set; }

    public Parser()
    {
        Ptr = Binding.ts_parser_new();
    }

    public Parser(Language language)
        : this()
    {
        if (!SetLanguage(language))
        {
            throw new Exception("Language could not be set");
        }
    }

    public void Dispose()
    {
        if (Ptr != IntPtr.Zero)
        {
            Binding.ts_parser_delete(Ptr);
            Ptr = IntPtr.Zero;
        }
    }

    public bool SetLanguage(Language language)
    {
        if (Binding.ts_parser_set_language(Ptr, language.Ptr))
        {
            Language = language;
            return true;
        }

        return false;
    }

    public bool SetIncludedRanges(Range[] ranges) => Binding.ts_parser_set_included_ranges(Ptr, ranges, (uint)ranges.Length);

    public Range[] IncludedRanges() => Binding.ts_parser_included_ranges(Ptr, out _);

    public Tree ParseString(string source, Tree oldTree = null)
    {
        var ptr = Binding.ts_parser_parse_string_encoding(Ptr, oldTree?.Ptr ?? IntPtr.Zero,
            source, (uint)source.Length * 2, InputEncoding.InputEncodingUTF16);
        return ptr != IntPtr.Zero ? new Tree(ptr, Language) : null;
    }

    public void Reset() => Binding.ts_parser_reset(Ptr);

    public void SetTimeout(TimeSpan timeout) => Binding.ts_parser_set_timeout_micros(Ptr, (ulong)timeout.TotalMicroseconds);

    public TimeSpan TimeOut() => TimeSpan.FromMicroseconds(Binding.ts_parser_timeout_micros(Ptr));

    public void SetLogger(Logger logger)
    {
        var data = new Binding.LoggerData
        {
            Log = logger != null ? new Binding.LogCallback((_, type, message) => logger(type, message)) : null
        };
        Binding.ts_parser_set_logger(Ptr, data);
    }
}