using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingSystem.Models
{
    public class Role
    {
        #region Model Data

        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        public string Name { get; set; }

        public bool CanManageApplication { get; set; }

        #endregion
    }
}
