﻿namespace CodePulse.API.Model.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //One to Many relationship - Each category can have multiple Contents.
        public ICollection<Content> Contents { get; set; }
    }
}
