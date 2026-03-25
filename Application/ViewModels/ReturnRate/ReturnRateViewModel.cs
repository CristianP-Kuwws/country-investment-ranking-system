using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReturnRate
{
    public class ReturnRateViewModel
    {
        public int IdReturnRate { get; set; }
        public required decimal MinReturnRate { get; set; }
        public decimal MaxReturnRate { get; set; }
    }
}
