using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Add Post");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            Console.WriteLine(" 0) Go Back");

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
            throw new NotImplementedException();
        }
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Publication Date: Ex. (1/1/1111 12:00:00 AM) ");
            post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());

            Console.Write("Author: ");
            AuthorRepository author = new AuthorRepository(_connectionString);
            List<Author> authorList = author.GetAll();

            int indexNum = 1;

            foreach (Author eachObject in authorList)
            {
                Console.WriteLine($"{indexNum} {eachObject.FullName}");
                indexNum++;
            }
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                Console.WriteLine(authorList[choice - 1].FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
            }

            _postRepository.Insert(post);
        }

        private void Edit()
        {
            throw new NotImplementedException();
        }

        private void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
