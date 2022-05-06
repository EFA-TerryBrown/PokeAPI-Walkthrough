
using Newtonsoft.Json;

public class Pokemon
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("base_experience")]
    public int BaseExperience { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("is_default")]
    public bool IsDefault { get; set; }

    [JsonProperty("order")]
    public int Order { get; set; }

    [JsonProperty("weight")]
    public int Weight { get; set; }

    [JsonProperty("abilities")]
    public List<Poke_Ability> Abilities { get; set; }

    [JsonProperty("forms")]
    public List<Form> Forms { get; set; }

    //adding our stuff
    public int PokeID { get; set; }
    public DateTime CaptureDate { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string PokemonCatcherName { get; set; }

    public Pokemon()
    {
        CaptureDate = DateTime.Now;
    }
    public Pokemon(string pokemonCatcherName)
    {
        PokemonCatcherName = pokemonCatcherName;
        CaptureDate = DateTime.Now;
    }
    public Pokemon(string pokemonName, string pokemonCatcherName)
    {
        Name = pokemonName;
        PokemonCatcherName = pokemonCatcherName;
        CaptureDate = DateTime.Now;
    }
}
