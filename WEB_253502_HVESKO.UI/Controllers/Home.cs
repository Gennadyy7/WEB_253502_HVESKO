using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_253502_HVESKO.UI.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            // Создание списка элементов
            var items = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Элемент 1" },
                new ListDemo { Id = 2, Name = "Элемент 2" },
                new ListDemo { Id = 3, Name = "Элемент 3" }
            };

            // Создание SelectList
            ViewData["ItemSelectList"] = new SelectList(items, "Id", "Name");

            ViewData["Headline"] = "Лабораторная работа №2";

            List<string> points = new List<string>
            {
                "элемент 1 списка",
                "элемент 2 списка",
                "элемент 3 списка",
                "элемент 4 списка",
            };

            return View(points);
        }
    }
}
