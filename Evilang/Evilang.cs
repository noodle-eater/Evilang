// See https://aka.ms/new-console-template for more information

using Evilang;

Run();

Parser parser;

void Run()
{
    if (args.Length > 0)
    {
        parser = new(false);
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(args[0]));
        parser.Parse(BitConverter.ToString(bytes));
    }
    else
    {
        Console.WriteLine("Evilang REPL by Noodle Eater\n");
        ShowDocs();
        Console.WriteLine();
        
        Console.WriteLine("REPL");
        parser = new(false);
    
        while (true)
        {
            Console.Write(">> ");
            string? input = Console.ReadLine();

            input = input?.ToLower();
        
            if (input != null && IsReplCommand(input)) continue;

            if (input == "exit") return;
        
            if (input != null) parser.Parse(input);
        }
    }
}

void ShowDocs()
{
    Console.WriteLine("Docs:");
    Console.WriteLine("1. evil  => Set array size by power of 2.");
    Console.WriteLine("2. e     => Shift pointer to left.");
    Console.WriteLine("3. v     => Shift pointer to right.");
    Console.WriteLine("4. i     => Add value in current cell by 1.");
    Console.WriteLine("5. l     => Reduce value in current cell by 1.");
    Console.WriteLine("6. vi    => Input a number in current cell.");
    Console.WriteLine("7. ev    => Print the value in cell as char.");
    Console.WriteLine("8. evi   => Print the value in cell as int.");
    Console.WriteLine("9. il    => Start loop until current cell value 0.");
    Console.WriteLine("10. el   => End loop.");
}

bool IsReplCommand(string input)
{
    if (input == "docs" || input == "help")
    {
        ShowDocs(); 
        return true;
    }

    if (input == "verbose 1")
    {
        parser.SetVerbose(true);
        Console.WriteLine("Set Verbose to true");
        return true;
    }

    if (input == "verbose 0")
    {
        parser.SetVerbose(false);
        Console.WriteLine("Set Verbose to false");
        return true;
    }

    if (input == "reset")
    {
        parser = new Parser(false);
        Console.WriteLine("Reset current parser");
        return true;
    }

    return false;
}