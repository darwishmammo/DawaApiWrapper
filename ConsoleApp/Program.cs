using Dawa;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.Json;

var serviceCollection = new ServiceCollection();

serviceCollection.AddDawa();

using var serviceProvider = serviceCollection.BuildServiceProvider();

var client = serviceProvider.GetRequiredService<IDawa>();

var result = await client
    .Navngivneveje
    .Opslag(new("46a834bd-559e-4e69-919d-a534e0a97fcf") { })
    .AsGeoJsonAsync();

var result1 = await client
    .Adgangsadresser
    .Søge(new() { Query = "rostrupsvej 10", PostNr = 9000 })
    .AsGeoJsonAsync();

var result2 = await client
    .Autocomplete
    .Søge(new() { Query = "rostrupsvej" })
    .AsJsonAsync();

var result3 = await client
    .Navngivneveje
    .Naboer(new("65eb3979-821b-41fd-a8ef-da0de69edbc0") { Afstand = 5.555555555, SRID = SRID.UTMZone32 })
    .AsGeoJsonAsync();

var result4 = await client
    .Vejstykker
    .Søge(new() { Side = 1, Per_side = 10 })
    .AsGeoJsonAsync();

var result5 = await client
    .Jordstykker
    .Søge(new() { Matrikelnr = "481b", SRID = SRID.UTMZone32 })
    .AsJsonAsync();

var result6 = await client
    .Postnumre
    .Søge(new() { Kommunekode = 851 })
    .AsJsonAsync();

var result7 = await client
    .Vejstykker
    .Søge(new() { Polygon = [[10.3, 55.3], [10.4, 55.3], [10.4, 55.31], [10.4, 55.31], [10.3, 55.3]], SRID = SRID.WGS84 })
    .AsJsonAsync();

var result8 = await client
    .Vejstykker
    .Autocomplete(new() { Query = "jolle" })
    .AsJsonAsync();

var result9 = await client
    .Vejstykker
    .Autocomplete(new() { Id = new Guid("5cbdee08-4eae-11e8-93fd-066cff24d637") })
    .AsJsonAsync();

var result10 = await client
    .Vejstykker
    .ReverseGeocode(new(12.5851471984198, 55.6832383751223) { })
    .AsJsonAsync();

var result11 = await client
    .Vejstykker
    .Naboer(new(101, 64) { })
    .AsJsonAsync();

var result12 = await client
    .Adresser
    .Datavask("danmarksgade")
    .AsJsonAsync();

var result13 = await client
    .Adresser
    .Historik(new() { PerSide = 10 })
    .AsJsonAsync();

Console.WriteLine(JsonSerializer.Serialize(result13, options: new() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
//Console.WriteLine(result?.Length);