using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBookApp.Data;
using MyBookApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MyBookApp.Pages.BookPages
{
    public class CreateModel : PageModel
    {
        private readonly BookContext context;
        private readonly IWebHostEnvironment webHost;

        public CreateModel(BookContext con, IWebHostEnvironment web)
        {
            context = con;
            webHost = web;
        }
        [BindProperty]
        public IFormFile file { get; set; }

        [BindProperty]
        public book Book { get; set; }

        public IEnumerable<book> Books { get; set; }

        [BindProperty]
        public ImageUpload image { get; set; }

        public async Task OnGet()
        {
            Books = await context.GetBooks();
        }

        public async Task<IActionResult> OnPost()
        {
            //file naming generating convention
            var fileUploaded = Path.Combine(webHost.ContentRootPath, "Files", file.FileName);
            using (var fs = new FileStream(fileUploaded, FileMode.Create))
            {
                await file.CopyToAsync(fs);
                //ViewData["Message"] = "The File" + file.FileName + " is uploaded successfully";
            }
            if (ModelState.IsValid)
            {
                //post a new record of a book to the db
                //await context.PostBook(Book);
                long id = await context.PostNewBook(Book);
                //method to post image record to DB here
                await context.InsertFileName(fileUploaded, id);

                //await context.InsertFileName(fileUploaded, bookId: id);
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
