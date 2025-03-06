using System.ComponentModel.DataAnnotations;

namespace Sangiaodich.Model
{
    public class LoginModel
    {
        [StringLength(100)]
        public string UserName { get; set; }


        [StringLength(100)]
        public string Password { get; set; }

    }
}
