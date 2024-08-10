using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ProductColorVms;

namespace Service.Services
{
    public class ProductColorService : IProductColorService
    {
        private readonly IProductColorRepository _repository;
        public readonly IMapper _mapper;

        public ProductColorService(IProductColorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductColorCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ProductColor>(model));
        }
    }
}
