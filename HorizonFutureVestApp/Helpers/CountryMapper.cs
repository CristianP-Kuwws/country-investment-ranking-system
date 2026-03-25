using Application.Dtos.Country;
using Application.ViewModels.Country;

namespace HorizonFutureVestApp.Helpers
{
    public static class CountryMapper
    {
        public static List<CountryViewModel> ToViewModel(this List<CountryDto> dtos)
        {
            if (dtos == null) return new List<CountryViewModel>();

            return dtos.Select(c => new CountryViewModel
            {
                IdCountry = c.IdCountry,
                Name = c.Name,
                ISOCode = c.ISOCode
            }).ToList();
        }
    }
}
