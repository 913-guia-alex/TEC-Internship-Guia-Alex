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
    public class PersonController : Controller
    {
        private readonly string apiUrl;

        public PersonController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiSettings:ApiUrl"];
        }

        public async Task<IActionResult> Index()
        {
            List<PersonInformation> list = new List<PersonInformation>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(apiUrl + "persons");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<PersonInformation>>(jstring);
                }
            }
            return View(list);
        }

        public IActionResult Add()
        {
            return View(new Person());
        }

        [HttpPost]
        public async Task<IActionResult> Add(int positionId, string name, string surname, int age, string email, string address, int salaryId)
        {
            Person person = null;

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    person = new Person
                    {
                        PositionId = positionId,
                        Name = name,
                        Surname = surname,
                        Age = age,
                        Email = email,
                        Address = address,
                        SalaryId = salaryId
                    };

                    var salary = new Salary { Amount = person.SalaryId };
                    person.Salary = salary;

                    var position = new Position { PositionId = person.PositionId };
                    person.Position = position;

                    var jsonPerson = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");

                    HttpResponseMessage message = await client.PostAsync(apiUrl + "persons", content);
                    if (message.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await message.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Failed to add the person. The server returned an error: {errorContent}");
                        return View(person);
                    }
                }
            }
            else
            {
                return View(person);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync($"{apiUrl}persons/{id}");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    Person person = JsonConvert.DeserializeObject<Person>(jstring);
                    return View(person);
                }
                else
                {
                    return RedirectToAction("Add");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person person)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonPerson = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(apiUrl + "persons", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Failed to update the person. The server returned an error: {errorContent}");
                        return View(person);
                    }
                }
            }
            else
            {
                return View(person);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}persons/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to delete the person. Please try again later.");
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
