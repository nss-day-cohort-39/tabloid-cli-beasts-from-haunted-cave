using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        private JournalRepository journalRepo;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
            journalRepo = new JournalRepository(_connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("\n# JOURNAL MENU");
            Console.WriteLine("1) List Journals");
            Console.WriteLine("2) Add Journal");
            Console.WriteLine("3) Edit Journal");
            Console.WriteLine("4) Remove Journal");
            Console.WriteLine("0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            
            Console.WriteLine("\nJOURNAL ENTRIES");
            foreach (Journal journal in journals)
            {
                Console.WriteLine();
                Console.WriteLine($"Title: {journal.Title}");
                Console.WriteLine($"Content: {journal.Content}");
                Console.WriteLine($"Date & Time: {journal.CreateDateTime}");
            }
        }

        private void Add()
        {
            Console.WriteLine("\nNew Journal");
            Journal journal = new Journal();

            Console.Write("\nTitle: ");
            journal.Title = Console.ReadLine();

            Console.Write("\nContent: ");
            journal.Content = Console.ReadLine();

            Console.Write("\nCreateDateTime: ");
            journal.CreateDateTime = Convert.ToDateTime(Console.ReadLine());

            _journalRepository.Insert(journal);
        }

        private void Edit()
        {
            // 1. loop list of jornals
            // 2. display all journals from index 
            // 2.5 take input from user
            // 3. choose  from #
            // 4. check = int
            // 5. get journal that matches chosen journal 
            // 6. update empty Journal bucket = chosenJournal
            // 7. Call Update(updatedJournal)
            //
            // Later... Empty or Return does NOT change existing Journal Entry. 
            // Also do this on Edit Post

            // Empty bucket
            Journal journal = new Journal();

            List<Journal> journalEntries = journalRepo.GetAll();

            Console.WriteLine("\nJOURNAL ENTRIES - EDIT:");
            for (int i = 0; i < journalEntries.Count; i++)
            {
                Console.WriteLine($"\nEntry {i + 1} \n{journalEntries[i].Title} \n" +
                    $"Date: {journalEntries[i].CreateDateTime} \n" +
                    $"Content: {journalEntries[i].Content} ");
            }

            Console.Write("\nSelect Journal entry to edit > ");
            int userChoice = -1;
            bool isUserChoice = int.TryParse(Console.ReadLine(), out userChoice);
            if (isUserChoice)
            {
                journalEntries[userChoice - 1].Id = userChoice;

                Console.Write("\nEnter Journal Title > ");
                string userTitleChoice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userTitleChoice))
                {
                    journalEntries[userChoice - 1].Title = userTitleChoice;

                    Console.Write("\nEnter Journal Content > ");
                    string userContentChoice = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(userContentChoice))
                    {
                        journalEntries[userChoice - 1].Content = userContentChoice;

                        Console.Write("\nEnter Date & Time > ");
                        DateTime userCreateDateTimeChoice = Convert.ToDateTime(Console.ReadLine());
                        
                        if (userCreateDateTimeChoice != null)
                        {
                            journalEntries[userChoice - 1].CreateDateTime = userCreateDateTimeChoice;
                            //
                            journal.Id = journalEntries[userChoice - 1].Id;
                            journal.Title = journalEntries[userChoice - 1].Title;
                            journal.Content = journalEntries[userChoice - 1].Content;
                            journal.CreateDateTime = journalEntries[userChoice - 1].CreateDateTime;
                            //
                            journalRepo.Update(journal);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Date & Time entered.");
                        }
                    }

                    else
                    {
                        Console.WriteLine("Incorrect Content.");
                    }
                }

                else
                {
                    Console.WriteLine("Must be greater than Zero");
                }
            }
        }

        private void Remove()
        {
            {
                Journal journalToDelete = Choose("Which journal would you like to remove?");
                if (journalToDelete != null)
                {
                    _journalRepository.Delete(journalToDelete.Id);
                }
            } 
        }

        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Journal> journals = _journalRepository.GetAll();

            for (int i = 0; i < journals.Count; i++)
            { 
                Journal journal = journals[i];
                Console.WriteLine($" {i + 1}) {journal.Title} - {journal.Content} - {journal.CreateDateTime}");
            }
            Console.Write("\n> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return journals[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
    }
}
