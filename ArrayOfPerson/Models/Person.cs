using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrayOfPerson.Models
{
    [Serializable()]
    public class Person
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Socialnumber { get; set; }
        public string PersonCategory { get; set; }
    }
}