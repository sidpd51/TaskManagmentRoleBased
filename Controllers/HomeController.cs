using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagmentRoleBased.Models;

namespace TaskManagmentRoleBased.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private office_sidpd_newEntities db = new office_sidpd_newEntities(); //access modifier taken as private here and in authentication controller check it out

        [Authorize(Roles = "Employee")]
        public ActionResult Index()
        {
            int employeeId = (int)Convert.ToInt64(Session["id"]);
            if(employeeId == 0) // 
            {
                Employee emp = new Employee();//instantiates a new employees
                return View(emp); // return the view named "index" with the newly created Employee object
            }
            Employee employee = db.Employees.Find(employeeId);
            return View(employee);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Manager()
        {
            int id = (int)Convert.ToInt64(Session["id"]);
            Employee emp = db.Employees.Find(id);

            return View(emp);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult EmployeeTask()
        {
            var id = (int)Convert.ToInt64(Session["id"]);

            Employee emp = db.Employees.Where(e => e.EmployeeID == id).FirstOrDefault();

            List<Employee> employees = db.Employees.ToList();
            List<Task> tasks = db.Tasks.Where( m => m.ApproverID == emp.EmployeeID).ToList(); 
            var data = (employees, tasks);//creating tuples

            return View(data);
        } 

        public ActionResult DeleteTask(int id)
        {
            var task = db.Tasks.Where(e => e.TaskID == id).FirstOrDefault();
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Task");
        }

        [Authorize(Roles = "Employee, Manager")]
        public ActionResult Task()
        {
            var id = (int)Convert.ToInt64(Session["id"]);
            Employee emp = db.Employees.Where(m => m.EmployeeID == id).FirstOrDefault();
            List<Employee> employees = db.Employees.ToList();
            List<Task> tasks = db.Tasks.Where(m => m.EmployeeID == emp.EmployeeID).ToList();
            var data = (emp, employees, tasks);//sending using touple
            return View(data);
        }

        public ActionResult GetTaskData(int id)
        {
            if(id == 0)
            {
                Task task = new Task();
                return PartialView("_PartialPageTask", task);
            }
            else
            {
                Task task = db.Tasks.Find(id);
                return PartialView("_PartialPageTask", task);
            }
        }

        [HttpPost]
        public ActionResult SubmitTask(Task task)
        {
            if(ModelState.IsValid)
            {
                var id = (int) Convert.ToInt64(Session["id"]);
                Employee emp = db.Employees.Where(m => m.EmployeeID == id).FirstOrDefault();
               
                task.EmployeeID = emp.EmployeeID;
                task.Status = "Pending";  
                if(task.TaskID == 0)
                {
                    task.CreatedOn = DateTime.Now;                }
                else
                {
                    Task currentTask = db.Tasks.Where(t => t.TaskID == task.TaskID).FirstOrDefault();
                    task.CreatedOn = currentTask.CreatedOn;

                }
                task.ApproverID = emp.Employee2.EmployeeID;
                task.ModifiedOn = DateTime.Now;
                db.Tasks.AddOrUpdate(task);
                db.SaveChanges();
                return RedirectToAction("Task","Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Approve(int id, Task task)
        {
            var fullname = Session["username"].ToString();
            var firstName = fullname.Split(' ')[0].Trim();
            var lastName = fullname.Split(' ')[1].Trim();

            Employee emp = db.Employees.Where((e) => e.FirstName == firstName && e.LastName == lastName).FirstOrDefault();
            Task currentTask = db.Tasks.Where(m => m.TaskID == id).FirstOrDefault();
            currentTask.Status = "Approved";
            currentTask.ApprovedOrRejectedBy = emp.EmployeeID;
            db.Tasks.AddOrUpdate(currentTask);
            db.SaveChanges();
            return RedirectToAction("EmployeeTask", "Home");

        }

        [HttpPost]
        public ActionResult Reject(int id, Task task)
        {
            var fullname = Session["username"].ToString();
            var firstName = fullname.Split(' ')[0].Trim();
            var lastName = fullname.Split(' ')[1].Trim();

            Employee emp = db.Employees.Where((e) => e.FirstName == firstName && e.LastName == lastName).FirstOrDefault();
            Task currentTask = db.Tasks.Where(m => m.TaskID == id).FirstOrDefault();
            currentTask.Status = "Rejected";
            currentTask.ApprovedOrRejectedBy = emp.EmployeeID;
            db.Tasks.AddOrUpdate(currentTask);
            db.SaveChanges();
            return RedirectToAction("EmployeeTask", "Home");

        }

        [Authorize(Roles = "Director")]
        public ActionResult Director()
        {
            List<Employee>  employees = db.Employees.ToList();
            return View(employees);
        }

        [HttpPost]
        public ActionResult UpdateData(string employeeCode, int departmentID, string reportingPerson)
        {
            var employee = db.Employees.FirstOrDefault(m => m.EmployeeCode == employeeCode);
            if(reportingPerson == null)
            {
                reportingPerson = "SIT-465-Siddharth pd";
            }
            var reportingPersonCode = reportingPerson.Split('-')[0].Trim() + "-" + reportingPerson.Split('-')[1].Trim();

            employee.Employee2 = db.Employees.Where(m => m.EmployeeCode == reportingPersonCode).FirstOrDefault();
            employee.Department = db.Departments.FirstOrDefault(m => m.DepartmentID == departmentID);
            List<Task> TasksForUpdate = db.Tasks.Where(m => m.Employee2.EmployeeID == employee.EmployeeID).ToList();


            foreach(Task task in TasksForUpdate)
            {
                task.ApproverID = employee.Employee2.EmployeeID;

            }

            db.SaveChanges();
            return RedirectToAction("Director", "Home");

        }

        [Authorize(Roles = "Director")]
        public ActionResult DirManager()
        {
            List<Employee> employees = db.Employees.ToList();
            return View(employees); 
        }

        public ActionResult GetReportingPerson(int id) 
        {
            List<SelectListItem> items = db.Employees.Where( m => m.DepartmentId < id)//review this as director is at position 1 in my case
            .Select(emp => new SelectListItem
            {
                Text = emp.EmployeeCode + emp.FirstName + emp.LastName,
                Value = emp.EmployeeCode + emp.FirstName + emp.LastName,
            }).ToList();
            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetData(int id)
        {
            if(id == 0)
            {
                Employee emp = new Employee();
                return PartialView("_PartialPageTaskFOrm", emp);// have a look
            }
            else
            {
                Employee currentEmp = db.Employees.Find(id);

                var Employees = db.Employees.Where(m => m.DepartmentId != 1 && m.DepartmentId != 0 && m.DepartmentId != null).ToList();//have a look
                var Departments = db.Departments.ToList();
                var Emp_EmpListTuple = (currentEmp, Employees, Departments);
                return PartialView("_PartialPageTaskForm", Emp_EmpListTuple);// have a look
            }
        }

        //public ActionResult Delete(int id)
        //{
        //    var emp = db.Employees.Where(m => m.EmployeeID == id).FirstOrDefault();
        //    var emp_task = db.Tasks.Where(m => m.EmployeeID == emp.EmployeeID).ToList();
        //    foreach(var task in emp_task)
        //    {
        //        db.Tasks.Remove(task);
        //    }
        //    db.SaveChanges();
        //    db.Employees.Remove(emp);
        //    db.SaveChanges();
        //    return RedirectToAction("Director");

        //}
        public ActionResult Delete(int id)
        {
            var emp = db.Employees.FirstOrDefault(m => m.EmployeeID == id);
            if (emp != null)
            {
                // Delete tasks associated with the employee
                var emp_tasks = db.Tasks.Where(task => task.EmployeeID == emp.EmployeeID).ToList();
                foreach (var task in emp_tasks)
                {
                    db.Tasks.Remove(task);
                }

                // Save changes to remove tasks
                db.SaveChanges();

                // Now delete the employee
                db.Employees.Remove(emp);
                db.SaveChanges();
            }

            return RedirectToAction("Director");
        }

        [Authorize(Roles = "Director")]
        public ActionResult ManageManagerTask()
        {
            List<Task> tasks = db.Tasks.ToList();
            List<Employee>  empoyees = db.Employees.ToList();

            var data = (empoyees, tasks);
            return View(data);
        }
    }
}