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
        private office_sidpd_newEntities db = new office_sidpd_newEntities(); //access modified taken as private here and in authentication controller check it out
        
        public ActionResult Index()
        {
            int employeeId = (int)Convert.ToInt64(Session["id"]);
            if(employeeId == 0) // 
            {
                Employee emp = new Employee();//instantiates a new employees
                return View(emp); // return the view named "index" with the newly created Employee object
            }
            Employee employee = db.Employees.Find(employeeId);
            return View();
        }

        public ActionResult Manager()
        {
            int id = (int)Convert.ToInt64(Session["id"]);
            Employee emp = db.Employees.Find(id);

            return View(emp);
        }

        public ActionResult EmployeeTask()
        {
            var id = (int)Convert.ToInt64(Session["id"]);

            Employee emp = db.Employees.Where(e => e.EmployeeID == id).FirstOrDefault();

            List<Employee> employees = db.Employees.ToList();
            List<Task> tasks = db.Tasks.Where( m => m.ApproverID == emp.EmployeeID).ToList(); 
            var data = (employees, tasks);//creating tuples

            return View(data);
        }

        public ActionResult DeleteTask(int TaskId)
        {
            var task = db.Tasks.Where(m => m.TaskID == TaskId).FirstOrDefault();
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Task");//Why returning to Task action method if it doesn't exists? below is the action method :)
        }

        public ActionResult Task()
        {
            var id = (int)Convert.ToInt64(Session["id"]);
            Employee emp = db.Employees.Where(m => m.EmployeeID == id).FirstOrDefault();
            List<Employee> employees = db.Employees.ToList();
            List<Task> tasks = db.Tasks.Where(m => m.EmployeeID == emp.EmployeeID).ToList();
            var data = (emp, employees, tasks);
            return View(data);
        }

        public ActionResult GetTaskData (int TaskId)
        {
            if(TaskId == 0)
            {
                Task task = new Task();
                return PartialView("_PartialPageTask", task);
            }
            else
            {
                Task task = db.Tasks.Find(TaskId);
                return PartialView("_PartialPageTask", task );
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
                // task.Status = 'Pending';  By default my db has this
                if(task.TaskID == 0)
                {
                    task.CreatedOn = DateTime.Now;// i think By default my db has this 
                }
                else
                {
                    Task currentTask = db.Tasks.Where(t => t.TaskID == task.TaskID).FirstOrDefault();
                    task.CreatedOn = currentTask.CreatedOn;

                }
                task.ApproverID = emp.Employee2.EmployeeID;// why using employee2
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

        public ActionResult Director()
        {
            List<Employee>  employees = db.Employees.ToList();
            return View(employees);
        }

        [HttpPost]
        public ActionResult UpdatedData(string employeeCode, int departmentID, string reportingPerson)
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

        public PartialViewResult GetData(int empId)
        {
            if(empId == 0)
            {
                Employee emp = new Employee();
                return PartialView("_PartialPageTaskFOrm", emp);// have a look
            }
            else
            {
                Employee currentEmp = db.Employees.Find(empId);

                var Employees = db.Employees.Where(m => m.DepartmentId != 1 && m.DepartmentId != 0 && m.DepartmentId != null).ToList();//have a look
                var Departments = db.Departments.ToList();
                var Emp_EmpListTuple = (currentEmp, Employees, Departments);
                return PartialView("_PartialPageTaskForm", Emp_EmpListTuple);// have a look
            }
        }

        public ActionResult Delete(int empId)
        {
            var emp = db.Employees.Where(m => m.EmployeeID == empId).FirstOrDefault();
            var emp_task = db.Tasks.Where(m => m.EmployeeID == emp.EmployeeID).ToList();
            db.Employees.Remove(emp);
            foreach(var task in emp_task)
            {
                db.Tasks.Remove(task);
            }
            db.SaveChanges();
            return RedirectToAction("Director");
                
        }

        public ActionResult ManageManagerTask()
        {
            List<Task> tasks = db.Tasks.ToList();
            List<Employee>  empoyees = db.Employees.ToList();

            var data = (empoyees, tasks);
            return View(data);
        }
    }
}