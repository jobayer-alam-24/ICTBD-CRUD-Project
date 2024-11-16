using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ICTBD_CRUD_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICTBD_CRUD_Project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CRUD_Operations_DbContext _CONTEXT;
        private readonly IWebHostEnvironment _env;
        public ProductsController(IWebHostEnvironment env)
        {
            _CONTEXT = new CRUD_Operations_DbContext();
            _env = env;
        }
        public IActionResult Index()
        {
            //Select from DB and convert to List 
            var products = _CONTEXT.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = _CONTEXT.Products.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image File is Required!");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            //Save image file 
            //!unique name
            string newFile = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFile += Path.GetExtension(productDto.ImageFile!.FileName);
            //! = usniqueFilename.jpg or .png...
            //Then find Image full Path
            string imageFullPath = _env.WebRootPath + "/images/" + newFile;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFile,
                CreatedAt = DateTime.Now
            };
            _CONTEXT.Products.Add(product);
            _CONTEXT.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
        public IActionResult Edit(int id)
        {
            var product = _CONTEXT.Products.Find(id);
            if (product == null) return RedirectToAction("Index", "Products");

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt;

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };
            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = _CONTEXT.Products.Find(id);
            if (product == null) return RedirectToAction("Index", "Products");

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt;

                return View(productDto);
            }

            string newFile = product.ImageFileName;
            if (productDto.ImageFile != null)
            {

                newFile += Path.GetExtension(productDto.ImageFile!.FileName);
                //! = usniqueFilename.jpg or .png...
                //Then find Image full Path
                string imageFullPath = _env.WebRootPath + "/images/" + newFile;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }
                //Delete old file (update image)
                string oldFullPath = _env.WebRootPath + "/images/" + product.ImageFileName;
                System.IO.File.Delete(oldFullPath);
            }
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFile;
            
            _CONTEXT.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Delete(int id)
        {
            var product = _CONTEXT.Products.Find(id);   
            if (product == null) return RedirectToAction("Index", "Products");
            string imageFullPath = _env.WebRootPath + "/images/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            _CONTEXT.Products.Remove(product);
            _CONTEXT.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}