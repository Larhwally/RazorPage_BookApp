using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBookApp.Data;
using MyBookApp.Models;

namespace MyBookApp.Pages.BookPages
{
    public class BorrowedBooksModel : PageModel
    {
        private readonly BookContext context;

        public BorrowedBooksModel(BookContext db)
        {
            context = db;
        }

        public IEnumerable<book> Books { get; set; }
        public async Task OnGet()
        {
            Books = await context.GetBorrowedBooks();
        }
    }
}
