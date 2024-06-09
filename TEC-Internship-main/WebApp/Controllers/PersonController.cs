using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class PersonController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync("http://localhost:5229/api/persons");
                if (message.IsSuccessStatusCode)
                {
                    var jstring = await message.Content.ReadAsStringAsync();
                    List<PersonInformation> list = JsonConvert.DeserializeObject<List<PersonInformation>>(jstring);
                    return View(list);
                }
                else
                {
                    return View(new List<PersonInformation>());
                }
            }
        }

        public IActionResult Add()
        {
            return View(new Person());
        }

        /*        [HttpPost]
                public async Task<IActionResult> Add(Person person)
                {
                    if (ModelState.IsValid)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            // Include Salary data in the request
                            var salary = new Salary { Amount = person.SalaryId };
                            person.Salary = salary;

                            // Include Position data in the request
                            var position = new Position { PositionId = person.PositionId };
                            person.Position = position;

                            var jsonPerson = JsonConvert.SerializeObject(person);
                            StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");
                            HttpResponseMessage message = await client.PostAsync("http://localhost:5229/api/persons", content);
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
                }*/


        [HttpPost]
        public async Task<IActionResult> Add(int positionId, string name, string surname, int age, string email, string address, int salaryId)
        {
            // Declare the person variable outside the conditional block
            Person person = null;

            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    // Create a new Person object with the provided parameters
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

                    // Include Salary data in the request
                    var salary = new Salary { Amount = person.SalaryId };
                    person.Salary = salary;

                    // Include Position data in the request
                    var position = new Position { PositionId = person.PositionId };
                    person.Position = position;

                    // Serialize the Person object to JSON
                    var jsonPerson = JsonConvert.SerializeObject(person);
                    StringContent content = new StringContent(jsonPerson, Encoding.UTF8, "application/json");

                    // Send POST request to the API endpoint
                    HttpResponseMessage message = await client.PostAsync("http://localhost:5229/api/persons", content);
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
                // If ModelState is not valid, return the view with the provided person object
                return View(person);
            }
        }



        public async Task<IActionResult> Update(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync($"http://localhost:5229/api/persons/{id}");
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
                    HttpResponseMessage response = await client.PutAsync("http://localhost:5229/api/persons", content);
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
                    HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5229/api/persons/{id}");
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
