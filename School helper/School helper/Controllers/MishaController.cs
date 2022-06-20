using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School_helper.Models;
using System.Text;
using System.Text.RegularExpressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace School_helper.Controllers
{
    public class MishaController : Controller
    {
        public IActionResult Misha()
        {
            ViewData["Message"] = "Миша";

            return View();
        }
    }
}
