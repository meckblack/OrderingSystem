using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystem.Models
{
    public class Cart
    {
        #region Model Data

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Input amount!")]
        public int Amount { get; set; }

        //Entity R/ship
        [Required]
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }

        //[Required]
        //public int OrderGroupId { get; set; }
        //public virtual OrderGroup OrderGroup { get; set; }

        //public int UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }

        #endregion
    }
}
