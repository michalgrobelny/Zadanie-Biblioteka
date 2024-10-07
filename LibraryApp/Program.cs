using System.Dynamic;
using LibraryApp.Service;

namespace LibraryApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool isRunning = true;

            LibraryService service = new LibraryService();

            service.PrepopulateBooksAndUsers();

            while (isRunning)
            {
                DisplayMenu();
                MenuOption option = GetMenuSelection();
                isRunning = ProcessSelectedOption(option, service);
            }

            Console.WriteLine("Exiting the Library App...");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("WELCOME TO OUR LIBRARY!\n");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Remove Book");
            Console.WriteLine("3. Borrow Book");
            Console.WriteLine("4. Return Book");
            Console.WriteLine("5. List Books");
            Console.WriteLine("6. Add User");
            Console.WriteLine("7. Remove User");
            Console.WriteLine("8. List Users");
            Console.WriteLine("9. Exit");
        }

        static MenuOption GetMenuSelection()
        {
            int option = 0;
            Console.Write("\nSelect your option: ");
            
            while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 9)
            {
                Console.WriteLine("Invalid option. Try again.");
                Console.Write("\nSelect your option: ");
            }

            return (MenuOption)option;
        }

        static bool ProcessSelectedOption(MenuOption option, LibraryService service)
        {
            switch (option)
            {
                case MenuOption.AddBook:
                    service.AddBook();
                    break;
                case MenuOption.RemoveBook:
                    service.RemoveBook();
                    break;
                case MenuOption.BorrowBook:
                    service.BorrowBook();
                    break;
                case MenuOption.ReturnBook:
                    service.ReturnBook();
                    break;
                case MenuOption.ListBooks:
                    service.ListBooks();
                    break;
                case MenuOption.AddUser:
                    service.AddUser();
                    break;
                case MenuOption.RemoveUser:
                    service.RemoveUser();
                    break;
                case MenuOption.ListUsers:
                    service.ListUsers();
                    break;
                case MenuOption.Exit:
                    return false;
            }
            return true;
        }
    }
}