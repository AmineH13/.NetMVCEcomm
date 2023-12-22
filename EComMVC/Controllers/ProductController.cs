using EComMVC.Data;
using EComMVC.Models;
using EComMVC.Models.BO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace EComMVC.Controllers
{
    public class ProductController : Controller
    {
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        private readonly DBContextConnection _dbContextConnection;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<ProductController> _logger; 

        public ProductController(DBContextConnection dB, IWebHostEnvironment hostingEnvironment, ILogger<ProductController> logger)
        {
            this._dbContextConnection = dB;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult addProduct(AddProductViewModel request)
        {
            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = DateTime.Now.Ticks + Path.GetExtension(ImageFile.FileName);

                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyToAsync(fileStream);
                    }

                    var prod = new Produit()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Price = request.Price,
                        Image = "/images/" + fileName
                    };

                    this._dbContextConnection.Add(prod);
                    this._dbContextConnection.SaveChanges();

                    _logger.LogInformation($"Produit {request.Name} ajouté avec succès");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de l'ajout du produit : {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filterName)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string userEmail = User.FindFirst(ClaimTypes.Name).Value;

                var panier = _dbContextConnection.paniers.Where(p => p.IdUser == userId).ToList();

                int numberProductInPanier = panier.Count;

                var produits = _dbContextConnection.produits.ToList();

                if (!string.IsNullOrEmpty(filterName))
                {
                    produits = produits.Where(p => p.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                var viewModel = new ProductPanierModel
                {
                    Produits = produits,
                    NumberProductInPanier = numberProductInPanier,
                    IdUser = userId
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("FormAuth", "User");
            }
        }





    }
}



