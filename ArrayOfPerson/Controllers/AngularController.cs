using ArrayOfPerson.DAL;
using ArrayOfPerson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArrayOfPerson.Controllers
{
    public class AngularController : Controller
    {
        private IPersonsRepository personsRepository;
        private IEnumerable<Person> data;
        private List<Person> context;
        public AngularController()
        {
            this.personsRepository = new PersonsRepository();
        }
        public ActionResult Index()
        {
            return View();
        }
        // GET: Angular
        public JsonResult Data(string sortOrder,string currentFilter, string searchString, int? page, int? pageSize, string action)
        {
            ViewPagination view = new ViewPagination();
           
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
            
            // search
            result = SearchArray(searchString, result);

            // filter
            result = SortOrder(sortOrder, result);

            // set up pagination
            view = Pagination(result, view, pageSize, page);

            return Json(view, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]     
        public ActionResult Create(Person person)
        {
            bool create;
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

                    create = true;
                }
                else
                {
                    create = false;
                }
            }
            catch (Exception)
            {
                create = false;
            }
            return Json(create, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save()
        {
            bool saved;
            try
            {
                data = Session["Persons"] as IEnumerable<Person>;               
                personsRepository.Save(data.ToList());
                saved = true;
            }
            catch (Exception)
            {
                saved = false;
            }
            
            return Json(saved, JsonRequestBehavior.AllowGet); 
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
            int viewPage = page == null ? 0 : page.Value - 1;
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

        public IEnumerable<Person> SearchArray(string searchString, IEnumerable<Person> result)
        {          

            if (!String.IsNullOrEmpty(searchString))
            {               
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