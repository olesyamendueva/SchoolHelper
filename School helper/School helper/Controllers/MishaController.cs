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

        /// <summary>
        /// Метод для сохранения результатов
        /// </summary>
        [HttpPost]
        public JsonResult Save([FromBody] SurveyModel model)
        {
            string fio = model.Fio.Trim();
            string age = model.Age.Trim();
            string hero = model.Hero.Trim();
            string source = model.Source.Trim();

            return Json(new { res = "Данные сохранены. Спасибо за участие!\n" + "Fio=" + fio + "\n" + "Age=" + age + "\n" + "Hero=" + hero + "\n" + "Source=" + source });

        }
    }
}
