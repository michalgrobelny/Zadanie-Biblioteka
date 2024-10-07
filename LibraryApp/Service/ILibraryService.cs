using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Service
{
    public interface ILibraryService
    {
        public void AddBook();
        public void RemoveBook();
        public void AddUser();
        public void RemoveUser();
        public void ListBooks();
        public void ListUsers();
        public void BorrowBook();
        public void ReturnBook();

    }
}
