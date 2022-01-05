using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerStore.ViewModel
{
    public class LoginViewModel
    {
        [Required, EmailAddress,DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool Remember { get; set; }

    }
}
