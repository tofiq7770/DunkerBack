﻿namespace DunkerFinal.ViewModels.Blog
{
    public class BlogUpdateVM
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}