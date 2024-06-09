using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internship.Model
{
    public class Position
    {
        public Position()
        {
            Persons = new HashSet<Person>();
        }

        [Key]
        public int PositionId { get; set; }

        public string Name { get; set; }

        public int DepartmentId { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
