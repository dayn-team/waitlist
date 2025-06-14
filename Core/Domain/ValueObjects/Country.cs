namespace Core.Domain.ValueObjects {
    public class Country {
        private string _extension;
        public string Name { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; } = "NGN";
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string PhoneExtension {
            get {
                if (!string.IsNullOrEmpty(_extension)) {
                    if (!_extension.StartsWith("+"))
                        return $"+{_extension}";
                }
                return _extension;
            }
            set {
                _extension = value;
            }
        }
    }
}
