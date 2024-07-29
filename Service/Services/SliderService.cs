using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Slider;

namespace Service.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repository;
        public readonly IMapper _mapper;

        public SliderService(ISliderRepository SliderRepository, IMapper mapper)
        {
            _repository = SliderRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<SliderListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SliderListVM>>(await _repository.GetAllAsync());
        }

        public async Task<SliderUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SliderUpdateVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(SliderCreateVM model)
        {
            var mapSlider = _mapper.Map<Slider>(model);
            await _repository.CreateAsync(mapSlider);
        }

        public async Task UpdateAsync(int id, SliderUpdateVM model)
        {
            var dbSlider = await _repository.GetByIdAsync(id);

            var mapSlider = _mapper.Map(model, dbSlider);

            await _repository.UpdateAsync(mapSlider);
        }

        public async Task<bool> AnyAsync(string key)
        {
            return await _repository.AnyAsync(key);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }
    }
}