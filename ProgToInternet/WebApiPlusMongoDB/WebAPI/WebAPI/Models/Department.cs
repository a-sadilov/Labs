using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Department
    {
        public ObjectId Id {get; set;}
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }



    }
}
