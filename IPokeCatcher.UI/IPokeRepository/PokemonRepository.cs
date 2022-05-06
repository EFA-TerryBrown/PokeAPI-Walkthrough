
public class PokemonRepository
{
    private readonly List<Pokemon> _pokemonDB = new List<Pokemon>();
    private int _count = 0;

    private HttpClient _client = new HttpClient();
    private readonly string baseURL = "https://pokeapi.co/api/v2/pokemon/";

    //C. 
    public async Task<bool> AddPokemonToDatabase(string pokemonName, string pokemonCatcherName)
    {
        if (pokemonName is null || pokemonCatcherName is null)
        {
            return false;
        }
        else
        {
            //! THIS IS OUR API CALL!!!
            var pokemonDataFrom_API = await RetrivePokemonDataAsync(pokemonName.ToLower());
            if (pokemonDataFrom_API is null)
                return false;
            else
            {
                //increased _count by one 
                _count++;
                pokemonDataFrom_API.PokeID = _count;
                pokemonDataFrom_API.PokemonCatcherName = pokemonCatcherName;
                _pokemonDB.Add(pokemonDataFrom_API);
                return true;
            }
        }
    }

    //R.
    public List<Pokemon> GetPokemonFromDB()
    {
        return _pokemonDB;
    }

    //R -> helper method GetByID
    public Pokemon GetCapturedPokemon(int pokeID)
    {
        return _pokemonDB.SingleOrDefault(p => p.PokeID == pokeID);

        // foreach (var pokemon in _pokemonDB)
        // {
        //     if(pokemon.PokeID==pokeID)
        //     {
        //         return pokemon;
        //     }
        // }
        // return null;
    }


    //U 
    public async Task<bool> UpdatePokemonAsync(int pokeID, Pokemon newPokemonData)
    {
        var existingPokemon = GetCapturedPokemon(pokeID);

        if (existingPokemon is not null)
        {
            if (existingPokemon.Name == newPokemonData.Name)
            {
                existingPokemon.PokemonCatcherName = newPokemonData.PokemonCatcherName;
            }
            else
            {
                //since the user changed the name of the pokemon we need to do another PokeAPI call....
                //with our helper method.
                var newPokeData = await RetrivePokemonDataAsync(newPokemonData.Name);

                //Reassigned the values 'to be safe' 
                //I wanted to 'keep the existingPokemon's PokeID
                existingPokemon.BaseExperience = newPokeData.BaseExperience;
                existingPokemon.Forms = newPokeData.Forms;
                existingPokemon.Height = newPokeData.Height;
                existingPokemon.Id = newPokeData.Height;
                existingPokemon.Name = newPokeData.Name;
                existingPokemon.Order = newPokeData.Order;
                existingPokemon.IsDefault = newPokeData.IsDefault;
                existingPokemon.Weight = newPokeData.Weight;
                existingPokemon.Abilities = newPokeData.Abilities;
                existingPokemon.PokemonCatcherName = newPokemonData.PokemonCatcherName;

                return true;
            }
        }
        return false;
    }

    //D
    public bool ReleasePokemon(int pokeID)
    {
        var pokemon = _pokemonDB.FirstOrDefault(p => p.PokeID == pokeID);
        return _pokemonDB.Remove(pokemon);
    }

    //Get all pokemon based on the name of the catcher
    public List<Pokemon> GetAllPokemonsByPokemonCatcher(string pokemonCatcherName)
    {
        return _pokemonDB.Where(p => p.PokemonCatcherName.ToLower() == pokemonCatcherName.ToLower()).ToList();

        // List<Pokemon> allPokemontCaught=new List<Pokemon>();
        // foreach (var pokemon in _pokemonDB)
        // {
        //     if(pokemon.PokemonCatcherName.ToLower() == pokemonCatcherName.ToLower())
        //     {
        //         allPokemontCaught.Add(pokemon);
        //     }
        // }
        // return allPokemontCaught;
    }


    //todo THIS IS THE HELPER METHOD!
    private async Task<Pokemon> RetrivePokemonDataAsync(string pokemonName)
    {
        var _response = await _client.GetAsync($"{baseURL}{pokemonName}");
        if (_response.IsSuccessStatusCode)
        {
            var pokemon = await _response.Content.ReadAsAsync<Pokemon>();
            return pokemon;
        }
        return null;
    }




}
