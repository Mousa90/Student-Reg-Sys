using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using StudentRegSys.Dtos;
using StudentRegSys.Models;
using StudentRegSys.ViewModels;

namespace StudentRegSys.Controllers.Api
{
    public class MajorsController : ApiController
    {
        private ApplicationDbContext _context;
        public MajorsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/majors
        public IHttpActionResult GetMajors()
        {
            try
            {
                return Ok(_context.Major.ToList().Select(Mapper.Map<Major, MajorDto>));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET /api/majors/1
        public IHttpActionResult GetMajor(int id)
        {
            try
            {
                var majorInDb = _context.Major.SingleOrDefault(m => m.ID == id);

                if (majorInDb == null)
                    return NotFound();

                return Ok(Mapper.Map<Major, MajorDto>(majorInDb));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST /api/majors
        [HttpPost]
        public IHttpActionResult CreateMajor (MajorDto majorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (_context.Major.Any(m => m.Name == majorDto.Name))
                {
                    ModelState.AddModelError("Name", "This Major Name is already exists");
                    return BadRequest(ModelState);
                }

                var major = Mapper.Map<MajorDto, Major>(majorDto);
                _context.Major.Add(major);
                _context.SaveChanges();

                majorDto.ID = major.ID;

                return Created(new Uri(Request.RequestUri + "/" + major.ID), majorDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT /api/Majors/1
        [HttpPut]
        public IHttpActionResult UpdateMajor (int id, MajorDto majorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (_context.Major.Any(m => m.Name == majorDto.Name && m.ID != id))
                {
                    ModelState.AddModelError("Name", "This Major Name is already exists");
                    return BadRequest(ModelState);
                }

                var majorInDb = _context.Major.SingleOrDefault(m => m.ID == id);

                if (majorInDb == null)
                    return NotFound();
                Mapper.Map(majorDto, majorInDb);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //DELETE /api/majors/1
        [HttpDelete]
        public IHttpActionResult DeleteMajor(int id)
        {
            try
            {
                var majorInDb = _context.Major.SingleOrDefault(m => m.ID == id);

                if (majorInDb == null)
                    return NotFound();

                // check if thier Students with this Major
                if (_context.Student.Any(s => s.MajorID == id))
                    return BadRequest();


                _context.Major.Remove(majorInDb);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}