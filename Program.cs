using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till TodoList!");
            Console.WriteLine("Kommandon: quit, load 'filename', save, save 'filename'");

            bool check = false;
            string command;
            string[] commandWord;

            do
            {
                Console.Write("> ");
                command = Console.ReadLine();
                commandWord = command.Split(' ');

                switch (commandWord[0]){
                    case "quit":
                        check = true;
                        Console.WriteLine("Goodbye!");
                        break;
                    case "load":
                        
                        break;
                }

            } while (!check);
            

        }
    }
}
