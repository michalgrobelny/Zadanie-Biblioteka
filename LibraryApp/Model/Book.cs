using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibraryApp.Model
{
    public class Book : IBook
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public uint YearOfPublication { get; set; }
        public bool IsBorrowed { get; set; }
        public string WhoBorrowed { get; set; }
        public Book(string isbn, string title, string author, uint yearOfPublication)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            YearOfPublication = yearOfPublication;
            IsBorrowed = false;
            WhoBorrowed = string.Empty;
        }

        public override string ToString()
        {
            string output = string.Empty;
            output += $"\tTitle: {Title}\n\tAuthor: {Author}\n\tYear of publication: {YearOfPublication}\n\tISBN: {ISBN}";
            return output;
        }
    }
}
