﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class BlogManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public BlogManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("\n# BLOG MENU");
            Console.WriteLine("1) List Blogs");
            Console.WriteLine("2) Add Blog");
            Console.WriteLine("3) Edit Blog");
            Console.WriteLine("4) Remove Blog");
            Console.WriteLine(" 5) Blog Details");
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
                case "5":
                    Blog blog = Choose();
                    if (blog == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new BlogDetailManager(this, _connectionString, blog.Id);
                    }
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog blog in blogs)
            {
                Console.WriteLine($"\n{blog.Title}");
                Console.WriteLine($"{blog.Url}");
               
            }
        }

        private void Add()
        {
            Console.WriteLine("New Blog");
            Blog blog = new Blog();

            Console.Write("Title: ");
            blog.Title = Console.ReadLine();

            Console.Write("Url: ");
            blog.Url = Console.ReadLine();


            _blogRepository.Insert(blog);
        }

        private void Edit()
        {
            Blog blog = new Blog();

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Console.WriteLine($"{i + 1} {blogs[i].Title} {blogs[i].Url}");
            }

            //

            Console.Write("Please choose a blog: > ");

            int userChoice = -1;

            bool isUserChoice = int.TryParse(Console.ReadLine(), out userChoice);

            if(isUserChoice)
            {
                blogs[userChoice - 1].Id = userChoice;

                Console.Write("Please enter Title: > ");
                string userTitleChoice = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userTitleChoice))
                {
                    blogs[userChoice - 1].Title = userTitleChoice;

                    Console.Write("Please enter a Url: > ");
                    string userUrlChoice = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(userUrlChoice))
                    {
                        blogs[userChoice - 1].Url = userUrlChoice;

                        blog.Id = blogs[userChoice - 1].Id;
                        blog.Title = blogs[userChoice - 1].Title;
                        blog.Url = blogs[userChoice - 1].Url;

                        _blogRepository.Update(blog);
                    }
                }  
            }
        }

        private void Remove()
        {
            {
                Blog blogToDelete = Choose("Which blog would you like to remove?");
                if (blogToDelete != null)
                {
                    _blogRepository.Delete(blogToDelete.Id);
                }
            }
        }

        private Blog Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title} - {blog.Url}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
    }
}
