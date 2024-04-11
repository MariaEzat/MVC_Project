﻿using Microsoft.AspNetCore.Mvc;
using MVC_Project.Interfaces;
using MVC_Project.ViewModel;

namespace MVC_Project.Controllers
{
	public class AuthorController : Controller
	{
	    IAuthor author;
		private readonly IWebHostEnvironment hostingEnvironment;
		public AuthorController(IAuthor _author, IWebHostEnvironment _hostingEnvironment)
		{
			author = _author;
			hostingEnvironment = _hostingEnvironment;
		}
		public IActionResult Index()
		{
			List<Author> authorList = author.GetAllAuthors();
			return View("Index", authorList);
		}

		/// <summary>
		/// should not return a view
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IActionResult GetAuthorsByName(string name)
		{
			var authors = author.GetAuthorsByName(name);

			return View("GetAuthorByName", authors);
		}

		public IActionResult details(int id)
		{
			Author author1 = author.GetAuthorById(id);
			author1.Books = author.GetAuthorBooks(author1.AuthorName);

			return View("details", author1);
		}
		[HttpGet]
		//add authorize
		public IActionResult AddNewAuthor(Author author)
		{
			return View("AddNewAuthor");
		}
		[HttpPost]
		public async Task<IActionResult> SaveAuthorAsync(AuthorName_Biography_BD_Country_ImgUrlViewModel AuthModel, IFormFile ImageUrl)
		{
			if (ModelState.IsValid == false)
			{
				return View("AddNewAuthor", AuthModel);
			}

			Author authorData = new Author()
			{
				AuthorName = AuthModel.AuthorName,
				BirthDate = AuthModel.BirthDate,
				Biography = AuthModel.Biography,
				Country = AuthModel.Country
			};

			if (ImageUrl != null && ImageUrl.Length > 0)
			{
				var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageUrl.FileName);
				var filePath = Path.Combine("wwwroot/images", fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await ImageUrl.CopyToAsync(stream);
				}

				authorData.ImageUrl = "/images/" + fileName;
			}

			author.InsertAuthor(authorData);
			author.Save();

			return RedirectToAction("Index");
		}

		//edit author

		[HttpGet]
		public IActionResult EditAuthor(int id)
		{
			author.UpdateAuthor(id);
			return View("Edit");
			
		}

		//delete author






	}
}
