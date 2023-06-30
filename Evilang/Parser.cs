﻿using System.Text;

namespace Evilang;

public class Parser
{
    private int _ptr = 0;
    private int _size = 1;
    private bool _hasError;
    private byte[] _arrays;
    private readonly bool _verbose;
    
    private readonly Stack<int> _loop = new ();
    
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

    public void Parse(string code)
    {
        var segments = code.Split(" ");

        foreach (var segment in segments)
        {
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
        
        // LogArray();
    }

    private void Execute(string chunk)
    {
        if (!_keywords.TryGetValue(chunk, out var tokenType))
        {
            
            return;
        }
        
        // LogParse(chunk, tokenType);
        
        switch (tokenType)
        {
            case TokenType.Initialize: SetArraySize();break;
            case TokenType.ShiftLeft: ShiftLeft(); break;
            case TokenType.ShiftRight: ShiftRight(); break;
            case TokenType.Increment: Increment(); break;
            case TokenType.Decrement: Decrement(); break;
            case TokenType.Input: break;
            case TokenType.Output: Ouput(); break;
            case TokenType.OutputInt: OutputInt(); break;
            case TokenType.Loop: break;
            case TokenType.Goto: break;
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

    private void Ouput()
    {
        if (IsError()) return;

        Console.Write(Convert.ToChar(_arrays[_ptr]));
    }

    private void OutputInt()
    {
        if (IsError()) return;

        Console.Write(_arrays[_ptr]);
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