using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCar.Application.Settings
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = null;
        public string AccessKey { get; set; } = null;
        public string SecretKey { get; set; } = null;
        public bool UseSSL { get; set; }
    }
}
