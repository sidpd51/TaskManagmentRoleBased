using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagmentRoleBased.Models;
//using TaskManagmentRoleBased.Filter;

namespace TaskManagmentRoleBased.Controllers
{
    
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        private office_sidpd_newEntities db = new office_sidpd_newEntities();

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Employee emp)
        {
            var flag = db.Employees.FirstOrDefault(e => e.Email == emp.Email);
            if(flag != null)
            {
                ModelState.AddModelError("Email", "Email already exists!");//changed the key from email to Email
                return View(emp);
            }
            if(ModelState.IsValid)
            {
                emp.DepartmentId = 3;
                emp.ReportingPerson = 1;
                db.Employees.Add(emp);  
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(emp);
        }

        public ActionResult Login() { return View(); }

        [HttpPost]
        public ActionResult Login(Employee emp, string ReturnUrl)
        {
            if (IsValid(emp) == true)
            {
                var employee = db.Employees.FirstOrDefault(e => e.Email == emp.Email);
                FormsAuthentication.SetAuthCookie(emp.Email, false);
                Session["username"] = employee.FirstName + " " + employee.LastName;
                Session["id"] = employee.EmployeeID;
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    if(employee.DepartmentId == 1)
                    {
                        return RedirectToAction("Director", "Home");
                    }
                    else if(employee.DepartmentId == 2)
                    {
                        return RedirectToAction("Manager", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(emp);
        }

        public bool IsValid(Employee emp)
        {
            var credentials = db.Employees.Where(model => model.Email == emp.Email && model.Password == emp.Password).FirstOrDefault();    
            if(credentials != null)
            {
                return (credentials.Email == emp.Email && credentials.Password == emp.Password);
            }
            else
            {
                return false;
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Authentication");
        }
    }
}