using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulator
{
    public class SaveSimulationMacroViewModel
    {
        public int IdMacroIndicator { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un macroindicador")]
        public int SelectedMacroIndicator { get; set; } // Para el dropdown

        [Required(ErrorMessage = "El peso es requerido")]
        [Range(0.01, 1, ErrorMessage = "El peso debe estar entre 0.01 y 1")]
        public decimal Weight { get; set; }

        // mostrar nombre y peso restante
        public string? Name { get; set; }
        public decimal RemainingWeight { get; set; }

        // lista de macroindicadores disponibles
        public List<MacroOptionViewModel> AvailableMacros { get; set; } = new();
    }
}
