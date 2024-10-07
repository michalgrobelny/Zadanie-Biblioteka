using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Model;

namespace LibraryApp.Service
{
    public class LibraryService : ILibraryService
    {
        private List<Book> Books { get; set; } = new List<Book>();
        private List<User> Users { get; set; } = new List<User>();

        public void AddBook()
        {
            Console.WriteLine("\nAdding a new book...");

            Book? book = GetNewBookDetails();
            if (book is not null)
            {
                Books.Add(book);
                PrintBookAddedMessage();
            }
        }

        private static void PrintBookAddedMessage()
        {
            Console.WriteLine("\nThe book has been successfully added to the library.\n");
        }

        private static void PrintUserAddedMessage()
        {
            Console.WriteLine("\nThe user has been successfully added to the library.\n");
        }

        public void AddBook(string isbn, string title, string author, uint yearOfPublication)
        {
            Console.WriteLine("\nAdding a new book...");

            if (CanBookBeAddedAsNew(isbn))
            {
                Books.Add(new Book(isbn, title, author, yearOfPublication));
                PrintBookAddedMessage();
            }
        }

        private bool IsBookInLibrary(string isbn, out Book? book)
        {
            book = Books.SingleOrDefault(b => b.ISBN == isbn);
            return book is not null;
        }

        private bool CanBookBeAddedAsNew(string isbn)
        {
            bool isBookInLibrary = Books.Any(b => b.ISBN == isbn);

            if (isBookInLibrary)
            {
                Console.WriteLine($"\nThe book with ISBN {isbn} is already in the library and cannot be added again.\n");
                return false;
            }

            return true;
        }

        private Book? GetNewBookDetails()
        {
            string isbn = GetStringAsInput("\nEnter the ISBN: ");

            if (!CanBookBeAddedAsNew(isbn))
            {
                return null;
            }

            string title = GetStringAsInput("\nEnter the title: ");
            string author = GetStringAsInput("\nEnter the author: ");
            uint yearOfPublication = GetYearAsUInt("\nEnter the year of publication: ");

            return new Book(isbn, title, author, yearOfPublication);
        }

        private static string GetStringAsInput(string prompt)
        {
            Console.Write(prompt);

            string? input;
            while (string.IsNullOrWhiteSpace(input = Console.ReadLine()))
            {
                Console.WriteLine("\nThis value cannot be empty. Please try again.");
            }

            return input;
        }

        private static uint GetYearAsUInt(string prompt)
        {
            string input;

            while (true)
            {
                input = GetStringAsInput(prompt);
                if (uint.TryParse(input, out uint value)) return value;

                Console.WriteLine("\nInvalid input. Please enter a valid integer number.");
            }
        }

        public void AddUser()
        {
            Console.WriteLine("\nAdding a new user...");

            User? user = GetNewUserDetails();

            if (user is not null)
            {
                Users.Add(user);
                PrintUserAddedMessage();
            }
        }

        public void AddUser(string userId, string firstName, string lastName, DateTime dateOfBirth, UserCategory category)
        {
            Console.WriteLine("\nAdding a new user...");

            if (!CanUserBeAddedAsNew(userId, dateOfBirth))
            {
                return;
            }

            if (category == UserCategory.Teacher)
            {
                Users.Add(new Teacher(userId, firstName, lastName, dateOfBirth));
                PrintUserAddedMessage();
            }
            else
            {
                Users.Add(new Student(userId, firstName, lastName, dateOfBirth));
                PrintUserAddedMessage();
            }
        }

        private bool CanUserBeAddedAsNew(string userId, DateTime dateOfBirth)
        {
            bool isUserValid = true;

            if (IsUserInLibrary(userId, out _))
            {
                Console.WriteLine($"\nThe user \"{userId}\" is already in the library and cannot be added again.");
                isUserValid = false;
            }

            if (!IsUserOldEnough(dateOfBirth))
            {
                Console.WriteLine($"\nThe user born on {dateOfBirth:dd-MM-yyyy} is under 13 years old and cannot be added to the library.");
                isUserValid = false;
            }

            return isUserValid;
        }

        private bool IsUserOldEnough(DateTime dateOfBirth)
        {
            Console.Write($"\n[DEBUG] The user born on {dateOfBirth:dd-MM-yyyy} is old enough: ");
            Console.Write($"{dateOfBirth < DateTime.Today.AddYears(-13)}\n");

            return dateOfBirth < DateTime.Today.AddYears(-13);
        }

        private bool IsUserInLibrary(string userId, out User? user)
        {
            user = Users.FirstOrDefault(u => u.Id == userId);

            Console.WriteLine($"\n[DEBUG] The user \"{userId}\" is in the library: {user is not null}");

            return user is not null;
        }

