using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuartoin, IWebHostEnvironment env)
        {
            _configuration = configuartoin;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }
        [HttpPost]
        public JsonResult Post(Employee employers)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int lastEmployeeId = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();
            employers.EmployeeId = lastEmployeeId + 1;
            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(employers);

            return new JsonResult("Added successfully");
        }
        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employee.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                .Set("Department", employee.Department)
                .Set("DateOfRegistration", employee.DateOfRegistration)
                .Set("PhotoFileName", employee.PhotoFileName);

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Updated successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deleted successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var HttpRequest = Request.Form;
                var postedFile = HttpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var steam = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(steam);
                }
                return new JsonResult("File posted");
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }

        }
    }
}
