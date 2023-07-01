using System.Text;

namespace Evilang;

public class Parser
{
    private int _ptr = 0;
    private int _size = 0;
    private bool _hasError;
    private byte[] _arrays;
    private int _segmentPtr;
    private bool _verbose;
    
    private readonly Stack<(int segmentPtr, int cellPtr)> _loop = new ();
    
    private readonly Dictionary<string, TokenType> _keywords = new ()
    {
        {"evil", TokenType.Initialize},
        
        {"e", TokenType.ShiftRight},
        {"v", TokenType.ShiftLeft},
        {"i", TokenType.Increment},
        {"l", TokenType.Decrement},
        
        {"vi", TokenType.Input},
        {"ev", TokenType.Output},
        {"evi", TokenType.OutputInt},
        
        {"il", TokenType.Loop},
        {"el", TokenType.Goto},
    };

    public Parser(bool verbose)
    {
        _verbose = verbose;
        _arrays = Array.Empty<byte>();
    }

    public void SetVerbose(bool verbose) => _verbose = verbose;

    public void Parse(string code)
    {
        var lines = code.Split(Environment.NewLine);
        var segments = new List<string>();
        
        foreach (var line in lines)
        {
            segments.AddRange(line.Split(" "));
        }

        int totalSegments = segments.Count;
        for (_segmentPtr = 0; _segmentPtr < totalSegments; _segmentPtr++)
        {
            var segment = segments[_segmentPtr];
            if (_keywords.ContainsKey(segment))
            {
                if (_hasError) return;

                Execute(segment);
                continue;
            }

            foreach (var c in segment)
            {
                if (_hasError) return;

                Execute(c.ToString());
            }
        }

        if (segments[^1] == "ev" || segments[^1] == "evi") Console.WriteLine();
    }

    private void Execute(string chunk)
    {
        if (!_keywords.TryGetValue(chunk, out var tokenType))
        {
            
            return;
        }
        
        switch (tokenType)
        {
            case TokenType.Initialize: SetArraySize();break;
            case TokenType.ShiftLeft: ShiftLeft(); break;
            case TokenType.ShiftRight: ShiftRight(); break;
            case TokenType.Increment: Increment(); break;
            case TokenType.Decrement: Decrement(); break;
            case TokenType.Input: Input(); break;
            case TokenType.Output: Output(); break;
            case TokenType.OutputInt: OutputInt(); break;
            case TokenType.Loop: Loop(); break;
            case TokenType.Goto: Goto(); break;
        }
    }

    private void SetArraySize()
    {
        _size++;
        int newLength = (int)Math.Pow(2, _size);
        _arrays = new byte[newLength];

        if (!_verbose) return;
        
        Console.WriteLine($"Array [{newLength}]");
    }

    private void ShiftLeft()
    {
        if (IsError()) return;

        _ptr--;
        
        if (!_verbose) return;
        Console.WriteLine($"Shift [{_ptr + 1}] to [{_ptr}]");
    }

    private void ShiftRight()
    {
        if (IsError()) return;

        _ptr++;
        
        if (!_verbose) return;
        Console.WriteLine($"Shift [{_ptr - 1}] to [{_ptr}]");
    }

    private void Increment()
    {
        if (IsError()) return;

        _arrays[_ptr]++;
        
        if (!_verbose) return;
        Console.WriteLine($"[{_ptr}] = {_arrays[_ptr]}");
    }

    private void Decrement()
    {
        if (IsError()) return;
        
        _arrays[_ptr]--;

        if (!_verbose) return;
        Console.WriteLine($"[{_ptr}] = {_arrays[_ptr]}");
    }

    private void Input()
    {
        if (IsError()) return;

        var inputStr = Console.ReadLine();
        if (byte.TryParse(inputStr, out var input))
        {
            _arrays[_ptr] = input;
        }
        else
        {
            Console.WriteLine($"Error unexpected input '{input}'");
        }
    }

    private void Output()
    {
        if (IsError()) return;

        var output = Convert.ToChar(_arrays[_ptr]);
        if (_verbose)
        {
            Console.WriteLine($"Output {output}");
        }
        else
        {
            Console.Write(output);
        }
    }

    private void OutputInt()
    {
        if (IsError()) return;

        var output = _arrays[_ptr];
        if (_verbose)
        {
            Console.WriteLine($"Output {output}");
        }
        else
        {
            Console.Write(output);
        }
    }

    private void Loop()
    {
        _loop.Push((_segmentPtr, _ptr));
        
        if (!_verbose) return;
        Console.WriteLine($"Condition Cell [{_ptr}] = {_arrays[_ptr]}");
    }

    private void Goto()
    {
        if (_loop.Count == 0) return;

        var check = _loop.Peek();
        if (_arrays[check.cellPtr] == 0)
        {
            _loop.Pop();
            if (!_verbose) return;
            Console.WriteLine("Stop Loop");
            return;
        }
        
        _segmentPtr = check.segmentPtr;
    }

    private bool IsError()
    {
        return IsArrayInitialized() || IsOutOfBound();
    }

    private bool IsArrayInitialized()
    {
        if (_arrays.Length > 0) return false;
        
        Console.WriteLine("Error: Array size is 0.");
        return true;
    }
    
    private bool IsOutOfBound()
    {
        if (!(_ptr < 0 || _ptr >= _arrays.Length)) return false;
        
        _hasError = true;
        
        Console.WriteLine("Error: Pointer out of bound.");
        
        return true;
    }

    private void LogParse(string chunk, TokenType type)
    {
        if (!_verbose) return;
        
        Console.WriteLine($"Parse [{chunk}] to {type}.");
    }

    private void LogArray()
    {
        StringBuilder builderCell = new();
        StringBuilder builderValue = new();

        builderCell.Append("Cell  : ");
        builderValue.Append("Value : ");

        for (int i = 0; i < _arrays.Length; i++)
        {
            builderCell.Append($"[{i}]");
            builderValue.Append($"[{_arrays[i]}]");
        }

        Console.WriteLine();
        Console.WriteLine(builderCell.ToString());
        Console.WriteLine(builderValue.ToString());
    }
}