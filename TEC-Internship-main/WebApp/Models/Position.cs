using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Position
    {
        public int PositionId { get; set; }

        public string Name { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
