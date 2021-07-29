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
    public class DeleteModel : PageModel
    {
        private readonly BookContext context;
        public DeleteModel(BookContext db)
        {
            context = db;
        }
        [BindProperty]
        public book singlebook { get; set; }

        public async Task OnGet(int id)
        {
            singlebook = await context.GetBookById(id);
        }

        public async Task<IActionResult> OnPost(int id)
        {
            if (id > 0)
            {
                await context.DeleteBook(id);
                return RedirectToPage("Index");
            }
            return RedirectToPage();

        }
    }
}
