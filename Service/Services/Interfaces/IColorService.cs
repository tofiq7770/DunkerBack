﻿using Domain.Entities;
using Service.ViewModels.Color;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface IColorService
    {
        public Task<bool> IsExistAsync(Expression<Func<Color, bool>> expression);

        Task<IEnumerable<ColorListVM>> GetAllAsync();
        Task<bool> AnyAsync(string name);
        Task<ColorUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(ColorCreateVM model);
        Task UpdateAsync(int id, ColorUpdateVM model);
        Task DeleteAsync(int id);
    }
}
