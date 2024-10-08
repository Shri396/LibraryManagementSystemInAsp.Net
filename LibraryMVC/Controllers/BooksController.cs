﻿using LibraryMVC.Models;
using LibraryMVC.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly object _BookRepository;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var Books = context.Books.OrderByDescending(p => p.Id).ToList();
            return View(Books);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookDto bookDto)
        {
            if (bookDto.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFileName", "The image file is required");

            }

            if (!ModelState.IsValid)
            {
                return View(bookDto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookDto.ImageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                bookDto.ImageFileName.CopyTo(stream);
            }


            Book book = new Book()
            {
                Name = bookDto.Name,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                Price = bookDto.Price,
                Description = bookDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };


            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }




        public IActionResult Edit(int id)
        {
            var book = context.Books.Find(id);


            if (book == null)
            {

                return RedirectToAction("Index", "Books");
            }
            var bookDto = new Book()
            {
                Name = book.Name,
                Author = book.Author,
                Genre = book.Genre,
                Price = book.Price,
                Description = book.Description,

            };


            ViewData["BookId"] = book.Id;
            ViewData["ImageFileName"] = book.ImageFileName;
            ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");
            return View(bookDto);
        }


        [HttpPost]
        public IActionResult Edit(int id, BookDto bookDto)
        {
            var book = context.Books.Find(id);


            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }




            if (!ModelState.IsValid)
            {

                ViewData["BookId"] = book.Id;
                ViewData["ImageFileName"] = book.ImageFileName;
                ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");
                return View(bookDto);
            }


            //update the image if we have a new image file
            string newFileName = book.ImageFileName;
            if (bookDto.ImageFileName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bookDto.ImageFileName.FileName);

                string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.ImageFileName.CopyTo(stream);
                }



                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/books/" + book.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }



            //update the Books database
            book.Name = bookDto.Name;
            book.Author = bookDto.Author;
            book.Genre = bookDto.Genre;
            book.Price = bookDto.Price;
            book.Description = bookDto.Description;
            book.ImageFileName = newFileName;


            context.SaveChanges();
            return RedirectToAction("Index", "Books");


        }





        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

                string imageFullPath = environment.WebRootPath + "/books/" + book.ImageFileName;
                System.IO.File.Delete(imageFullPath);


                context.Books.Remove(book);
                context.SaveChanges(true);

                return RedirectToAction("Index", "Books");



            }

        }

}


