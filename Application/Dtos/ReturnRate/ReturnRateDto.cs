using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.ReturnRate
{
    public class ReturnRateDto
    {
        public int IdReturnRate { get; set; }
        public required decimal MinReturnRate { get; set; }
        public required decimal MaxReturnRate { get; set; }
    }
}
