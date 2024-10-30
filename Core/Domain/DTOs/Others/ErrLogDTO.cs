using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.DTOs.Others {
    public class ErrLogDTO {
        public DateTime date { get; set; } = DateTime.Now;
        public string details { get; set; }
        public string request { get; set; }
        public string route { get; set; }
    }
}
