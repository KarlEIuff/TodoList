﻿using System;
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
            Console.WriteLine("Kommandon: quit, load 'filename', save, save 'filename'");

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
