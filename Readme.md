# DAWA .NET Client

A strongly-typed, fluent, and dependency-injectable **DAWA API client** for .NET, making it easy to query Denmark’s Address Web API from your applications.

This package provides a fully typed interface to all major DAWA endpoints, with built-in support for multiple output formats (JSON, CSV, GeoJSON), query parameter models, and a fluent API for composing requests.

---

## ✨ Features

- **Dependency Injection Ready** – Add it once to your service collection and inject it anywhere.
- **Strongly Typed Endpoints** – All DAWA endpoints exposed via clear, discoverable interfaces.
- **Multiple Output Formats** – Fetch results as JSON, CSV, or GeoJSON with a single method call.
- **Fluent Request Building** – Chain `.Søge(...)`, `.Opslag(...)`, `.Autocomplete(...)` etc., directly to output methods.
- **Built-in Query Parameter Models** – Reduce boilerplate by passing structured query objects.
- **Full Endpoint Coverage** – Adgangsadresser, Adresser, Autocomplete, Jordstykker, Navngivne veje, Postnumre, Vejstykker, and more.
- **SRID Support** – Strongly typed coordinate system handling (e.g., WGS84, UTMZone32).
- **Async-First** – All API calls are asynchronous.

---

## 📦 Installation

```bash
dotnet add package darwish.dawa.sdk
```
## 🚀 Getting Started

1.  **Register the service** in your dependency container:
```
builder.Services.AddDawa();
```
2. **Resolve and use the client** anywhere via constructor injection or manually:
```
using Dawa;

public class IndexModel(IDawa dawa) : PageModel
{
    private readonly IDawa _dawa = dawa;
}
```
## 🔍 Example Usage
```
var result = await _dawa
    .Navngivneveje
    .Opslag(new("46a834bd-559e-4e69-919d-a534e0a97fcf") { })
    .AsGeoJsonAsync();

var result1 = await _dawa 
    .Adgangsadresser
    .Søge(new() { Query = "rostrupsvej 10", PostNr = 9000 })
    .AsGeoJsonAsync();

var result2 = await _dawa 
    .Autocomplete
    .Søge(new() { Query = "rostrupsvej" })
    .AsJsonAsync();

var result3 = await client
    .Navngivneveje
    .Naboer(new("65eb3979-821b-41fd-a8ef-da0de69edbc0") { 
        Afstand = 5.555555555, 
        SRID = SRID.UTMZone32 })
    .AsGeoJsonAsync();

var result4 = await _dawa 
    .Vejstykker
    .Søge(new() { Side = 1, Per_side = 10 })
    .AsGeoJsonAsync();

var result5 = await _dawa 
    .Jordstykker
    .Søge(new() { Matrikelnr = "481b", SRID = SRID.UTMZone32 })
    .AsJsonAsync();

var result6 = await _dawa 
    .Postnumre
    .Søge(new() { Kommunekode = 851 })
    .AsJsonAsync();

var result7 = await _dawa 
    .Vejstykker
    .Søge(new() { 
        Polygon = [[10.3, 55.3], [10.4, 55.3], [10.4, 55.31], [10.4, 55.31], [10.3, 55.3]], 
        SRID = SRID.WGS84 })
    .AsJsonAsync();

var result8 = await _dawa 
    .Vejstykker
    .Autocomplete(new() { Query = "jolle" })
    .AsJsonAsync();

var result9 = await _dawa 
    .Vejstykker
    .Autocomplete(new() { Id = new Guid("5cbdee08-4eae-11e8-93fd-066cff24d637") })
    .AsJsonAsync();

var result10 = await _dawa 
    .Vejstykker
    .ReverseGeocode(new(12.5851471984198, 55.6832383751223) { })
    .AsJsonAsync();

var result11 = await _dawa 
    .Vejstykker
    .Naboer(new(101, 64) { })
    .AsJsonAsync();

var result12 = await _dawa 
    .Adresser
    .Datavask("danmarksgade")
    .AsJsonAsync();

var result13 = await _dawa 
    .Adresser
    .Historik(new() { PerSide = 10 })
    .AsJsonAsync();

var addresses = await dawa.Adgangsadresser
    .Søge(new() { Query = "rostrupsvej 10", PostNr = 9000 })
    .AsGeoJsonAsync();

var streets = await dawa.Vejstykker
    .Autocomplete(new() { Query = "jolle" })
    .AsJsonAsync();

var location = await dawa.Vejstykker
    .ReverseGeocode(new(12.5851, 55.6832))
    .AsJsonAsync();
```