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

namespace MyBookApp.Pages.BookPages
{
    public class EditModel : PageModel
    {
        private readonly BookContext context;
        private readonly IWebHostEnvironment webHost;

        public EditModel(BookContext db, IWebHostEnvironment env)
        {
            context = db;
            webHost = env;
        }

        [BindProperty]
        public book singlebook { get; set; }

        public IEnumerable<book> Books { get; set; }
        [BindProperty]

        public IFormFile file { get; set; }
        public string fileName { get; set; }

        public async Task OnGet(int id)
        {
            
            fileName = context.ReturnFileName(id);
            string[] path = fileName.Split('\\');
            fileName = String.Join("/", path);
            //Guid guid = new Guid();
            //var imageName = Path.Combine(webHost.ContentRootPath, "Files", guid + file.FileName);
            //if (context.CheckImage(imageName) == true)
            //{
            //    using(var fileStream = new FileStream(imageName, FileMode.Open))
            //    {
            //        var im =  PhysicalFile(imageName, "image/jpg");
            //    }
            //}
            Books = await context.GetBooks();
            singlebook = await context.GetBookById(id);
        }
        public FileResult OnGetImage()
        {
            var img = System.IO.File.ReadAllBytes(fileName);
            return File(img, "image/jpg");
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var bookById = await context.GetBookById(singlebook.id);

                bookById.bookTitle = singlebook.bookTitle;
                bookById.authorName = singlebook.authorName;
                bookById.status = singlebook.status;

                if (singlebook.status == "Borrowed")
                {
                    await context.UpdateBook(singlebook);
                    await context.PostBorrowedBook(singlebook.id, singlebook.status);
                    return RedirectToPage("Index");
                }
                else
                {
                    await context.UpdateBook(singlebook);
                    await context.UpdateBorrowedBook(singlebook.status, singlebook.id);
                    return RedirectToPage("Index");
                }


            }

            return RedirectToPage();
        }
    }
}
