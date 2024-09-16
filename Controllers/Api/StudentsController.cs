using System;
using System.Collections.Generic;
using System.IO;
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
    public class StudentsController : ApiController
    {
        private ApplicationDbContext _context;
        public StudentsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/students
        public IHttpActionResult GetStudents()
        {
            try
            {
                return Ok(_context.Student.ToList().Select(Mapper.Map<Student, StudentDto>));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET /api/students/1
        public IHttpActionResult GetStudent(int id)
        {
            try
            {
                var studentInDb = _context.Student.SingleOrDefault(s => s.ID == id);

                if (studentInDb == null)
                    return NotFound();

                return Ok(Mapper.Map<Student, StudentDto>(studentInDb));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST /api/students
        [HttpPost]
        public IHttpActionResult CreateStudent(StudentDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (_context.Student.Any(s => s.Name == studentDto.Name))
                {
                    ModelState.AddModelError("Name", "This Student Name is already exists");
                    return BadRequest(ModelState);
                }

                var student = Mapper.Map<StudentDto, Student>(studentDto);
                _context.Student.Add(student);
                _context.SaveChanges();

                studentDto.ID = student.ID;

                return Created(new Uri(Request.RequestUri + "/" + student.ID), studentDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT /api/students/1
        [HttpPut]
        public IHttpActionResult UpdateStudent(int id, StudentDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (_context.Student.Any(s => s.Name == studentDto.Name && s.ID != id))
                {
                    ModelState.AddModelError("Name", "This Student Name is already exists");
                    return BadRequest(ModelState);
                }

                var StudentInDb = _context.Student.SingleOrDefault(s => s.ID == id);

                if (StudentInDb == null)
                    return NotFound();
                Mapper.Map(studentDto, StudentInDb);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //DELETE /api/students/1
        [HttpDelete]
        public IHttpActionResult DeleteStudent(int id)
        {
            try
            {
                var studentInDb = _context.Student.SingleOrDefault(s => s.ID == id);

                if (studentInDb == null)
                    return NotFound();

                _context.Student.Remove(studentInDb);
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

