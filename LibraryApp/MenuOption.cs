using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    public enum MenuOption
    {
        None = 0,
        AddBook,
        RemoveBook,
        BorrowBook,
        ReturnBook,
        ListBooks,
        AddUser,
        RemoveUser,
        ListUsers,
        Exit
    }
}