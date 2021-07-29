using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBookApp.Pages
{
    public class TutorialModel : PageModel
    {
        public string Greeting { get; set; }
        public Dictionary<string, string> dico = new Dictionary<string, string>();
        public void OnGet()
        {
            Greeting = "Hi! You are welcome to C# .NET CLass!";
            dico.Add("Name:", "Aina Divine");
            dico.Add("Age:", "15");
            dico.Add("Place of birth:", "Ilaje");
            dico.Add("Gender", "Male");

        }
    }
}