        private User? GetNewUserDetails()
        {
            Console.Write("\nEnter the category (1 - Teacher, 2 - Student): ");
            UserCategory category = GetUserCategory();

            string userId = GetStringAsInput("\nEnter the user ID: ");

            Console.Write("\nEnter the date of birth in the format yyyy-mm-dd: ");
            DateTime dateOfBirth = GetDateOfBirth();

            if (!CanUserBeAddedAsNew(userId, dateOfBirth))
            {
                return null;
            }

            string firstName = GetStringAsInput("\nEnter the first name: ");
            string lastName = GetStringAsInput("\nEnter the last name: ");

            if (category == UserCategory.Teacher)
            {
                return new Teacher(userId, firstName, lastName, dateOfBirth);
            }
            else
            {
                return new Student(userId, firstName, lastName, dateOfBirth);
            }
        }

        private DateTime GetDateOfBirth()
        {
            string? input;

            while (true)
            {
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("\nInput cannot be empty. Please try again.");
                    continue;
                }

                if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
                {
                    return dateOfBirth;
                }

                Console.WriteLine("\nInvalid input. Please enter a valid date in the format yyyy-mm-dd.");
            }
        }

        private UserCategory GetUserCategory()
        {
            string? input;

            while (true)
            {
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)
                    && int.TryParse(input, out int value)
                    && (value == 1 || value == 2))
                {
                    return (UserCategory)value;
                }

