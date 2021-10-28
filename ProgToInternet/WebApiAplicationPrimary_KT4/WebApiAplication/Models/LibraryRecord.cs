using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAplication.Models
{
    public class LibraryRecord
    {
        public int Id { get; set; }

        public DateTime LastUpdateTime { get; set; }


        private string _bookName;

        public string BookName
        {
            get { return _bookName; }
            set 
            {
                _bookName = value;
                LastUpdateTime = DateTime.Now;
            }
        }
    }
}
