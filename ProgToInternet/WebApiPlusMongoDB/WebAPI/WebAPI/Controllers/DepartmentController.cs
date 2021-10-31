using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuartoin)
        {
            _configuration = configuartoin;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);
        }
        [HttpPost]
        public JsonResult Post(Department department) 
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int lastDepartmentId = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();
            department.DepartmentId = lastDepartmentId + 1;
            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(department);

            return new JsonResult("Added successfully");
        }
        [HttpPut]
        public JsonResult Put(Department department)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Department>.Filter.Eq("DepartmentId",department.DepartmentId);
            var update = Builders<Department>.Update.Set("DepartmentName", department.DepartmentName);

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(filter, update);

            return new JsonResult("Updated successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Department>.Filter.Eq("DepartmentId", id);

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(filter);

            return new JsonResult("Deleted successfully");
        }
    }
}
