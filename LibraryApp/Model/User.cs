using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public abstract class User : IUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<string> BorrowedBooks { get; set; }
        public int AllowedBooks { get; set; }

        public User(string userId, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = userId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            BorrowedBooks = [];
        }
        public override string ToString()
        {
            string output = string.Empty;
            output += $"\tUser ID: {Id}\n\tFirst name: {FirstName}\n\tLast name: {LastName}" +
                $"\n\tDate of birth: {DateOfBirth:dd-MM-yyyy}\n\tBorrowed books: {BorrowedBooks.Count}";
            return output;
        }
    }
}