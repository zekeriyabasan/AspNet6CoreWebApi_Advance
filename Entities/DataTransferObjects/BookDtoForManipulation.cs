using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract record BookDtoForManipulation
    {
        [Required(ErrorMessage ="Name field is required!")]
        [MinLength(2,ErrorMessage ="Name field must consist of at least 2 characters!")]
        [MaxLength(50, ErrorMessage = "Name field must consist of at maximum 50 characters!")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Price field is required!")]
        [Range(1,1000)]
        public decimal Price { get; init; }
    }
}
