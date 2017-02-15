using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArrayOfPerson.Controllers;
using ArrayOfPerson.Models;
using static ArrayOfPerson.Controllers.HomeController;

namespace ArrayOfPerson.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        [TestMethod]
        public void sortOrder()
        {
            // Arrange
            HomeController controller = new HomeController();
            List<Person> persons = new List<Person>();
           
            string sortOrder;
            for (int i = 1; i < 10; i++)
            {
                Person person = new Person() {ID= i, Firstname="Name"+i.ToString(),Lastname="LastName"+i.ToString(), Socialnumber="123456-7890"+i.ToString(), PersonCategory="test" + i.ToString() };
                persons.Add(person);
            }

            for (int i = 0; i < 6; i++)
            {
                if(i == 1)
                {
                    sortOrder = "name_desc";
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 9;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
                else if(i == 2)
                {
                    sortOrder = "Social";
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 1;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
                else if (i == 3)
                {
                    sortOrder = "social_desc";
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 9;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
                else if (i == 4)
                {
                    sortOrder = "Type";
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 1;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
                else if (i == 5)
                {
                    sortOrder = "type_desc";
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 9;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
                else
                {
                    sortOrder = null;
                    // Act
                    IEnumerable<Person> result = controller.SortOrder(sortOrder, persons);
                    int accuatal = result.First().ID;
                    int expected = 1;
                    // Assert
                    Assert.AreEqual(accuatal, expected);
                }
               

            }
           
           
        }

        [TestMethod]
        public void search()
        {
            // Arrange
            HomeController controller = new HomeController();
            List<Person> persons = new List<Person>();

            string searchString = "Name1";
            string currentFilter = null;
            for (int i = 1; i < 10; i++)
            {
                Person person = new Person() { ID = i, Firstname = "Name" + i.ToString(), Lastname = "LastName" + i.ToString(), Socialnumber = "123456-7890" + i.ToString(), PersonCategory = "test" + i.ToString() };
                persons.Add(person);
            }

            // Act
            IEnumerable<Person> result = controller.SearchArray(currentFilter, searchString, persons);

            // Assert
            Assert.AreEqual(result.Count(), 1);
        }
        [TestMethod]
        public void viewPaging()
        {
            // Arrange
            HomeController controller = new HomeController();
            List<Person> persons = new List<Person>();
                        
            for (int i = 1; i < 15; i++)
            {
                Person person = new Person() { ID = i, Firstname = "Name" + i.ToString(), Lastname = "LastName" + i.ToString(), Socialnumber = "123456-7890" + i.ToString(), PersonCategory = "test" + i.ToString() };
                persons.Add(person);
            }
            IEnumerable<Person> result = persons as IEnumerable<Person>;
            ViewPagination view = new ViewPagination();
            int? pageSize = 10;
            int? page = 1;

            // Act
            ViewPagination viewPagination = controller.Pagination(result, view, pageSize, page);
           
            // Assert
            Assert.AreEqual(viewPagination.pageCount, "2");
            Assert.AreEqual(viewPagination.pageNumber, 1);
            Assert.AreEqual(viewPagination.pageSize, 10);
            Assert.AreEqual(viewPagination.viewResult.Count(), 10);

           
            page = 2;

            // Act
            viewPagination = controller.Pagination(result, view, pageSize, page);

            // Assert
            Assert.AreEqual(viewPagination.pageCount, "2");
            Assert.AreEqual(viewPagination.pageNumber, 2);
            Assert.AreEqual(viewPagination.pageSize, 10);
            Assert.AreEqual(viewPagination.viewResult.Count(), 4);

            page = null;

            // Act
            viewPagination = controller.Pagination(result, view, pageSize, page);

            // Assert
            Assert.AreEqual(viewPagination.pageCount, "2");
            Assert.AreEqual(viewPagination.pageNumber, 1);
            Assert.AreEqual(viewPagination.pageSize, 10);
            Assert.AreEqual(viewPagination.viewResult.Count(), 10);
        }
      
    }
}
