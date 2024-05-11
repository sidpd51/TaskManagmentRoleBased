using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagmentRoleBased.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
        internal class EmployeeMetadata
        {
            public EmployeeMetadata()
            {
                this.Employee1 = new HashSet<Employee>();
                this.Tasks = new HashSet<Task>();
                this.Tasks1 = new HashSet<Task>();
                this.Tasks2 = new HashSet<Task>();
            }

            public int EmployeeID { get; set; }
            public string EmployeeCode { get; set; }


            [Required(ErrorMessage ="Email is required!")]
            [RegularExpression("[a-z0-9\\.]+@[a-z]{2,}\\.[a-z]{2,}$", ErrorMessage = "Enter Valid Email")]
            public string Email { get; set; }


            [Required(ErrorMessage = "Password is required!")]
            //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage = "Password must be 8 characters or longer and include uppercase, lowercase, number, and special character.")]
            public string Password { get; set; }




            [Required(ErrorMessage ="FirstName is required!")]
            public string FirstName { get; set; }




            [Required(ErrorMessage ="LastName is required!")]
            public string LastName { get; set; }


            [Required(ErrorMessage ="BirthDate is required!")]
            public Nullable<System.DateTime> BirthDate { get; set; }


            [Required(ErrorMessage ="Gender is Required")]
            public string Gender { get; set; }
            public Nullable<int> DepartmentId { get; set; }
            public Nullable<int> ReportingPerson { get; set; }

            public virtual Department Department { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Employee> Employee1 { get; set; }
            public virtual Employee Employee2 { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Task> Tasks { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Task> Tasks1 { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<Task> Tasks2 { get; set; }
        }
    }
}