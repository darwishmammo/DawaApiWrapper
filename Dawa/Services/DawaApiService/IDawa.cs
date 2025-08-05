namespace Dawa.Services.DawaApiService;

public interface IDawa
{
    public IAutocompleteEndpoint Autocomplete { get; }
    public IAdresseEndpoint Adresser { get; }
    public IAdgangsadresseEndpoint Adgangsadresser { get; }
    public IPostnummerEndpoint Postnumre { get; }
    public IJordstykkeEndpoint Jordstykker { get; }
}