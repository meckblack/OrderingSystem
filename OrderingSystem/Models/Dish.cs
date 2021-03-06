﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystem.Models
{
    public class Dish
    {
        #region Model Data

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public double Amount { get; set; }

        #endregion

        #region Forigen Keys

        public int MealId { get; set; }
        public virtual Meal Meal { get; set; }

        #endregion
    }
}
