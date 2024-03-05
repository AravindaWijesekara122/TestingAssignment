using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class LoginOutputDTO
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
