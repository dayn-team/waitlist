using Core.Domain.ValueObjects;

namespace Core.Domain.DTOs.Others {
    public class CountryInformation {
        public string name { get; set; }
        public string isoAlpha2 { get; set; }
        public string isoAlpha3 { get; set; }
        public string flag { get; set; }
        public int isoNumeric { get; set; }
        public Currency currency { get; set; }
        public Country ToCountryObject() {
            return new Country {
                Code = this.isoAlpha2,
                Currency = currency.code,
                CurrencyName = this.currency.name,
                CurrencySymbol = this.currency.symbol,
                Name = this.name
            };
        }
    }
    public class Currency {
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }
}
