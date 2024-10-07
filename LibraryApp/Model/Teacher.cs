using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public class Teacher : User
    {
        public Teacher(string userId, string firstName, string lastName, DateTime dateOfBirth)
            : base(userId, firstName, lastName, dateOfBirth)
        {
            AllowedBooks = 10;
        }
    }
}