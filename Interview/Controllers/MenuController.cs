using Microsoft.AspNetCore.Mvc;
using Interview.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Interview.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Menu/
        public async Task<IActionResult> Index(string? searchString = null)
        {
            var client = _httpClientFactory.CreateClient("MenuApiClient");
            string apiUrl = "/api/Menu";
            if (!string.IsNullOrEmpty(searchString))
            {
                apiUrl += $"?searchString={searchString}";
            }

            var dishes = await client.GetFromJsonAsync<List<Dish>>(apiUrl);

            return View(dishes);
        }

        // GET: /Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MenuApiClient");
            var dish = await client.GetFromJsonAsync<Dish>($"/api/Menu/{id}");

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }
        // GET: /Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImageUrl,Price")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("MenuApiClient");
                var response = await client.PostAsJsonAsync("/api/Menu", dish);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "新增菜單失敗");
            }
            return View(dish);
        }

        // POST: /Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("MenuApiClient");
            var response = await client.DeleteAsync($"/api/Menu/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "刪除菜單失敗");
            return RedirectToAction(nameof(Index));
        }

    }
}
