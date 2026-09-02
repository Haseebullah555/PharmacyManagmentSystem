using Application.Dtos.Common;

namespace Application.Dtos.Currency
{
    public class UpdateCurrencyDto : UpdateBaseDto
    {
        public string CurrencyName { get; set; }
    }
}
