using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models.Data;

namespace WebApp.Models.Shop
{
    public class ShopCategoryVM
    {
        public ShopCategoryVM() { }
        public ShopCategoryVM(ShopCategoryDB model)
        {
            Id = model.Id;
            Name = model.Name;
            Sorting = model.Sorting;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(15, ErrorMessage ="Minimum 3, maximum 15", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public int Sorting { get; set; }

    }
}