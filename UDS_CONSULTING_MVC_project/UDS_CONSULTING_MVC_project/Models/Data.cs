using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UDS_CONSULTING_MVC_project.Models
{
    public class Data
    {
        public string guid { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string about { get; set; }
        public string registered { get; set; }
        public List<string> tags { get; set; }
    }
}