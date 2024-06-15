using fruit_backend_project.Areas.ViewModels.Products;
using fruit_backend_project.Data;
using fruit_backend_project.Helpers.Extentions;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;
        public ProductController(AppDbContext context, IProductService service, IWebHostEnvironment webHostEnvironment,
                                  ICategoryService categoryService)
        {
            _context = context;
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var dbProduct = await _service.GetAllPaginateAsync(page);

            List<ProductVM> mappedDatas = _service.GetMappedDatas(dbProduct);

            int pageCount = await GetPageCountAsync(4);

            Paginate<ProductVM> model = new(mappedDatas, pageCount, page);

            return View(model);


        }
        private async Task<int> GetPageCountAsync(int take)
        {
            int count = await _service.GetCountAsync();
            return (int)Math.Ceiling((decimal)count / take);
        }

        [HttpGet]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _service.GetByIdAsync((int)id);
            if (product == null) return NotFound();

            List<ProductImageVM> productImages = new();
            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });

            }

            ProductDetailVM model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category.Name,
                Images = productImages,
                Quality = product.Quality,
                Check = product.Сheck,
                MinWeight = product.MinWeight,
                Weight = product.Weight

            };
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();
            //if (!ModelState.IsValid) return View();

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 kb");
                    return View();
                }

                if (!item.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Images", "Image format must be img");
                    return View();
                }
            }
            List<ProductImage> images = new();

            foreach (var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                await item.SaveFileToLocalAsync(path);


                images.Add(new ProductImage
                {
                    Image = fileName
                });
            }

            images.FirstOrDefault().IsMain = true;

            Product product = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = decimal.Parse(request.Price),
                CategoryId = request.CategoryId,
                ProductImages = images,
                Weight = request.Weight,
                MinWeight = request.MinWeight,
                Origin = request.Origin,
                Сheck = request.Check,
                Quality = request.Quality

            };

            await _service.CreateAsync(product);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _categoryService.GetALlBySelectedAsync();

            if (id == null) return BadRequest();
            Product product = await _service.GetByIdAsync((int)id);
            if (product == null) return NotFound();


            List<ProductImageVM> productImage = new();

            foreach (var item in product.ProductImages)
            {
                productImage.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });
            }


            return View(new ProductEditVM { Name = product.Name, Description = product.Description, Images = productImage, Weight = product.Weight, Price = product.Price, Origin = product.Origin, Check = product.Сheck, Quality = product.Quality });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditVM productEditVM, int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return BadRequest();
            Product existProduct = await _service.GetByIdAsync((int)id);
            if (existProduct == null) return NotFound();

            if (productEditVM.Photos != null)
            {
                foreach (var item in productEditVM.Photos)
                {
                    if (!item.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200 kb");
                        return View(productEditVM);
                    }
                    if (!item.CheckFileFormat("image/"))
                    {
                        ModelState.AddModelError("Photo", "Image format must be img");
                        return View(productEditVM);
                    }
                }

                foreach (var item in existProduct.ProductImages)
                {
                    _context.ProductImages.Remove(item);

                    FileExtention.DeleteFileFromLocalAsync(Path.Combine(_webHostEnvironment.WebRootPath, "img"), item.Image);
                }

                List<ProductImage> images = new();

                foreach (var item in productEditVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
                    await item.SaveFileToLocalAsync(path);

                    images.Add(new ProductImage
                    {
                        Image = fileName
                    });
                }

                images.FirstOrDefault().IsMain = true;

                existProduct.ProductImages = images;
            }

            existProduct.Name = productEditVM.Name;
            existProduct.Description = productEditVM.Description;
            existProduct.Price = productEditVM.Price;
            existProduct.Weight = productEditVM.Weight;
            existProduct.MinWeight = productEditVM.MinWeight;
            existProduct.Origin = productEditVM.Origin;
            existProduct.Сheck = productEditVM.Check;
            existProduct.CategoryId = (int)productEditVM.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Where(m => m.Id == id).Include(m => m.ProductImages).FirstOrDefaultAsync();
            if (product == null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", item.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }




            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

    }
}
