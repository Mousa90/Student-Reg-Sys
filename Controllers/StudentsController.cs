using StudentRegSys.Dtos;
using StudentRegSys.Models;
using StudentRegSys.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace StudentRegSys.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext _context;
        public StudentsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Students/StudentsManagement
        public ActionResult StudentsManagement()
        {
            try
            {
                var students = _context.Student.ToList();

                return View(students);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        // GET: Students/StudentForm
        public ActionResult StudentForm()
        {
            try
            {
                var student = new Student();

                var majors = _context.Major.ToList();
                var viewModel = new StudentViewModel
                {
                    Student = student,
                    Majors = majors
                };

                viewModel.IsAdd = true;

                return View(viewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

        //GET: Students/Edit/1
        public ActionResult Edit(int id)
        {
            try
            {
                var student = _context.Student.SingleOrDefault(s => s.ID == id);

                if (student == null)
                    return HttpNotFound();

                var majors = _context.Major.ToList();
                var viewModel = new StudentViewModel
                {
                    Student = student,
                    Majors = majors
                };

                viewModel.IsAdd = false;
                return View("StudentForm", viewModel);
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Student student)
        {
            try
            {
                var majors = _context.Major.ToList();
                var viewModel = new StudentViewModel
                {
                    Student = student,
                    Majors = majors
                };

                if (!ModelState.IsValid)
                    return View("StudentForm", viewModel);

                // To get the path and generate Name for the new uploded image
                HttpPostedFileBase file = null;
                var path = "";


                if (Request.Files.Count > 0)
                {
                    file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileExtension = Path.GetExtension(file.FileName);
                        var fileName = "StudentPic_" + student.Name + fileExtension;
                        path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        student.Photo = "/Images/" + fileName;
                    }
                }

                // Add
                if (student.ID == 0)
                {
                    if (_context.Student.Any(s => s.Name == student.Name))
                    {
                        ModelState.AddModelError("Student.Name", "This Student Name is already exists");
                        viewModel.IsAdd = true;
                        return View("StudentForm", viewModel);
                    }
                    _context.Student.Add(student);
                }
                // Edit
                else
                {
                    if (_context.Student.Any(s => s.Name == student.Name && s.ID != student.ID))
                    {
                        ModelState.AddModelError("Student.Name", "This Student Name is already exists");
                        viewModel.IsAdd = false;
                        return View("StudentForm", viewModel);
                    }

                    var studentInDb = _context.Student.Single(s => s.ID == student.ID);

                    string old_path = Server.MapPath(studentInDb.Photo);
                    FileInfo fi = new FileInfo(old_path);
                    if (fi.Exists)
                        fi.Delete();

                    studentInDb.Name = student.Name;
                    studentInDb.DateofBirth = student.DateofBirth;
                    studentInDb.Gender = student.Gender;
                    studentInDb.Address = student.Address;
                    studentInDb.Phone = student.Phone;
                    studentInDb.Photo = student.Photo;
                    studentInDb.MajorID = student.MajorID;
                }

                if (path != "")
                    // to crop and save the uploaded pic
                    /* for automatically crop the image in largest 1:1 ratio (sequre) possible enter 0's in
                    the maxWidth & maxHeight parametesrs */
                    SaveCroppedImage(Image.FromStream(file.InputStream), 0, 0, path); 

                _context.SaveChanges();
                return RedirectToAction("StudentsManagement");
            }
            catch (Exception)
            {
                return View("StudentForm", student);
            }
        }

        // DELETE: Students/Delete/1
        public ActionResult Delete(int id)
        {
            try
            {
                var student = _context.Student.SingleOrDefault(s => s.ID == id);

                if (!(student == null))
                {
                    _context.Student.Remove(student);

                    // to delete saved image
                    string path = Server.MapPath(student.Photo);
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                        fi.Delete();
                }
                _context.SaveChanges();
                return RedirectToAction("StudentsManagement");
            }
            catch (Exception)
            {
                return RedirectToAction("StudentsManagement");
            }
        }

        //Helper method
        public bool SaveCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;

            //to encodes the rotation/flipping necessary to display the image correctly
            //PropertyItem pi = image.PropertyItems.Select(x => x)
            //       .FirstOrDefault(x => x.Id == 0x0112);


            //byte o = pi.Value[0];

            //if (o == 2) image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //if (o == 3) image.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            //if (o == 4) image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //if (o == 5) image.RotateFlip(RotateFlipType.Rotate90FlipX);
            //if (o == 6) image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //if (o == 7) image.RotateFlip(RotateFlipType.Rotate90FlipY);
            //if (o == 8) image.RotateFlip(RotateFlipType.Rotate90FlipXY);

            System.Drawing.Bitmap bitmap = null;
            try
            {
                //to automatically take the largest sequre dimensions possible 
                if (maxWidth == 0 && maxHeight == 0)
                {
                    if (image.Width > image.Height)
                    {
                        maxWidth = image.Width;
                        maxHeight = image.Width;
                    }
                    else
                    {
                        maxWidth = image.Height;
                        maxHeight = image.Height;
                    }
                }

                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }
    }
}