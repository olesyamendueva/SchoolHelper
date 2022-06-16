using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School_helper.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace School_helper.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Генератор схем";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Мендуева Алёна Павловна";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /// <summary>
        /// Метод для сохранения/создания объекта FlSubject.
        /// </summary>
        [HttpPost]
        public JsonResult Save([FromBody] TextModel model)
        {
            string text = model.Text.ToLower().Trim();

            if (CheckText(text))
            {
                string schema = GenerateSchema(text);

                string htmlContent = schema;
                htmlContent = htmlContent.Replace(@"[с\к]", "<img src =\"/images/items/br.jpg\" height=\"50\" width=\"80\"/>");
                htmlContent = htmlContent.Replace(@"[к]", "<img src =\"/images/items/r.jpg\" height=\"50\" width=\"50\"/>");
                htmlContent = htmlContent.Replace(@"[з\к]", "<img src =\"/images/items/gr.jpg\" height=\"50\" width=\"80\"/>");
                htmlContent = htmlContent.Replace(@"[с]", "<img src =\"/images/items/b.jpg\" height=\"50\" width=\"50\"/>");
                htmlContent = htmlContent.Replace(@"[з]", "<img src =\"/images/items/g.jpg\" height=\"50\" width=\"50\"/>");

                htmlContent = "<h4>" + htmlContent + @"</h4>";

                return Json(new { schema = htmlContent });
            }
            else
            {
                return Json(new { errors = true });
            }
        }

        public bool CheckText(string text)
        {
            Regex rgx = new Regex("^[А-ЯЁа-яё]+$");   // /^[А-ЯЁ][а-яё]*$/   @"^[а-яА-Я]$"
            return rgx.IsMatch(text);
        }


        public string GenerateSchema(string text) {

            string schema = "";

            char[] chars = text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                string curLetter = chars[i].ToString();
                string curCase = "";



                if (curLetter == "а" || curLetter == "о" || curLetter == "у" || curLetter == "ы" || curLetter == "э")
                {
                    // Если перед ней есть согласная
                    if (i > 0) {
                        if (IsSoglas(chars[i - 1].ToString())) {
                            curCase = @"[с\к]";
                        }
                    }
                    
                    // Иначе (нет согласной или начало слова)
                    if(String.IsNullOrEmpty(curCase))
                        curCase = @"[к]";

                    schema = schema + curCase + " ";
                }



                if (curLetter == "я" || curLetter == "ё" || curLetter == "ю" || curLetter == "е")
                {
                    curCase = @"[з\к]";
                    schema = schema + curCase + " ";
                }



                if (curLetter == "и")
                {
                    // Если перед ней есть согласная
                    if (i > 0)
                    {
                        if (IsSoglas(chars[i - 1].ToString()))
                        {
                            curCase = @"[з\к]";
                        }
                    }

                    // Иначе (нет согласной или начало слова)
                    if (String.IsNullOrEmpty(curCase))
                        curCase = @"[к]";

                    schema = schema + curCase + " ";
                }



                if (curLetter == "ж" || curLetter == "ш" || curLetter == "ц")
                {
                    bool isGlasAfter = false;
                    if (chars.Length > (i + 1))
                    {
                        // Если после нее гласная 
                        if (IsGlas(chars[i + 1].ToString()))
                            isGlasAfter = true;
                    }
                    if (isGlasAfter == true)
                    {
                        curCase = @"[с\к]";
                        i = i + 1;
                    }
                    else {
                        curCase = @"[с]";
                    }

                    schema = schema + curCase + " ";
                }

                if (curLetter == "й" || curLetter == "ч" || curLetter == "щ")
                {
                    bool isGlasAfter = false;
                    if (chars.Length > (i + 1))
                    {
                        // Если после нее гласная 
                        if (IsGlas(chars[i + 1].ToString()))
                            isGlasAfter = true;
                    }
                    if (isGlasAfter == true)
                    {
                        curCase = @"[з\к]";
                        i = i + 1;
                    }
                    else
                    {
                        curCase = @"[з]";
                    }

                    schema = schema + curCase + " ";
                }

                // Все остальные согласные
                // После которых нет гласной
                if (IsOtherSoglas(curLetter))
                {
                    bool case5 = true;
                    if (chars.Length > (i + 1))
                    {
                        // Если после нее гласная значит не наш случай
                        if (IsGlas(chars[i + 1].ToString()))
                            case5 = false;
                    }

                    bool hasMagZnak = false;
                    if (case5 == true)
                    {
                        // Если потом идет ь
                        if (chars.Length > (i + 1))
                        {
                            // ь сразу после
                            if (chars[i + 1].ToString() == "ь")
                                hasMagZnak = true;

                            // ь через одну согласную
                            if (IsSoglas(chars[i + 1].ToString()))
                            {
                                if (chars.Length > (i + 2))
                                {
                                    if (chars[i + 2].ToString() == "ь")
                                        hasMagZnak = true;
                                }
                            }
                        }

                        if (hasMagZnak == true)
                        {
                            curCase = @"[з]";
                        }
                        else
                        {
                            curCase = @"[с]";
                        }
                        schema = schema + curCase + " ";
                    }
                }

            }

            schema = schema.Trim();

            return schema;
        }

        public bool IsGlas(string s)
        {
            if (s == "а" || s == "о" || s == "у" || s == "ы" || s == "э" || s == "и" || s == "я" || s == "ё" || s == "ю" || s == "е")
            {
                return true;
            }
            else return false;
        }

        public bool IsOtherSoglas(string s)
        {
            if (s != "а" && s != "о" && s != "у" && s != "ы" && s != "э" && s != "и" && s != "я" && s != "ё" && s != "ю" && s != "е" && 
                s != "ь" && s != "ъ" && 
                s != "ж" && s != "ш" && s != "ц" && 
                s != "й" && s != "ч" && s != "щ")
            {
                return true;
            }
            else return false;
        }

        public bool IsSoglas(string s)
        {
            if (s != "а" && s != "о" && s != "у" && s != "ы" && s != "э" && s != "и" && s != "я" && s != "ё" && s != "ю" && s != "е" &&
                s != "ь" && s != "ъ")
            {
                return true;
            }
            else return false;
        }
    }
}
