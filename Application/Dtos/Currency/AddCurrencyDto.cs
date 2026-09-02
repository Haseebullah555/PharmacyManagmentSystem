using Application.Dtos.Common;

namespace Application.Dtos.Currency
{
    public class AddCurrencyDto : CreateBaseDto
    {
        public string CurrencyName { get; set; }
    }
}
