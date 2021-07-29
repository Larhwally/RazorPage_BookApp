using MyBookApp.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509.SigI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBookApp.Data
{
    public class BookContext
    {
        public string ConnectionString { get; set; }

        //Constructor of the parent class using and attribute of the class COnnectionString setting to its own parameter connection
        public BookContext(string connection)
        {
            this.ConnectionString = connection;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public async Task <List<book>> GetBooks()
        {
            List<book> books = new List<book>();
            string s = "AVAILABLE";

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM tbl_books WHERE status = " + "\"" + s + "\"", connection);

                await using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new book
                    {
                        id = Convert.ToInt32(reader["id"]),
                        bookTitle = reader["bookTitle"].ToString(),
                        authorName = reader["authorName"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        publishYear = Convert.ToDateTime(reader["publishYear"]),
                        createdBy = reader["createdBy"].ToString(),
                        createDate = Convert.ToDateTime(reader["createDate"]),
                        status = reader["status"].ToString(),
                    });
                }
                
            }
            
            return books;
        }


        //This method is used to fetch records of borrowed books from the db
        public async Task<List<book>> GetBorrowedBooks()
        {
            List<book> books = new List<book>();
            string s = "BORROWED";

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT tbl_borrowedbooks.id, bookId, bookTitle, authorName, ISBN, publishYear, tbl_borrowedbooks.createdBy, tbl_borrowedbooks.createDate, tbl_borrowedbooks.status FROM tbl_borrowedbooks "
+ "inner join tbl_books ON tbl_books.id = tbl_borrowedbooks.bookId  WHERE tbl_borrowedbooks.status = " + "\"" + s + "\"";
                MySqlCommand command = new MySqlCommand(query , connection);

                await using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new book
                    {
                        id = Convert.ToInt32(reader["bookId"]),
                        bookTitle = reader["bookTitle"].ToString(),
                        authorName = reader["authorName"].ToString(),
                        ISBN = reader["ISBN"].ToString(),
                        publishYear = Convert.ToDateTime(reader["publishYear"]),
                        createdBy = reader["createdBy"].ToString(),
                        createDate = Convert.ToDateTime(reader["createDate"]),
                        status = reader["status"].ToString(),
                    });
                }

            }

            return books;
        }


        //This method insert new book record in the db
        public async Task<bool> PostBook(book bookrec)
        {
            var pubYear = DateTime.Now;
            string bookStatus = "AVAILABLE";

            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("INSERT INTO tbl_books(bookTitle, authorName, ISBN, publishYear, createDate, createdBy, status) VALUES(@bookTitle, @authorName, @ISBN, @publishYear, " +
                "@createDate, @createdBy, @status)", connection);

            command.Parameters.AddWithValue("@bookTitle", bookrec.bookTitle);
            command.Parameters.AddWithValue("@authorName", bookrec.authorName);
            command.Parameters.AddWithValue("@ISBN", bookrec.ISBN);
            command.Parameters.AddWithValue("@publishYear", bookrec.publishYear);
            command.Parameters.AddWithValue("@createDate", pubYear);
            command.Parameters.AddWithValue("@status", bookStatus);
            command.Parameters.AddWithValue("@createdBy", bookrec.createdBy);

            int n = command.ExecuteNonQuery();
            return n > 0;
        }

        //Insert a new record of a book and immediately grabbing the ID(primary key) set for the book
        public async Task<long> PostNewBook(book bookrec)
        {
            var pubYear = DateTime.Now;
            string bookStatus = "AVAILABLE";

            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("INSERT INTO tbl_books(bookTitle, authorName, ISBN, publishYear, createDate, createdBy, status) VALUES(@bookTitle, @authorName, @ISBN, @publishYear, " +
                "@createDate, @createdBy, @status)", connection);

            command.Parameters.AddWithValue("@bookTitle", bookrec.bookTitle);
            command.Parameters.AddWithValue("@authorName", bookrec.authorName);
            command.Parameters.AddWithValue("@ISBN", bookrec.ISBN);
            command.Parameters.AddWithValue("@publishYear", bookrec.publishYear);
            command.Parameters.AddWithValue("@createDate", pubYear);
            command.Parameters.AddWithValue("@status", bookStatus);
            command.Parameters.AddWithValue("@createdBy", bookrec.createdBy);

            //int n = command.ExecuteNonQuery();
            command.ExecuteScalar();
            long id = command.LastInsertedId;
            return id;
        }


        // A method that gets a record of a book from the database by its ID
        public async Task<book> GetBookById(int id)
        {
            book singlebook = new book();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM tbl_books WHERE id = " + id, connection);
                command.Parameters.AddWithValue("@id", id);

                await using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    singlebook.id = Convert.ToInt32(reader["id"]);
                    singlebook.bookTitle = reader["bookTitle"].ToString();
                    singlebook.authorName = reader["authorNAme"].ToString();
                    singlebook.ISBN = reader["ISBN"].ToString();
                    singlebook.publishYear = Convert.ToDateTime(reader["publishYear"]);
                    singlebook.createdBy = reader["createdBy"].ToString();
                    singlebook.createDate = Convert.ToDateTime(reader["createDate"]);
                }
            }

            return singlebook;
        }


        //A method to update a book record in the dabase
        public async Task<bool> UpdateBook(book book)
        {
            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("UPDATE tbl_books SET bookTitle = @bookTitle, authorName = @authorName, status = @status WHERE id = " + book.id, connection);

            command.Parameters.AddWithValue("@bookTitle", book.bookTitle);
            command.Parameters.AddWithValue("@authorName", book.authorName);
            command.Parameters.AddWithValue("@status", book.status);

            int n = command.ExecuteNonQuery();
            return n > 0;

        }


        //An update method for updating borrowed book in the db
        public async Task<bool> UpdateBorrowedBook(string status, int id)
        {
            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("UPDATE tbl_borrowedbooks SET status = @status WHERE bookId = " + id, connection);

            //command.Parameters.AddWithValue("@bookTitle", book.bookTitle);
            //command.Parameters.AddWithValue("@authorName", book.authorName);
            command.Parameters.AddWithValue("@status", status);

            int n = command.ExecuteNonQuery();
            return n > 0;

        }


        //A method for search
        public async Task<List<book>> SearchBook(string searchItem)
        {
            List<book> searchBooks = new List<book>();
            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM tbl_books WHERE bookTitle OR authorName LIKE " + "\"%" + searchItem + "%\"", connection);
            command.Parameters.AddWithValue("bookTitle", searchItem);
            command.Parameters.AddWithValue("authorName", searchItem);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                searchBooks.Add(new book
                {
                    id = Convert.ToInt32(reader["id"]),
                    bookTitle = reader["bookTitle"].ToString(),
                    authorName = reader["authorNAme"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    publishYear = Convert.ToDateTime(reader["publishYear"]),
                    createdBy = reader["createdBy"].ToString(),
                    createDate = Convert.ToDateTime(reader["createDate"]),
                });
                
            }
            return searchBooks;
        }

        //A method for delete
        public async Task<bool> DeleteBook(int id)
        {
            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("DELETE FROM tbl_books WHERE id = " + id, connection);
            int n = command.ExecuteNonQuery();
            return n > 0;
        }

        //A method to insert bookId and book status to borrewed book table
        public async Task<bool> PostBorrowedBook(int id, string status)
        {
            string createdBy = "ADMIN";
            var createDate = DateTime.Now;
            await using MySqlConnection connection = GetConnection();
            connection.Open();

            MySqlCommand command = new MySqlCommand("INSERT INTO tbl_borrowedbooks(bookId, status, createdBy, createDate)VALUES(@bookId, @status, @createdBy, @createDate)", connection);
            command.Parameters.AddWithValue("@bookId", id);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@createdBy", createdBy);
            command.Parameters.AddWithValue("@createDate", createDate);

            int n = command.ExecuteNonQuery();
            return n > 0;

        }
        //A method to post file name in the DB
        public async Task<string> InsertFileName(string fileName, long bookId)
        {
            await using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("INSERT INTO tbl_file(fileName, bookId) VALUES(@fileName, @bookId)",connection);
            command.Parameters.AddWithValue("@fileName", fileName);
            command.Parameters.AddWithValue("@bookId", bookId);
            int n = command.ExecuteNonQuery();
            return fileName;

        }


      
        //write a method that queries the database to select an image by its name/string        Guid guid = new Guid();
        public bool CheckImage(string fileName)
        {
            using MySqlConnection connection = GetConnection();
            connection.Open();
            MySqlCommand command = new MySqlCommand("Select * from tbl_file where fileName = " + fileName);
            int n = command.ExecuteNonQuery();
            return n > 0; 
        }

        //A method to return file name by book id from tbl_book which matches a foreign key on tbl_files
        public string ReturnFileName(int id)
        {
            string fileName = "";
            using MySqlConnection connection = GetConnection();
            connection.Open();

            MySqlCommand command = new MySqlCommand("Select fileName from tbl_file where bookId = " + id, connection);
            command.Parameters.AddWithValue("bookId", id);
            MySqlDataReader reader = command.ExecuteReader();
            bool hasRow = reader.HasRows;
            if (reader.Read())
            {
                fileName = reader["fileName"].ToString();
            }
            return fileName;
        }





    }
}
