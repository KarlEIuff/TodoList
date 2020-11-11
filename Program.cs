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
                        string dummy;
                        bool Q = true;
                        Console.WriteLine("Vill du spara innan du stänger av? (j/n)");
                        // Ser till så att användaren anger korrekt svar
                        do
                        {
                            dummy = Console.ReadLine();
                            if (dummy == "j")
                            {
                                Save(TodoList, path);
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
                            dummy = Console.ReadLine();
                            if (dummy == "j")
                            {
                                Save(TodoList, path);
                                L = false;
                            }
                            else if (dummy == "n")
                            {
                                L = false;
                            }
                            else
                            {
                                Console.WriteLine("Svara med antingen ett 'j' eller 'n'");
                            }
                        } while (L);
                        path = commandWord[1];
                        TodoList = Load(path);
                        Console.WriteLine("Lista Laddad");
                        break;
                    case "save":
                        // Kollar om användaren anger en ny sökväg, annars används den gamla som användes vid load
                        if(commandWord.Length == 1)
                        {
                            Save(TodoList, path);
                            Console.WriteLine("Lista Sparad");
                        }
                        else
                        {
                            Save(TodoList, commandWord[1]);
                            Console.WriteLine("Lista Sparad");
                        }
                        break;
                    default:
                        Console.WriteLine("Okänt kommando! Försök igen");
                        break;
                    case "visa":
                        Console.WriteLine("N  datum  S rubrik\n--------------------------------------------");

                        for(int i = 0; i < TodoList.Count; i++)
                        {
                            string date = TodoList[i].date;
                            string status = TodoList[i].status;
                            string title = TodoList[i].title;
                            // Line 120 idea found from https://stackoverflow.com/questions/644017/net-format-a-string-with-fixed-spaces
                            // To align text with center for the date
                            if(commandWord.Length == 1)
                            {
                                if (status != "*")
                                {
                                    string fullText = i + 1 + ": "
                                                  + String.Format("{0,-6}", String.Format("{0," + ((6 + date.Length) / 2).ToString() + "}", date))
                                                  + " " + status
                                                  + " " + title;
                                    Console.WriteLine(fullText);
                                }
                            }
                            else if(commandWord.Length == 2)
                            {
                                if (commandWord[1] == "klara")
                                {
                                    if (status == "*")
                                    {
                                        string fullText = i + 1 + ": "
                                                  + String.Format("{0,-6}", String.Format("{0," + ((6 + date.Length) / 2).ToString() + "}", date))
                                                  + " " + status
                                                  + " " + title;
                                        Console.WriteLine(fullText);
                                    }
                                }
                                else if(commandWord[1] == "allt")
                                {
                                    string fullText = i + 1 + ": "
                                                  + String.Format("{0,-6}", String.Format("{0," + ((6 + date.Length) / 2).ToString() + "}", date))
                                                  + " " + status
                                                  + " " + title;
                                    Console.WriteLine(fullText);
                                }
                            }
                            
                            
                        }
                        Console.WriteLine("--------------------------------------------");
                        break;
                    case "move":
                        int oldPos = int.Parse(commandWord[1]) - 1;
                        if(commandWord[2] == "up")
                        {
                            Todo temp = TodoList[oldPos];
                            TodoList.RemoveAt(oldPos);
                            TodoList.Insert(oldPos - 1, temp);
                        } else if(commandWord[2] == "down")
                        {
                            Todo temp = TodoList[oldPos];
                            TodoList.RemoveAt(oldPos);
                            TodoList.Insert(oldPos + 1, temp);
                        }
                        break;
                    case "delete":
                        int position = int.Parse(commandWord[1]);
                        TodoList.RemoveAt(position - 1);
                        break;
                    case "add":
                        string tmp = "";
                        for(int i = 2; i < commandWord.Length; i++)
                        {
                            tmp += commandWord[i] + " ";
                        }
                        Todo nyUppgift = new Todo(commandWord[1], "v", tmp);
                        TodoList.Add(nyUppgift);
                        break;
                    case "set":
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
                }

            } while (!check);
        }

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

    }
}
