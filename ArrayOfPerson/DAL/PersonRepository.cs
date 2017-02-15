using System;
using System.Collections.Generic;
using ArrayOfPerson.Models;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace ArrayOfPerson.DAL
{
    public class PersonsRepository : IPersonsRepository
    {
       
        public void Dispose()
        {
            throw new NotImplementedException();
        }       

        public IEnumerable<Person> InitPersons()
        {
            XmlDocument xmldoc = new XmlDocument();
            using (FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/DAL/personer.xml", FileMode.Open))
            {
                xmldoc.Load(fileStream);
            }

            XmlNodeList settings = xmldoc.SelectNodes("/ArrayOfPerson");
            XmlNodeList defaults = xmldoc.GetElementsByTagName("Person");

            List<Person> persons = new List<Person>();

            foreach (XmlNode node in defaults)
            {
                Person person = new Person();
                person.ID = int.Parse(node["ID"].InnerText);
                person.Firstname = node["Firstname"].InnerText;
                person.Lastname = node["Lastname"].InnerText;
                person.Socialnumber = node["Socialnumber"].InnerText;
                person.PersonCategory = node["PersonCategory"].InnerText;
                persons.Add(person);
            }

            return persons;
        }

      

        public void Save(List<Person> person)
        {
            //create the serialiser to create the xml
            XmlSerializer serialiser = new XmlSerializer(typeof(List<Person>));
            TextWriter Filestream = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "/DAL/personer.xml");
            //write to the file
            serialiser.Serialize(Filestream, person);
            // Close the file
            Filestream.Close();
        }

    }
}