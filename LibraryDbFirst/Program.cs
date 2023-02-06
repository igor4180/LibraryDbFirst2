using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryDbFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            //Authors author = new Authors { FirstName = "Michio", LastName = "Kaku" };
            //AddAuthor(author);
            //GetAllAuthors();
            Init();
            GetAllBooks();
        }

        static void AddAuthor(Authors author)
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {
                //db.Database.Log = Console.Write;
                //db.Database.Log = MyLogger.EFLog;
                Authors a = db.Authors.Where((x) => x.FirstName == author.FirstName && x.LastName == author.LastName).FirstOrDefault();
                if (a == null)
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    Console.WriteLine("New author added:" + author.LastName);
                }
            }
        }

        static void GetAllAuthors()
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {
                var au = db.Authors.ToList();
                foreach (var a in au)
                {
                    Console.WriteLine(a.FirstName + "  " + a.LastName);
                }
            }
        }

        static void GetAllBooks()
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {
                //db.Configuration.LazyLoadingEnabled = false;

                //var books = (from b in db.Books
                //             select b).ToList<Books>();
                //var author = (from b in db.Authors
                //              where (b.LastName.StartsWith("si"))
                //              select b).FirstOrDefault<Authors>();

                //db.Entry(author).Collection("Book").Load();
                var au = db.Books.OrderBy((x) => x.Title).ToList();

                foreach (var a in au)
                {
                    Console.WriteLine("Book: " + a.Title + " price: " + a.Price +
"  author: " + a.Authors.FirstName + "   " + a.Authors.LastName);
                }

            }
        }

        static void AddPublisher(Publishers publisher)
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {
                Publishers a = db.Publishers.Where((x) => x.PublisherName
                == publisher.PublisherName).FirstOrDefault();
                if (a == null)
                {
                    db.Publishers.Add(publisher);
                    db.SaveChanges();
                    Console.WriteLine("New publisher added:" + publisher.PublisherName);
                }
            }
        }

        static void AddBook(Books book)
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {
                Books a = db.Books.Where((x) => x.Title == book.Title).FirstOrDefault();
                if (a == null)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    Console.WriteLine("New book added:" + book.Title);
                }
            }
        }

        static void Init()
        {
            Authors author = new Authors { FirstName = "Ray", LastName = "Bradbury" };
            AddAuthor(author);
            author = new Authors { FirstName = "Harry", LastName = "Harrison" };
            AddAuthor(author);
            author = new Authors { FirstName = "Clifford", LastName = "Simak" };
            AddAuthor(author);

            Publishers publisher = new Publishers { PublisherName = "Rainbow", Address = "Kyiv" };
            AddPublisher(publisher);
            publisher = new Publishers { PublisherName = "Exlibris", Address = "Kyiv" };
            AddPublisher(publisher);

            Books book = new Books
            {
                Title = "Way Station",
                PublisherId = 1,
                AuthorId = 4,
                Pages = 350,
                Price = 85
            };
            AddBook(book);
            book = new Books
            {
                Title = "Ring Around the Sun",
                PublisherId = 1,
                AuthorId = 4,
                Pages = 420,
                Price = 99
            };
            AddBook(book);
            book = new Books
            {
                Title = "The Martian Chronicles",
                PublisherId = 2,
                AuthorId = 2,
                Pages = 410,
                Price = 105
            };
            AddBook(book);
            book = new Books
            {
                Title = "I, Robot",
                PublisherId = 1,
                AuthorId = 1,
                Pages = 378,
                Price = 100
            };
            AddBook(book);
        }

        //транзакция
        static void MyTransaction()
        {
            using (LibraryDbEntities db = new LibraryDbEntities())
            {

                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        Authors author = new Authors { FirstName = "Stanislaw", LastName = "Lem" };
                        db.Authors.Add(author);
                        db.Authors.Remove(author);

                        db.SaveChanges();

                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {

                        dbTran.Rollback();
                    }

                }
            }
        }
    }

    //класс для логирования
    public class MyLogger
    {
        public static void EFLog(string message)
        {
            Console.WriteLine("Action performed: {0} ", message);
        }
    }
}
