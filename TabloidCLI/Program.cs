using System;
using System.Collections.Generic;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Pick a Color:");
            Console.WriteLine("1) DarkYellow");
            Console.WriteLine("2) Blue");
            Console.WriteLine("3) DarkCyan");
            Console.WriteLine("4) Magenta");
            var userchoice = Console.ReadLine();
            try
            {

                switch (userchoice)
                {
                    case "1":
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        break;
                    case "2":
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        break;
                    case "3":
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        break;
                    case "4":
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Clear();
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        break;
                }
            }catch(IndexOutOfRangeException ex)
            {
                Console.WriteLine("Invalid choice");
            }

            // MainMenuManager implements the IUserInterfaceManager interface
            IUserInterfaceManager ui = new MainMenuManager();
            while (ui != null)
            {
                // Each call to Execute will return the next IUserInterfaceManager we should execute
                // When it returns null, we should exit the program;
                ui = ui.Execute();
            }
           
        
        }
    }
}
