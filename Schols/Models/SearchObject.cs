using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schols.Models
{
    public class SearchObject
    {
        public string title {get; set;}
        public string purpose { get; set; }
        public string department { get; set; }
        public string college { get; set; }
        public string schoolYear { get; set; }
        public string major { get; set; }
        public string undergradGPA { get; set; }
        public string gradGPA { get; set; }
        public string highschoolGPA { get; set; }
        public string keyword { get; set; }
    }
}