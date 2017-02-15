using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using ArrayOfPerson.Models;
using ArrayOfPerson.DAL;
using System.Data;

namespace ArrayOfPerson.Controllers
{
    public class HomeController : Controller
    {       
        private IPersonsRepository personsRepository;
        private IEnumerable<Person> data;
        private List<Person> context;
        public HomeController()
        {
            this.personsRepository = new PersonsRepository();            
        }
        //PagedList
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize,string action)
        {
            ViewPagination view = new ViewPagination();

            List<int> pageSizeList = new List<int>() { 10, 50, 100 };
            ViewBag.pageSize = pageSizeList;

            if (Session.IsNewSession)
            {
                data = personsRepository.InitPersons();
                Session["Persons"] = data;
            }
            else
            {
                data = Session["Persons"] as IEnumerable<Person>;
            }

            var result = data;

            ViewBag.Saved = SetMessage(action);
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SocialSortParm = sortOrder == "Social" ? "social_desc" : "Social";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.CurrentSort = sortOrder; 
            ViewBag.selectedPageSize = pageSize;
            ViewBag.page = page;

            // search
            result = SearchArray(currentFilter,searchString, result);
           
            // filter
            result = SortOrder(sortOrder, result);

            // set up pagination
            view = Pagination(result, view, pageSize, page);
            
            return View(view);

        }

        private dynamic SetMessage(string action)
        {
            string result = "";
            if (action == "saved")
            {
                result = "The list have been saved";
            }
            else if (action == "edit")
            {
                result = "The list have been edited";
            }
         
            return result;
        }

        public ActionResult Save()
        {
            data = Session["Persons"] as IEnumerable<Person>;
            personsRepository.Save(data.ToList());

            return RedirectToAction("Index", new { action = "saved" });
        }
    
        public ActionResult Create()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    [   Bind(Include = "firstName, lastName, socialnumber, personCategory")]
        Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    data = Session["Persons"] as IEnumerable<Person>;
                    context = data.ToList();
                    person.ID = context.Count() + 1;
                    context.Add(person);
                    data = context as IEnumerable<Person>;
                    Session["Persons"] = data;
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {              
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(person);
        }
        public class ViewPagination
        {
            public int pageSize { get; set; }
            public int pageNumber { get; set; }
            public string pageCount { get; set; }
            public IEnumerable<Person> viewResult { get; set; }
        }
        public IEnumerable<Person> SortOrder(string sortOrder, IEnumerable<Person> result)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    result = result.OrderByDescending(s => s.Firstname);
                    break;
                case "Social":
                    result = result.OrderBy(s => s.Socialnumber);
                    break;
                case "social_desc":
                    result = result.OrderByDescending(s => s.Socialnumber);
                    break;
                case "Type":
                    result = result.OrderBy(s => s.PersonCategory);
                    break;
                case "type_desc":
                    result = result.OrderByDescending(s => s.PersonCategory);
                    break;
                default:
                    result = result.OrderBy(s => s.ID);
                    break;
            }
            return result;
        }
        public ViewPagination Pagination(IEnumerable<Person> result, ViewPagination view, int? pageSize, int? page)
        {
            decimal viewPageSize = pageSize == null ? 10 : pageSize.Value;
            int viewPage = page == null ? 0 : page.Value-1;
            decimal resultCount = result.Count();           
            view.pageSize = (int)viewPageSize;
            decimal calcPageCount = resultCount / viewPageSize;
            view.pageCount = Math.Ceiling(calcPageCount).ToString(); 
            if (view.pageCount == "0")
            {
                view.pageCount = "1";
            }           
            view.viewResult = result.Skip(((int)viewPage) * (int)viewPageSize).Take((int)viewPageSize);
            view.pageNumber = page == null ? 1 : page.Value;
            return view;
        }

        public IEnumerable<Person> SearchArray(string currentFilter, string searchString, IEnumerable<Person> result)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;
                searchString = searchString.ToLower();
                result = result.Where(s => s.Firstname.ToLower().Contains(searchString)
                                       || s.Lastname.ToLower().Contains(searchString)
                                       || s.Socialnumber.ToLower().Contains(searchString)
                                       || s.PersonCategory.ToLower().Contains(searchString));
            }
            return result;
        }

    }


}