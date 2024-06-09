using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            using (var db = new APIDbContext())
            {
                var list = db.Persons.Include(x => x.Salary).Include(x => x.Position)
                    .Select(x => new PersonInformation()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PositionName = x.Position.Name,
                        Salary = x.Salary.Amount,
                    }).ToList();
                return Ok(list);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var db = new APIDbContext())
            {
                Person person = db.Persons.FirstOrDefault(x => x.Id == id);
                if (person == null)
                {
                    return NotFound();
                }
                return Ok(person);
            }
        }


        [HttpPost]
        public IActionResult Post(int positionId, string name, string surname, int age, string email, string address, int salaryId)
        {
            if (ModelState.IsValid)
            {
                using (var db = new APIDbContext())
                {
                    // Create a new person object
                    var person = new Person
                    {
                        PositionId = positionId,
                        Name = name,
                        Surname = surname,
                        Age = age,
                        Email = email,
                        Address = address,
                        SalaryId = salaryId
                    };

                    db.Persons.Add(person);
                    db.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
                }
            }
            return BadRequest(ModelState);
        }





        /*        [HttpPut]
                public IActionResult UpdatePerson(Person person)
                {

                    if (ModelState.IsValid)
                    {
                        var db = new APIDbContext();
                        Person updateperson = db.Persons.Find(person.Id);
                        if (updateperson == null)
                        {
                            return NotFound();
                        }
                        updateperson.Address = person.Address;
                        updateperson.Age = person.Age;
                        updateperson.Email = person.Email;
                        updateperson.Name = person.Name;
                        updateperson.PositionId = person.PositionId;
                        updateperson.SalaryId = person.SalaryId;
                        updateperson.Surname = person.Surname;
                        db.SaveChanges();
                        return NoContent();
                    }
                    else
                        return BadRequest();
                }*/


        [HttpPut]
        public IActionResult UpdatePerson(Person person)
        {
            if (ModelState.IsValid)
            {
                using (var db = new APIDbContext())
                {
                    var updatePerson = db.Persons.Include(p => p.Position).Include(p => p.Salary).FirstOrDefault(p => p.Id == person.Id);
                    if (updatePerson == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the person entity
                    updatePerson.Address = person.Address;
                    updatePerson.Age = person.Age;
                    updatePerson.Email = person.Email;
                    updatePerson.Name = person.Name;
                    updatePerson.Surname = person.Surname;

                    // Update related entities
                    updatePerson.PositionId = person.PositionId;
                    updatePerson.SalaryId = person.SalaryId;

                    db.SaveChanges();
                    return NoContent();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }




        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                var person = db.Persons.FirstOrDefault(x => x.Id == id);
                if (person == null)
                {
                    return NotFound(); 
                }
                db.Persons.Remove(person);
                db.SaveChanges();
                return NoContent(); 
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
