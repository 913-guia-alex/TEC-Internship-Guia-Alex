using System.ComponentModel.DataAnnotations;

namespace Internship.Model
{
    public class PersonDetails
    {
        public DateTime BirthDay { get; set; }
        public string PersonCity { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
