using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskManagmentRoleBased.Models;

namespace TaskManagmentRoleBased.Models
{
    public partial class Task
    {
        internal class TaskMetadata
        {
            public int TaskID { get; set; }


            [Required(ErrorMessage ="Task Date is Required!")]
            public Nullable<System.DateTime> TaskDate { get; set; }
            public Nullable<int> EmployeeID { get; set; }


            [Required(ErrorMessage ="Task Name is Required!")]
            public string TaskName { get; set; }


            [Required(ErrorMessage ="Task Description is Required!")]
            public string TaskDescription { get; set; }


            public Nullable<int> ApproverID { get; set; }
            public Nullable<int> ApprovedOrRejectedBy { get; set; }
            public string Status { get; set; }
            public Nullable<System.DateTime> CreatedOn { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public Nullable<System.DateTime> ApprovedOrRejectedOn { get; set; }

            public virtual Employee Employee { get; set; }
            public virtual Employee Employee1 { get; set; }
            public virtual Employee Employee2 { get; set; }

        }
    }
}