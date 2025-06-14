namespace Core.Application.UseCases.Other {
    using Core.Domain.DTOs.Others;
    using NetCore.AutoRegisterDi;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    [RegisterAsSingleton]
    public sealed class CountryDataLoader {
        private static readonly Lazy<CountryDataLoader> _instance =
            new Lazy<CountryDataLoader>(() => new CountryDataLoader());

        public static CountryDataLoader Instance => _instance.Value;

        public IReadOnlyList<CountryInformation> Countries { get; private set; }
        private CountryDataLoader() {
            LoadDataFromJson();
        }

        private void LoadDataFromJson() {
            try {
                string filePath = "countries.json";
                string jsonString = File.ReadAllText(filePath);
                Countries = JsonSerializer.Deserialize<List<CountryInformation>>(jsonString)
                             ?? new List<CountryInformation>();
            } catch (Exception ex) {
                Console.WriteLine($"Error loading JSON data: {ex.Message}");
                Countries = new List<CountryInformation>();
            }
        }

        public void ReloadData() {
            LoadDataFromJson();
        }
    }
}
