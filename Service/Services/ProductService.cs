using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Product;

namespace Service.Services
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, ICategoryService categoryService, ITagService tagService, IColorService colorService, IBrandService brandService, IMapper mapper)
        {
            _repository = repository;
            _categoryService = categoryService;
            _tagService = tagService;
            _colorService = colorService;
            _brandService = brandService;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(ProductCreateVM model)
        {
            var songId = await _repository.CreateAsync(_mapper.Map<Product>(model));
            return songId;
        }

        public async Task<bool> DeleteAsync(int id, string ImagePath)
        {
            var product = await _repository.GetSingleAsync(x => x.Id == id, "ProductImages");

            if (product is null)
                return false;

            foreach (var image in product.ProductImages)
                image.Path.DeleteImage(ImagePath);

            _repository.Delete(product);
            await _repository.SaveAsync();

            return true;

        }

        public async Task<List<Product>> GetAllAsync(int? categoryId = null)
        {
            var products = await _repository.GetAll("ProductImages", "Category").ToListAsync();

            if (categoryId is not null)
                products = products.Where(x => x.CategoryId == categoryId).ToList();

            return products;
        }

        public async Task<List<Product>> GetBestProducts()
        {
            var products = await _repository.GetAll("ProductImages").ToListAsync();

            products = products.OrderByDescending(x => x.Quantity).Take(2).ToList();

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _getProductById(id);
        }

        public async Task<List<Product>> GetNewProducts()
        {
            var products = await _repository.GetAll("ProductImages").OrderByDescending(x => x.Id).Take(12).ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetBestSellerProducts()
        {
            var products = await _repository.GetAll("ProductImages").OrderBy(x => x.Quantity).Take(12).ToListAsync();

            return products;
        }


        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            var products = await _repository.GetFiltered(x => x.CategoryId == categoryId, "ProductImages").ToListAsync();

            return products;
        }

        public async Task<ProductUpdateVM?> GetUpdatedProductAsync(int id, dynamic ViewBag)
        {
            var product = await _getProductById(id);

            if (product is null)
                return null;

            ProductUpdateVM VM = new()
            {
                Id = id,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Rating = product.Rating,
                TagIds = product.ProductTags.Select(x => x.TagId).ToList(),
                ColorIds = product.ProductColors.Select(x => x.ColorId).ToList(),
                ImagePaths = product.ProductImages.Where(x => !x.IsMain && !x.IsHover).Select(x => x.Path).ToList(),
                ImageIds = product.ProductImages.Where(x => !x.IsMain && !x.IsHover).Select(x => x.Id).ToList(),
                HoverImagePath = product.ProductImages.FirstOrDefault(x => x.IsHover)?.Path ?? "null",
                MainImagePath = product.ProductImages.FirstOrDefault(x => x.IsMain)?.Path ?? "null",
            };

            await SendViewBagElements(ViewBag);

            return VM;
        }


        public async Task SendViewBagElements(dynamic ViewBag)
        {
            var categories = (await _categoryService.GetAllAsync()).ToList();
            var brands = await _brandService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Tags = tags;
        }

        public async Task<bool?> UpdateAsync(ProductUpdateVM VM, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState, dynamic ViewBag, string imagePath)
        {
            await SendViewBagElements(ViewBag);

            if (!ModelState.IsValid)
                return false;

            var existProduct = await _getProductById(VM.Id);

            if (existProduct is null)
                return null;



            #region Product Validations


            var isExistCategory = await _categoryService.IsExistAsync(x => x.Id == VM.CategoryId);

            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryId", "Category is not found");
                return false;
            }

            var isExistBrand = await _brandService.IsExistAsync(x => x.Id == VM.BrandId);

            if (!isExistBrand)
            {
                ModelState.AddModelError("BrandId", "Brand is not found");
                return false;
            }

            foreach (var tagId in VM.TagIds)
            {
                var isExistTag = await _tagService.IsExistAsync(x => x.Id == tagId);

                if (!isExistTag)
                {
                    ModelState.AddModelError("TagIds", "Tag is not found");
                    return false;
                }

            }

            foreach (var colorId in VM.ColorIds)
            {
                var isExistColor = await _colorService.IsExistAsync(x => x.Id == colorId);

                if (!isExistColor)
                {
                    ModelState.AddModelError("ColorIds", "Color is not found");
                    return false;
                }

            }



            if (VM.MainImage is not null && !VM.MainImage.CheckImage())
            {
                ModelState.AddModelError("MainImage", "Please enter valid input");
                return false;
            }

            if (VM.HoverImage is not null && !VM.HoverImage.CheckImage())
            {
                ModelState.AddModelError("HoverImage", "Please enter valid input");
                return false;
            }

            foreach (var image in VM.Images)
            {
                if (!image.CheckImage())
                {
                    ModelState.AddModelError("Images", "Please enter valid input");
                    return false;
                }
            }



            #endregion



            existProduct.Name = VM.Name;
            existProduct.Price = VM.Price;
            existProduct.CategoryId = VM.CategoryId;
            existProduct.BrandId = VM.BrandId;
            existProduct.Weight = VM.Weight;
            existProduct.SKU = VM.SKU;
            existProduct.Quantity = VM.Quantity;
            existProduct.Price = VM.Price;
            existProduct.Rating = VM.Rating;
            existProduct.Description = VM.Description;

            existProduct.ProductTags = new List<ProductTag>();
            existProduct.ProductColors = new List<ProductColor>();

            foreach (var tagId in VM.TagIds)
            {
                existProduct.ProductTags.Add(new()
                {
                    TagId = tagId,
                    Product = existProduct
                });
            }

            foreach (var colorId in VM.ColorIds)
            {
                existProduct.ProductColors.Add(new()
                {
                    ColorId = colorId,
                    Product = existProduct
                });
            }


            #region CreateNewImages



            if (VM.MainImage is not null)
            {
                existProduct.ProductImages.FirstOrDefault(x => x.IsMain)?.Path.DeleteImage(imagePath);
                string filename = await VM.MainImage.CreateImageAsync(imagePath);
                if (existProduct.ProductImages.FirstOrDefault(x => x.IsMain) is not null)
                    existProduct.ProductImages.FirstOrDefault(x => x.IsMain).Path = filename;
                else
                {
                    ProductImage image = new()
                    {
                        IsMain = true,
                        Path = filename,
                        Product = existProduct
                    };
                    existProduct.ProductImages.Add(image);
                }
            }


            if (VM.HoverImage is not null)
            {
                existProduct.ProductImages.FirstOrDefault(x => x.IsHover)?.Path.DeleteImage(imagePath);
                string filename = await VM.HoverImage.CreateImageAsync(imagePath);
                if (existProduct.ProductImages.FirstOrDefault(x => x.IsHover) is not null)
                    existProduct.ProductImages.FirstOrDefault(x => x.IsHover).Path = filename;

                else
                {
                    ProductImage image = new()
                    {
                        IsHover = true,
                        Path = filename,
                        Product = existProduct
                    };
                    existProduct.ProductImages.Add(image);
                }
            }




            var ExistedImages = existProduct.ProductImages.Where(x => !x.IsMain && !x.IsHover).Select(x => x.Id).ToList();
            if (VM.ImageIds is not null)
            {
                ExistedImages = ExistedImages.Except(VM.ImageIds).ToList();

            }
            if (ExistedImages.Count > 0)
            {
                foreach (var imageId in ExistedImages)
                {
                    var deletedImage = existProduct.ProductImages.FirstOrDefault(x => x.Id == imageId);
                    if (deletedImage is not null)
                    {

                        existProduct.ProductImages.Remove(deletedImage);
                        deletedImage.Path.DeleteImage(imagePath);

                    }

                }
            }


            foreach (var image in VM.Images)
            {
                existProduct.ProductImages.Add(new() { Path = await image.CreateImageAsync(imagePath), Product = existProduct });
            }

            #endregion

            _repository.Update(existProduct);
            await _repository.SaveAsync();

            return true;
        }

        private async Task<Product?> _getProductById(int id)
        {
            return await _repository.GetSingleAsync(x => x.Id == id, "ProductImages", "Category", "Brand", "ProductTags.Tag");
        }

        Task<ProductUpdateVM?> IProductService.GetUpdatedProductAsync(int id, dynamic ViewBag)
        {
            throw new NotImplementedException();
        }
    }
}
