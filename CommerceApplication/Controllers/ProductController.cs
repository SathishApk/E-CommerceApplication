using AutoMapper;
using CommerceApplication.Data;
using CommerceApplication.Models;
using CommerceApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IProductService _productService;
        private IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public ProductController(UserManager<IdentityUser> userManager, ILogger<ProductController> logger, IProductService productService, IMapper mapper)
        {
            _userManager = userManager;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: ProductController
        [Authorize(Policy = "readonlypolicy")]
        public async Task<ActionResult> Index()
        {
            var userId = _userManager.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name))?.Id;
            var products = await _productService.GetAllProducts(User.Identity.Name);
            if (products!= null)
            {
                products = _mapper.Map<List<Product>>(products);
                return View(products);
            }
            else
            {
                return View(new List<Product>());
            }
        }

        [Authorize(Policy = "readonlypolicy")]
        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.Product_Get(id);
            var userEmail = _userManager.Users.FirstOrDefault(x => x.Id.Equals(product.Owner))?.Email;
            if (product != null)
            {
                product = _mapper.Map<Product>(product);
                ViewBag.ImageData = product.ProductImage;
                ViewBag.Owner = userEmail;
                return View(product);
            }
            else
            {
                return View(new List<Product>());
            }
        }

        // GET: ProductController/Create
        [Authorize(Policy = "writepolicy")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Owner = User.Identity.Name;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create_Post()
        {
            try
            {
                if (Request.Form.Files != null && Request.Form.Files.Count() > 0)
                {
                    Product product = new Product();
                    await TryUpdateModelAsync(product);
                    var IsSuccess = await _productService.CreateProduct(product, Request.Form.Files.FirstOrDefault(), User.Identity.Name);
                    if (IsSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Owner = User.Identity.Name;
                        ViewBag.UploadError = "Image format not correct or User not found !!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Owner = User.Identity.Name;
                    ViewBag.UploadError = "Upload Image !!!";
                    return View();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                ViewBag.Owner = User.Identity.Name;
                ViewBag.UploadError = "";
                return View();
            }
        }

        // GET: ProductController/Edit/5
        [Authorize(Policy = "writepolicy")]
        public async Task<ActionResult> Edit(int id)
        {
            string userId = _userManager.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name)).Id;
            var product = await _productService.EditProduct_Get(id, userId);
            if (product != null)
            {
                product = _mapper.Map<Product>(product);
                ViewBag.Image = product.ProductImage;
                ViewBag.Owner = User.Identity.Name;
                return View(product);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(include: "ProductId,ProductName,Price,ProductDescription,Category")] Product product)
        {
            try
            {
                string userId = _userManager.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name)).Id;
                var IsSuccess = await _productService.EditProduct(product, Request.Form.Files.FirstOrDefault(), userId);
                if(IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    product = _mapper.Map<Product>(product);
                    ViewBag.Image = product.ProductImage;
                    ViewBag.Owner = User.Identity.Name;
                    ViewBag.UploadError = "Unable to Edit the record The reason may be due to the absense of record in Db or User not found";
                    return View();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                product = _mapper.Map<Product>(product);
                ViewBag.Image = product.ProductImage;
                ViewBag.Owner = User.Identity.Name;
                ViewBag.UploadError = "Unable to Edit the record The reason may be due to the absense of record in Db or User not found";
                return View();
            }
        }

        // GET: ProductController/Delete/5
        [Authorize(Policy = "writepolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            string userId = _userManager.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name)).Id;
            var product = await _productService.DeleteProduct_Get(id, userId);
            if(product != null)
            {
                ViewBag.ProductName = product.ProductName;
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_Post(int id)
        {
            try
            {
                string userId = _userManager.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name)).Id;
                var IsSuccess = await _productService.DeleteProduct(id, userId);
                if(IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ProductName = Request.Form["ProductName"];
                    return View();
                }

            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                ViewBag.ProductName = Request.Form["ProductName"];
                return View();
            }
        }
    }
}
