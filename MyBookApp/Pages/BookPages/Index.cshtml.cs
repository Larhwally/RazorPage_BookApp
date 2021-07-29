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
    public class IndexModel : PageModel
    {
        private readonly BookContext context;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public IndexModel(BookContext db)
        {
            context = db;
        }

        public IEnumerable<book> Books { get; set; }
        public async Task OnGet()
        {

            if (string.IsNullOrEmpty(SearchTerm))
            {
                Books = await context.GetBooks();
            }
            else
            {
                Books = await context.SearchBook(SearchTerm);
            }

        }
    }
}
