using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly string apiUrl;

        public DepartmentController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiSettings:ApiUrl"];
        }

        public async Task<IActionResult> Index()
        {
            List<Department> list = new List<Department>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + "departments");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jstring = await responseMessage.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Department>>(jstring);
                }
            }
            return View(list);
        }

        public IActionResult Add()
        {
            Department department = new Department();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsondepartment = JsonConvert.SerializeObject(department);
                    StringContent content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PostAsync(apiUrl + "departments", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "There is an API error");
                        return View(department);
                    }
                }
            }
            else
            {
                return View(department);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(apiUrl + "departments/" + Id);
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Department department = JsonConvert.DeserializeObject<Department>(jstring);
                    return View(department);
                }
                else
                {
                    return RedirectToAction("Add");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Department department)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsondepartment = JsonConvert.SerializeObject(department);
                    StringContent content = new StringContent(jsondepartment, Encoding.UTF8, "application/json");
                    HttpResponseMessage message = await client.PutAsync(apiUrl + "departments", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(department);
                    }
                }
            }
            else
            {
                return View(department);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}departments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete the department. Please try again later.");
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
