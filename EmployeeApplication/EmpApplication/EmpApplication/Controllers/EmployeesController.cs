using EmpApplication.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmpApplication.Controllers
{
    [Route("api/Employees")]
    public class EmployeesController : ApiController
    {
        private readonly EmpDBContext _context;
        public EmployeesController()
        {
            _context = new EmpDBContext();
        }

        public EmployeesController(EmpDBContext context)
        {
            _context = context;
        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult Get(string search = null,
            string sortField = "Name",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 3)
        {
            IQueryable<Employee> query = _context.Employees;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.Name.Contains(search) ||
                    e.EmployeeId.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.Mobile.ToString().Contains(search));
            }
            switch (sortField.ToLower())
            {
                case "name":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "employeeid":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.EmployeeId) : query.OrderByDescending(e => e.EmployeeId);
                    break;
                case "email":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                    break;
                case "mobile":
                    query = sortOrder.ToLower() == "asc" ? query.OrderBy(e => e.Mobile) : query.OrderByDescending(e => e.Mobile);
                    break;
                default:
                    query = query.OrderBy(e => e.Name);
                    break;
            }
            var totalCount = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var employees = query.ToList();

            return Ok(new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = employees
            });
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult AddEmployee(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(new { id = employee.EmployeeId });
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IHttpActionResult Put(string id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                var employee = _context.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChangesAsync();
                    return Ok("Deleted");
                }
            }
            catch (Exception)
            {
                throw;
            }
            //return NotFound();
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

    }
}