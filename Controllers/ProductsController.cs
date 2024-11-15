using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ICTBD_CRUD_Project.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CRUD_Operations_DbContext _CONTEXT;
        public ProductsController()
        {
            _CONTEXT = new CRUD_Operations_DbContext();
        }
        public IActionResult Index()
        {   
            //Select from DB and convert to List 
            var products = _CONTEXT.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}