// See https://aka.ms/new-console-template for more information

using Evilang;

Console.WriteLine("Evilang REPL by Noodle Eater\n");
ShowDocs();
Console.WriteLine();
Run();

void Run()
{
    Parser parser;

    if (args.Length > 0)
    {
        parser = new(false);
        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(args[0]));
        parser.Parse(BitConverter.ToString(bytes));
    }
    else
    {
        Console.WriteLine("REPL");
        parser = new(true);
    
        while (true)
        {
            Console.Write(">> ");
            string? code = Console.ReadLine();
        
            if (code == "docs" || code == "help")
            {
                ShowDocs(); 
                continue;
            }
        
            if (code != null) parser.Parse(code);
        }
    }
}

void ShowDocs()
{
    Console.WriteLine("Syntax:");
    Console.WriteLine("1. evil  => Set array size by power of 2.");
    Console.WriteLine("2. e     => Shift pointer to left.");
    Console.WriteLine("3. v     => Shift pointer to right.");
    Console.WriteLine("4. i     => Add value in current cell by 1.");
    Console.WriteLine("5. l     => Reduce value in current cell by 1.");
    Console.WriteLine("6. ev    => Print the value in cell as char.");
    Console.WriteLine("7. evi   => Print the value in cell as int.");
    Console.WriteLine("8. il    => Start loop until current cell value 0.");
    Console.WriteLine("9. el    => End loop.");

}