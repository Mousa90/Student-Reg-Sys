using StudentRegSys.Dtos;
using StudentRegSys.Models;
using StudentRegSys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace StudentRegSys.Controllers
{
    public class MajorsController : Controller
    {
        private ApplicationDbContext _context;
        public MajorsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Majors/MajorsManagement
        public ActionResult MajorsManagement()
        {
            try
            {
                var majors = _context.Major.ToList();

                return View(majors);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: Majors/MajorForm
        public ActionResult MajorForm()
        {
            try
            {
                var major = new Major();

                major.IsAdd = true;

                return View(major);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        //GET: Majors/Edit/1
        public ActionResult Edit(int id)
        {
            try
            {
                var major = _context.Major.SingleOrDefault(m => m.ID == id);

                if (major == null)
                    return HttpNotFound();

                major.IsAdd = false;
                return View("MajorForm", major);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Major major)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("MajorForm", major);

                // Add
                if (major.ID == 0)
                {
                    if (_context.Major.Any(m => m.Name == major.Name))
                    {
                        ModelState.AddModelError("Name", "This Major Name is already exists");
                        major.IsAdd = true;
                        return View("MajorForm", major);
                    }
                    _context.Major.Add(major);
                }
                // Edit
                else
                {
                    if (_context.Major.Any(m => m.Name == major.Name && m.ID != major.ID))
                    {
                        ModelState.AddModelError("Name", "This Major Name is already exists");
                        major.IsAdd = false;
                        return View("MajorForm", major);
                    }

                    var majorInDb = _context.Major.Single(m => m.ID == major.ID);
                    majorInDb.Name = major.Name;
                }
                _context.SaveChanges();
                return RedirectToAction("MajorsManagement");
            }
            catch (Exception)
            {
                return View("MajorForm", major);
            }
        }

        //Save method using web Api
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveApi(MajorDto majorDto)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:63349/api/");

                    //HTTP POST
                    if (majorDto.ID == 0)
                    {
                        var postTask = client.PostAsJsonAsync<MajorDto>("majors", majorDto);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (!result.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                            return RedirectToAction("MajorForm", majorDto);
                        }
                    }
                    //HTTP PUT
                    else
                    {
                        var putTask = client.PutAsJsonAsync<MajorDto>("majors/" + majorDto.ID.ToString(), majorDto);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (!result.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                            return RedirectToAction("MajorForm", majorDto);
                        }
                    }

                    return RedirectToAction("MajorsManagement");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("MajorForm", majorDto);
            }
        }

        // DELETE: Students/Delete/1
        public ActionResult Delete(int id)
        {
            try
            {
                var major = _context.Major.SingleOrDefault(m => m.ID == id);

                if (!(major == null))
                {
                    _context.Major.Remove(major);
                }
                _context.SaveChanges();
                return RedirectToAction("MajorsManagement");
            }
            catch (Exception)
            {
                return RedirectToAction("MajorsManagement");
            }
        }

    }
}