using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    class BlogRepository : DatabaseConnector
    {
        public BlogRepository(string connectionString) : base(connectionString) { }
        public List<Blog> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Title,
                                               Url
                                          FROM Blog";

                    List<Blog> blogs = new List<Blog>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Blog newBlog = new Blog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("Url"))
                        };
                        blogs.Add(newBlog);
                    }

                    reader.Close();

                    return blogs;
                }
            }
        }
    }
}
