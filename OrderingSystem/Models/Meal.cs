using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystem.Models
{
    public class Meal
    {
        #region Model Data

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        #endregion

        #region ICollection

        public virtual ICollection<Dish> Dishes { get; set; }

        #endregion
    }
}
