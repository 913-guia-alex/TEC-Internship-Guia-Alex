using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SalaryController : Controller
    {
        private readonly string apiUrl;

        public SalaryController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiSettings:ApiUrl"];
        }

        public async Task<IActionResult> Index()
        {
            List<Salary> list = new List<Salary>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(apiUrl + "salaries");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Salary>>(jstring);
                }
            }
            return View(list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.DeleteAsync($"{apiUrl}salaries/{id}");
                if (message.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult Add()
        {
            Salary salary = new Salary();
            return View(salary);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Salary salary)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsondepartment = JsonConvert.SerializeObject(salary);
                    StringContent content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PostAsync(apiUrl + "salaries", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "There is an API error");
                        return View(salary);
                    }
                }
            }
            else
            {
                return View(salary);
            }
        }
    }
}
