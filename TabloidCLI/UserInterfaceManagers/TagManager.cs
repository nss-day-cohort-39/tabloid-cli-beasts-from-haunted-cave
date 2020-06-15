using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;
        private string _connectionString;

        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("\nTAG MENU");
            Console.WriteLine("1) List Tags");
            Console.WriteLine("2) Add Tag");
            Console.WriteLine("3) Edit Tag");
            Console.WriteLine("4) Remove Tag");
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
            List<Tag> tags = _tagRepository.GetAll();
            for (int i = 0; i < tags.Count; i++)
            {
                Console.WriteLine($"Id: {tags[i].Id} Name: {tags[i].Name}");
            }
        }

        private void Add()
        {
            Tag tag = new Tag();
            Console.Write("Enter tag name> ");
            string tagName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tagName))
            {
                tag.Name = tagName;
                _tagRepository.Insert(tag);
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }

        private void Edit()
        {
            Tag tag = new Tag();
            List<Tag> tags = _tagRepository.GetAll();
            for (int i = 0; i < tags.Count; i++)
            {
                Console.WriteLine($"{i+1}) Id: {tags[i].Id} Name: {tags[i].Name}");
            }
            Console.Write("Please choose tag> ");
            int userTagChoice = -1;
            bool isUserTagChoice = int.TryParse(Console.ReadLine(), out userTagChoice);
            if (isUserTagChoice)
            {
                Console.Write("Enter tag name>");
                string tagNameToEdit = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(tagNameToEdit))
                {
                    tags[userTagChoice -1].Name = tagNameToEdit;
                    tag.Id = tags[userTagChoice - 1].Id;
                    tag.Name = tags[userTagChoice - 1].Name;
                    _tagRepository.Update(tag);
                    Console.WriteLine("successfully updated");
                }
                else
                {
                    Console.WriteLine("Invalid entry");
                }
            }
            else
            {
                Console.WriteLine("Invalid entry");
            }
        }
        private void Remove()
        {
            List<Tag> tags = _tagRepository.GetAll();
            for (int i = 0; i < tags.Count; i++)
            {
                Console.WriteLine($"{i + 1}) Id: {tags[i].Id} Name: {tags[i].Name}");
            }
            Console.Write("Please choose tag> ");
            int userTagChoice = -1;
            bool isUserTagChoice = int.TryParse(Console.ReadLine(), out userTagChoice);
            if (isUserTagChoice)
            {
                try
                {
                    _tagRepository.Delete(tags[userTagChoice - 1].Id);
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Please pick number between {1} and {tags.Count}");
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine($"Please pick number between {1} and {tags.Count}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }

        }
    }
}
