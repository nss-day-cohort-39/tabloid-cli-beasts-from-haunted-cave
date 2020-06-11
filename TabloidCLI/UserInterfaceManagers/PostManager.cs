﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
            List<Post> posts = _postRepository.GetAll();
            for (int i = 0; i < posts.Count; i++)
            {
                Console.WriteLine(@$"{i+1}).Title: {posts[i].Title} Url: {posts[i].Url}");
            }
        }
        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
            post.Url = Console.ReadLine();

            Console.Write("Publication Date: Ex. (1/1/1111 12:00:00 AM) > ");
            post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Author: ");
            AuthorRepository author = new AuthorRepository(_connectionString);
            List<Author> authorList = author.GetAll();

            for(int i=0; i<authorList.Count; i++)
            {
                Console.WriteLine($"{i+1} {authorList[i].FullName}");
            }
           
            int usrAuthorChoice = -10;
            bool userChoice = int.TryParse(Console.ReadLine(), out usrAuthorChoice);
            if(userChoice)
            {
                Console.WriteLine($"Author {authorList[usrAuthorChoice -1].FullName}");
                Console.WriteLine($"Your Author choice {usrAuthorChoice}");
                post.Author = authorList[usrAuthorChoice-1];

                BlogRepository blogRepo = new BlogRepository(_connectionString);
                List<Blog> blogs = blogRepo.GetAll();
                for (int i=0;i< authorList.Count; i++)
                {
                    Console.WriteLine($"{i+1} {blogs[i].Title}");
                }
                Console.WriteLine("Choose Blog");
                int blogChoice = -1;
                bool userBlogChoice = int.TryParse(Console.ReadLine(), out blogChoice);
                if (userBlogChoice)
                {
                    Console.WriteLine($"Your Blog choice {blogs[blogChoice - 1].Title}");
                    post.Blog = blogs[blogChoice - 1];
                    _postRepository.Insert(post);

                }
                else
                {
                    Console.WriteLine("Invalid choice");
                }

            }
            else
            {
                Console.WriteLine("Invalid Selection");
            }

        }

        private void Edit()
        {
            throw new NotImplementedException();
        }
        
        private void Remove()
        {
            Console.WriteLine("Please choose post");
            List<Post> posts = _postRepository.GetAll();
            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine(@$" {i + 1}) [Title:-{post.Title}
                                         Url: {post.Url}
                                         Published Date:- {post.PublishDateTime}
                                         Author Id:- {post.Id}
                                         Blog Id:- {post.Id}]");
            }
            int userChoice = -1;
            bool isUserChoice = int.TryParse(Console.ReadLine(), out userChoice);
            if (isUserChoice)
            {
                Console.WriteLine($"Your choice is {userChoice}");
                _postRepository.Delete(posts[userChoice -1].Id);
            }
            else
            {
                Console.WriteLine("Invalid Selection");
            }
        }
    }
}
