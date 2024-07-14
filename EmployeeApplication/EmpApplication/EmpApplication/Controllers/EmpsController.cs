using EmpApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EmpApplication.Controllers
{
    public class EmpsController : Controller
    {
        string BaseAddress = ConfigurationManager.AppSettings["Issuer"];
        // GET: Emps
        public ActionResult Index(string search = null, string sortField = "Name",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 3)
        {
            EmpDBContext context = new EmpDBContext();
            List<Employee> employeeList = new List<Employee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                var responseTask = client.GetAsync("api/Employees?search=" + search + "&sortField=" + sortField + "&sortOrder=" + sortOrder + "&page=" + page + "&pageSize=" + pageSize);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    var jsonData = JObject.Parse(jsonString.Result.ToString());
                    var employeesData = jsonData["Data"].ToObject<List<Employee>>();
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    employeeList = employeesData;
                    //var data = JsonConvert.DeserializeObject<List<Employee>>(d);
                    //employeeList = JsonConvert.DeserializeObject<List<Employee>>(readTask.Result.ToString());
                }
                else //web api sent error response 
                {
                    //log response status here..   
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(employeeList);
        }

        public ActionResult Create(Employee employee)
        {
            if(employee == null)
            {
                employee = new Employee();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(BaseAddress);
                        var responseTask = client.PostAsJsonAsync("api/Employees?employee=" , employee);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return View("Index", new Employee());
                        }

                    }
                }
            }
            return View(employee);
        }

    }
}