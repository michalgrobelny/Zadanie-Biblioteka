using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Model
{
    public interface IBook
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public uint YearOfPublication { get; set; }
    }
}