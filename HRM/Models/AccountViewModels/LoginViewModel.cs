using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Mã ID")]
        public string AccountID { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Lưu dữ liệu biểu mẫu ?")]
        public bool RememberMe { get; set; }
    }
}
