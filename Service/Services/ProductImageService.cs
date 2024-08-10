using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ProductImageVMs;

namespace Service.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repository;
        private readonly IMapper _mapper;

        public ProductImageService(IProductImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductImageCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ProductImage>(model));
        }
    }
}
