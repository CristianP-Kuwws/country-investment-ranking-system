using Application.Dtos.MacroIndicator;
using Application.ViewModels.MacroIndicator;

namespace HorizonFutureVestApp.Helpers
{
    public static class MacroIndicatorMapper
    {
        public static List<MacroIndicatorViewModel> ToViewModel(this List<MacroIndicatorDto> dtos)
        {
            if (dtos == null) return new List<MacroIndicatorViewModel>();

            return dtos.Select(m => new MacroIndicatorViewModel
            {
                IdMacroIndicator = m.IdMacroIndicator,
                Name = m.Name,
                Weight = m.Weight,
                IsHighBetter = m.IsHighBetter,
                IndicatorsQuantity = m.IndicatorsQuantity
            }).ToList();
        }
    }
}
