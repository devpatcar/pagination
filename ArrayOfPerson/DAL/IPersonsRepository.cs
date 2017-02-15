using ArrayOfPerson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrayOfPerson.DAL
{
    public interface IPersonsRepository:IDisposable
    {
        IEnumerable<Person> InitPersons();           
        void Save(List<Person> person);
    }
}