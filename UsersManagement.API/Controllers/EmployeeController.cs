using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.API.Security;
using UsersManagement.Domain.Employee;


namespace UsersManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [HttpHead]
        [HttpOptions]
        [Authorize]
        public async Task<ActionResult<List<Employee>>> GetEmployee()
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";

            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A3";

            lstEmp.Add(obj);


            
            return lstEmp;
        }

        

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";

            return obj;
        }

        [HttpPost]
        public async Task<ActionResult<List<Employee>>> PostEmployee(Employee objPost)
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";

            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A3";

            lstEmp.Add(obj);

            lstEmp.Add(objPost);
            return lstEmp;
        }

        [HttpPut]
        public async Task<ActionResult<List<Employee>>> PutEmployee(string ID, Employee objPost)
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";

            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A3";

            lstEmp.Add(obj);

            lstEmp.Where(a => a.Name == ID).ToList().ForEach(a => a.Email = "a@yahoo.com");
            return lstEmp;
        }

        [HttpDelete]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(string ID)
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";

            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A3";

            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A4";

            lstEmp.Add(obj);

            lstEmp.Remove(lstEmp.FirstOrDefault(a => a.Name == ID));
            return lstEmp;
        }



        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployeeWithPhone()
        {
            List<Employee> lstEmp = new List<Employee>();
            Employee obj = new Employee();
            obj.Name = "A1";
            obj.Phone = "121212121";
            lstEmp.Add(obj);

            obj = new Employee();
            obj.Name = "A3";
            obj.Phone = "24545454";

            lstEmp.Add(obj);
            return lstEmp;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>> SaveEmployeeTask(Employee emp)
        {
            List<string> lstEmp = new List<string>();
           
            lstEmp.Add("A1");

           
            lstEmp.Add("A2");

            lstEmp.Add("strTask");
            return lstEmp;
        }
    }
}