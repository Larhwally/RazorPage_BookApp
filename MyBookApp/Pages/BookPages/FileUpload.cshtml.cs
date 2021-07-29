using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBookApp.Pages.BookPages
{
    public class FileUploadModel : PageModel
    {
        private readonly IWebHostEnvironment webHost;

        public FileUploadModel(IWebHostEnvironment env)
        {
            webHost = env;
        }
        [BindProperty]
        public IFormFile myFile { get; set; }
        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPost()
        {
            Guid guid = new Guid();
            //set up a name for files to be uploaded
            var fileupload = Path.Combine(webHost.ContentRootPath, "Files", guid + myFile.FileName);
            using (var fileStream = new FileStream(fileupload, FileMode.Create))
            {
                await myFile.CopyToAsync(fileStream);
            }
            return RedirectToPage("Index");
        }
    }
}
