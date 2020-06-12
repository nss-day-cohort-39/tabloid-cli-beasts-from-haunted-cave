using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT post.Id,
                                        post.Title,
                                        post.Url,
                                        post.PublishDateTime,
                                        post.AuthorId AS AuthorId,
                                        post.BlogId AS BlogId,
                                        author.FirstName,
                                        author.LastName,
                                        author.Bio,
                                        blog.Title AS BlogTitle,
                                        blog.Url AS BlogUrl
                                        FROM Post post
                                        LEFT JOIN Author author ON author.Id = post.AuthorId
                                        LEFT JOIN Blog blog ON blog.Id = post.BlogId";

                    List<Post> posts = new List<Post>();
                                        
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Post newPost = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio"))
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl"))
                            }                            
                        };
                        posts.Add(newPost);
                    }

                    reader.Close();
                    return posts;
                }
            }
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT post.id,
                                               post.Title As PostTitle,
                                               post.URL AS PostUrl,
                                               post.PublishDateTime,
                                               post.AuthorId,
                                               post.BlogId,
                                               author.FirstName,
                                               author.LastName,
                                               author.Bio,
                                               blog.Title AS BlogTitle,
                                               blog.URL AS BlogUrl
                                          FROM Post post 
                                               LEFT JOIN Author author on post.AuthorId = author.Id
                                               LEFT JOIN Blog blog on post.BlogId = blog.Id 
                                         WHERE post.AuthorId = @id";
                    cmd.Parameters.AddWithValue("@authorId", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Post post = null;
                    if (reader.Read())
                    {
                        post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                    }

                    reader.Close();

                    return post;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }
        
        public void Insert(Post post)
        {
            
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post (Title, Url, PublishDateTime, AuthorId, BlogId)
                                                       OUTPUT INSERTED.Id
                                                       VALUES (@title, @url, @publishDateTime, @authorId, @blogId)";
                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@authorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@blogId", post.Blog.Id);

                    int PostId = (int)cmd.ExecuteScalar();
                    post.Id = PostId;
                }
            }
        }

        public void Update(Post post)
        {
             using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Post
                        SET Title = @postTitle,
                        URL = @postUrl,
                        PublishDateTime = @publishDateTime,
                        AuthorId = @authorId,
                        BlogId = @blogId
                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@postTitle", post.Title);
                    cmd.Parameters.AddWithValue("@postUrl", post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@authorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@blogId", post.Blog.Id);
                    cmd.ExecuteNonQuery();
                }
                                
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Post WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