                Console.WriteLine($"\nInvalid input. Please enter 1 or 2.");
            }
        }

        public void BorrowBook()
        {
            Console.WriteLine("\nLending a book...");

            Book book = GetExistingBook();

            bool isBookAvailable = !book.IsBorrowed;

            Console.WriteLine($"[DEBUG] The book #{book.ISBN} is borrowed = {book.IsBorrowed}");  // test

            if (isBookAvailable)
            {
                Console.WriteLine("\nThe book is available."); // test

                User user = GetExistingUser();

                if (user.BorrowedBooks.Count < user.AllowedBooks)
                {
                    Console.WriteLine("This user can borrow a book.");
                    user.BorrowedBooks.Add(book.ISBN);
                    Books.Single(b => b.ISBN == book.ISBN).WhoBorrowed = user.Id;
                }
                else
                {
                    Console.WriteLine($"This user has already {user.BorrowedBooks.Count} books and cannot borrow more.");
                    return;
                }
            }
        }

        public void BorrowBook(string userId, string isbn)
        {
            Console.WriteLine("\nLending a book...");

            if (IsBookInLibrary(isbn, out Book? book) && book is not null)
            {
                bool isBookAvailable = !book.IsBorrowed;

                Console.WriteLine($"\n[DEBUG] The book #{book.ISBN} is available = {isBookAvailable}");  // test

                if (isBookAvailable)
                {
                    Console.WriteLine("\nThe book is available."); // test

                    if (IsUserInLibrary(userId, out User? user) && user is not null)
                    {
                        if (user.BorrowedBooks.Count < user.AllowedBooks)
                        {
                            Console.WriteLine("\nThis user can borrow a book.");

                            user.BorrowedBooks.Add(book.ISBN);
                            Books.Single(b => b.ISBN == isbn).IsBorrowed = true;
                            Books.Single(b => b.ISBN == isbn).WhoBorrowed = userId;
                            return;
                        }
                        else
                        {
                            Console.WriteLine($"\nThis user has already {user.BorrowedBooks.Count} books and cannot borrow more.");
                            return;
                        }
                    }
                }
            }
        }

        public void ListBooks()
        {
            Console.WriteLine($"\nDisplaying the list of {Books.Count} books...");

            Book book;
            User? user;
            for (int i = 0; i < Books.Count; i++)
            {
                book = Books[i];
                user = GetUserById(book.WhoBorrowed);

                Console.WriteLine($"\nBook #{i + 1}:");
                Console.Write($"{book}\n");

                if (user is not null)
                {
                    Console.WriteLine($"\tBorrowed by {user.FirstName} {user.LastName} ({user.Id})\n");
                }
                else
                {
                    Console.WriteLine($"\tAvailable for borrowing\n");
                }

            }

            Console.WriteLine("");
        }

        private User? GetUserById(string userId)
        {
            return Users.FirstOrDefault(u => u.Id == userId);
        }

        public void ListUsers()
        {
            Console.WriteLine($"\nDisplaying the list of {Users.Count} users...");

            for (int i = 0; i < Users.Count; i++)
            {
                Console.WriteLine($"\nUser #{i + 1}:");
                Console.Write($"{Users[i]}\n");
            }

            Console.WriteLine("");
        }

        public void RemoveBook()
        {
            Console.WriteLine("\nRemoving a book...");

            Book book = GetExistingBook();
            if (book.IsBorrowed)
            {
                Console.WriteLine("\nThis book has not been returned yet and cannot be removed.\n");
            }
            else
            {
                Books.Remove(book);

                Console.WriteLine($"\nThe book \"{book.Title}\" has been successfully removed from the library.\n");
            }
        }

        public void RemoveUser()
        {
            Console.WriteLine("\nRemoving a user...");

            User user = GetExistingUser();
            if (user.BorrowedBooks.Count > 0)
            {
                Console.WriteLine("\nThis user has not returned all the books yet. Cannot remove this user.\n");
            }
            else
            {
                Users.Remove(user);
            }
        }

        public void ReturnBook()
        {
            Console.WriteLine("\nReturning a book...");

            Book book = GetExistingBook();

            string isbn = book.ISBN;

            Console.WriteLine($"\n[DEBUG] The book with ISBN {isbn} {book.IsBorrowed} has been borrowed by {book.WhoBorrowed}");

            if (book.IsBorrowed)
            {
                string userId = book.WhoBorrowed;
                Books.Single(b => b.ISBN == isbn).WhoBorrowed = string.Empty;
                Users.Single(u => u.Id == userId).BorrowedBooks.Remove(userId);

                Console.WriteLine($"\nThe book \"{book.Title}\" has been returned successfully.\n");
                return;
            }
            else
            {
                Console.Write("\nYou have attempted to return a book. ");
                Console.Write("However, the book is available for borrowing, so it cannot be returned at the moment.\n");
                return;
            }

        }

        private User GetExistingUser()
        {
            while (true)
            {
                string userId = GetStringAsInput("\nEnter the user ID: ");

                var user = Users.SingleOrDefault(u => u.Id == userId);
                if (user is not null) return user;

                Console.WriteLine($"\nUser not found. Please try again.\n");
            }
        }

        private Book GetExistingBook()
        {
            while (true)
            {
                string isbn = GetStringAsInput("Enter the ISBN: ");

                Book? book = Books.SingleOrDefault(b => b.ISBN == isbn);

                if (book is not null)
                {
                    return book;
                }

                Console.WriteLine($"\nBook not found. Please try again.");
            }
        }

        public void PrepopulateBooksAndUsers()
        {
            Console.WriteLine("\n*************************************************");
            Console.WriteLine("*        Pre-Poulating The Database             *");
            Console.WriteLine("*************************************************");

            Console.WriteLine("\n---- Books ----");

            AddBook("1", "1984", "George Orwell", 1949);
            AddBook("2", "To Kill a Mockingbird", "Harper Lee", 1960);
            AddBook("3", "The Great Gatsby", "F. Scott Fitzgerald", 1925);
            AddBook("4", "One Hundred Years of Solitude", "Gabriel Garcia Marquez", 1967);
            AddBook("5", "Moby Dick", "Herman Melville", 1851);
            AddBook("6", "War and Peace", "Leo Tolstoy", 1869);
            AddBook("7", "Pride and Prejudice", "Jane Austen", 1813);
            AddBook("8", "The Catcher in the Rye", "J.D. Salinger", 1951);
            AddBook("9", "The Hobbit", "J.R.R. Tolkien", 1937);
            AddBook("10", "Crime and Punishment", "Fyodor Dostoevsky", 1866);

            Console.WriteLine("\n---- Users ----");

            AddUser("jl", "John", "Lennon", new DateTime(1940, 10, 9), UserCategory.Teacher);
            AddUser("pm", "Paul", "McCartney", new DateTime(1942, 6, 18), UserCategory.Teacher);
            AddUser("gh", "George", "Harrison", new DateTime(1943, 2, 25), UserCategory.Student);
            AddUser("rs", "Ringo", "Starr", new DateTime(1940, 7, 7), UserCategory.Student);
            AddUser("js", "Jane", "Smith", new DateTime(2012, 5, 15), UserCategory.Student);

            Console.WriteLine("\n---- Invalid entries ----");

            AddBook("2", "To Kill a Mockingbird", "Harper Lee", 1960); //invalid entry
            AddUser("jl", "John", "Lennon", new DateTime(1940, 10, 9), UserCategory.Teacher); // invalid entry
            AddUser("gh", "George", "Harrison", new DateTime(1943, 2, 25), UserCategory.Teacher); //invalid entry

            AddSomeBorrowings();

            ListBooks();
            ListUsers();

            Console.WriteLine("\n*************************************************");
            Console.WriteLine("*                 DONE                          *");
            Console.WriteLine("*************************************************\n\n");
        }
        private void AddSomeBorrowings()
        {

            BorrowBook("rs", "1");
            BorrowBook("rs", "2");
            BorrowBook("rs", "3");
            BorrowBook("rs", "4");

            BorrowBook("jl", "4");
            BorrowBook("gh", "5");
        }
    }
}