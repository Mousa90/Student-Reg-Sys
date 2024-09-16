using StudentRegSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList.Mvc;
using PagedList;

namespace StudentRegSys.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: /Home/Index
        public ViewResult Index()
        {
            return View();
        }

        // GET: /Home/StudentsCards
        public ActionResult StudentsCards(int? i, string search = "")
        {
            try
            {
                var students = _context.Student.Include(s => s.Major)
                    .OrderBy(s => s.Name)
                    .Where(s => s.Name.Contains(search) || s.Major.Name.Contains(search) ||
                    s.Address.Contains(search) || s.Phone.Contains(search))
                    .ToList().ToPagedList(i ?? 1, 8);

                ViewBag.StudentsSearch = search;
                return View(students);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: /Home/StudentProfile/1
        public ActionResult StudentProfile(int id)
        {
            try
            {
                var students = _context.Student.Include(s => s.Major);
                var student = students.SingleOrDefault(s => s.ID == id);

                return View(student);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: /Home/About
        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        // Action Result for errors test
        // GET: /Home/Error
        public ActionResult Error()
        {
            throw new Exception();
        }
    }
}