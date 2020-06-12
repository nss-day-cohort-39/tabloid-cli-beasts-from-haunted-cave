using System;
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
        private BlogRepository _blogRepository;
        private AuthorRepository _authorRepository;
        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
            _blogRepository = new BlogRepository(_connectionString);
            _authorRepository = new AuthorRepository(_connectionString);
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
        private Blog BlogChoice(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Blog:";
            }

            Console.WriteLine(prompt);
            
            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title}");
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

        private Author AuthorChoice(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);
            List<Author> authors = _authorRepository.GetAll();

            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($" {i + 1}) {author.FullName}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return authors[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
       
        private void Edit()
        {
            Post post = new Post();
            List<Post> posts = _postRepository.GetAll();
            for (int i = 0; i < posts.Count; i++)
            {
                Console.WriteLine(@$"{i+1}). Title: {posts[i].Title} 
                    URL: {posts[i].Url} 
                    Publish date time: {posts[i].PublishDateTime} 
                    Author Id : {posts[i].Author.Id} 
                    Blog Id {posts[i].Blog.Id}");
            }
            Console.Write("Enter your choice >");
            int userChoice = -1; 
            bool isUserChoice = int.TryParse(Console.ReadLine(), out userChoice);
            if (isUserChoice)
            {
                Console.WriteLine($"Your choice {posts[userChoice -1].Title} by {posts[userChoice - 1].Author.Id}");
                Console.Write("Edit title >");
                
                string userTitleChoice = Console.ReadLine();
                if (userTitleChoice != "" || userTitleChoice != null)
                {
                    posts[userChoice - 1].Title = userTitleChoice;
                    
                    Console.Write("Edit Url >");
                    string userUrlChoice = Console.ReadLine();
                    if (userUrlChoice != "" || userUrlChoice != null)
                    {
                        posts[userChoice - 1].Url = userUrlChoice;
                        Console.Write("Edit Publish date time >");
                        DateTime userPublishDateTimeChoice = Convert.ToDateTime(Console.ReadLine());
                        if (userPublishDateTimeChoice != null)
                        {
                            posts[userChoice - 1].PublishDateTime = userPublishDateTimeChoice;
                            post.Id = posts[userChoice -1].Id;
                            post.Title = posts[userChoice - 1].Title;
                            post.Url = posts[userChoice - 1].Url;
                            post.PublishDateTime = posts[userChoice - 1].PublishDateTime;
                            post.Author = AuthorChoice("choose author");
                            post.Blog = BlogChoice("choose blog");
                            _postRepository.Update(post);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please enter title");
                }

            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
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
                    Published Date: {post.PublishDateTime}
                    Author Id: {post.Id}
                    Blog Id: {post.Id}]");
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
