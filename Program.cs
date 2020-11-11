using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList2
{
    class Program
    {
        /* CLASS: Todo
         * PURPOSE: A todo task
        */
        class Todo
        {
            public string date;
            public string status;
            public string title;
            public Todo(string arg1, string arg2, string arg3)
            {
                date = arg1;
                status = arg2;
                title = arg3;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till TodoList!");
            Console.WriteLine("Kommandon: quit, load 'filename', save, save 'filename', visa, move, add, delete, set");

            // INITIAL DECLARATIONS
            List<Todo> TodoList = new List<Todo>();
            bool check = false;
            string command;
            string[] commandWord;
            string path = "";

            do
            {
                Console.Write("> ");
                command = Console.ReadLine();
                commandWord = command.Split(' ');

                switch (commandWord[0]){
                    case "quit":
                        bool Q = true;
                        Console.WriteLine("Vill du spara innan du stänger av? (j/n)");
                        // Ser till så att användaren anger korrekt svar
                        do
                        {
                            Q = SaveQ(path, TodoList, Q);
                        } while (Q);
                        
                        check = true;
                        Console.WriteLine("Hejdå!");
                        break;
                    case "load":
                        bool L = true;
                        Console.WriteLine("Vill du spara innan du laddar en ny fil? (j/n)");
                        // Ser till så att användaren anger korrekt svar
                        do
                        {
                            L = SaveQ(path, TodoList, L);
                        } while (L);

                        // TBD: Check if path is valid
                        path = commandWord[1];
                        TodoList = Load(path);
                        Console.WriteLine("Lista Laddad");
                        break;
                    case "save":
                        // Checks if user assigned a new path or not
                        if(commandWord.Length == 1)
                        {
                            Save(TodoList, path);
                        }
                        else
                        {
                            Save(TodoList, commandWord[1]);
                        }
                        Console.WriteLine("Lista Sparad");
                        break;
                    case "visa":
                        Console.WriteLine("N  datum  S rubrik\n--------------------------------------------");
                        for(int i = 0; i < TodoList.Count; i++)
                        {
                            string date = TodoList[i].date;
                            string status = TodoList[i].status;
                            string title = TodoList[i].title;
                            
                            if(commandWord.Length == 1)
                            {
                                if (status != "*")
                                {
                                    Print(date, status, title, i);
                                }
                            }
                            else if(commandWord.Length == 2)
                            {
                                if (commandWord[1] == "klara")
                                {
                                    if (status == "*")
                                    {
                                        Print(date, status, title, i);
                                    }
                                }
                                else if(commandWord[1] == "allt")
                                {
                                    Print(date, status, title, i);
                                }
                            }
                        }
                        Console.WriteLine("--------------------------------------------");
                        break;
                    case "move":
                        // TBD: Check if position is a valid position
                        TodoList = Move(commandWord, TodoList);
                        break;
                    case "delete":
                        // TBD: Check if position is a valid position
                        int position = int.Parse(commandWord[1]);
                        TodoList.RemoveAt(position - 1);
                        break;
                    case "add":
                        string tmp = "";
                        // Adds each word after index 1 from commandWord into a whole string
                        for(int i = 2; i < commandWord.Length; i++)
                        {
                            tmp += commandWord[i] + " ";
                        }
                        Todo nyUppgift = new Todo(commandWord[1], "v", tmp);
                        TodoList.Add(nyUppgift);
                        break;
                    case "set":
                        // TBD: Check if position is a valid position
                        int pos = int.Parse(commandWord[1]);
                        if(commandWord[2] == "avklarad")
                        {
                            TodoList[pos - 1].status = "*";
                        } else if(commandWord[2] == "väntande")
                        {
                            TodoList[pos - 1].status = "v";
                        } else if(commandWord[2] == "pågående")
                        {
                            TodoList[pos - 1].status = "P";
                        }
                        else
                        {
                            Console.WriteLine("Ange en korrekt status!");
                        }
                        break;
                    default:
                        Console.WriteLine("Okänt kommando! Försök igen");
                        break;
                }
            } while (!check);
        }

        // METHODS

        /* METHOD: Save
         * PURPOSE: Save a todo list to a file in given path
         * PARAMETERS: List<Todo> Todo - The list to be saved
         *             string path - the path to save the list at
         * RETURN VALUE: None
         */
        static void Save(List<Todo> Todo, string path)
        {
            List<String> allLines = new List<string>();
            string line;
            for (int i = 0; i < Todo.Count; i++)
            {
                line = Todo[i].date + "#" + Todo[i].status + "#" + Todo[i].title;
                allLines.Add(line);
            }
            File.WriteAllLines(path, allLines);
        }

        /* METHOD: Load
         * PURPOSE: Loads a todo list from a file in given path
         * PARAMETERS: string path - the path to load from
         * RETURN VALUE: The todo list from the file that was loaded
         */
        static List<Todo> Load(string path)
        {
            List<Todo> TodoList = new List<Todo>();
            string[] linesFromFile;
            linesFromFile = File.ReadAllLines(path);
            for(int i = 0; i < linesFromFile.Length; i++)
            {
                string[] splittedLine = linesFromFile[i].Split('#');
                TodoList.Add(new Todo(splittedLine[0], splittedLine[1], splittedLine[2]));
            }
            return TodoList;
        }

        /* METHOD: Move
         * PURPOSE: Moves a task up or down in the list
         * PARAMETERS: string[] commandWord - Used to check what index that will get moved and in what direction
         *             List<Todo> todoList - The list with the task that will get moved
         * RETURN VALUE: The new list with updated positions for the tasks
         */
        static List<Todo> Move(string[] commandWord, List<Todo> todoList)
        {
            int oldPos = int.Parse(commandWord[1]) - 1;
            if (commandWord[2] == "up")
            {
                Todo temp = todoList[oldPos];
                todoList.RemoveAt(oldPos);
                todoList.Insert(oldPos - 1, temp);
            }
            else if (commandWord[2] == "down")
            {
                Todo temp = todoList[oldPos];
                todoList.RemoveAt(oldPos);
                todoList.Insert(oldPos + 1, temp);
            }
            return todoList;
        }

        /* METHOD: Print
         * PURPOSE: Prints a todo list
         * PARAMETERS: string date - the date of the task
         *             string status - the status of the task
         *             string title - the title of the task
         *             int i - the position of the task
         * RETURN VALUE: None
         */
        static void Print(string date, string status, string title, int i)
        {
            // Idea found from https://stackoverflow.com/questions/644017/net-format-a-string-with-fixed-spaces
            // To align text with center for the date
            string fullText = i + 1 + ": "
                            + String.Format("{0,-6}", String.Format("{0," + ((6 + date.Length) / 2).ToString() + "}", date))
                            + " " + status
                            + " " + title;
            Console.WriteLine(fullText);
        }

        /* METHOD: SaveQ
         * PURPOSE: Checks wether or not the user wants to save their changes before performing an action
         * PARAMETERS: string path - the path to save the list to if the user wants to save
         *             List<Todo> todoList - the list to save if the user wants to save it
         *             bool Q - Checks if the user entered a valid letter or not
         * RETURN VALUE: The bool to check if the program has to ask the user again to enter a letter in case the previous was invalid
         */
        static bool SaveQ(string path, List<Todo> todoList, bool Q)
        {
            string dummy;
            dummy = Console.ReadLine();
            if (dummy == "j")
            {
                Save(todoList, path);
                Q = false;
            }
            else if (dummy == "n")
            {
                Q = false;
            }
            else
            {
                Console.WriteLine("Svara med antingen ett 'j' eller 'n'");
            }
            return Q;
        }
    }
}