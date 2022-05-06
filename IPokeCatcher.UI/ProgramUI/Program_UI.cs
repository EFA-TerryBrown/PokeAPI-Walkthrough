using static System.Console;

public class Program_UI
{
    private readonly PokemonRepository _pRepo = new PokemonRepository();

    public void Run()
    {
        SeedData().Wait();
        RunApplication();
    }


    private void RunApplication()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Clear();
            WriteLine("Welcome to the IPokeCatcher App!\n" +
                      "Please Make a Selection:\n" +
                      "1. Insert Pokemon Into the Database\n" +
                      "2. View all Captured Pokemon\n" +
                      "3. View Captured Pokemon by PokeID\n" +
                      "4. Update Captured Pokemon\n" +
                      "5. Released Captured Pokemon\n" +
                      "0. Exit Application\n");

            var userInput = int.Parse(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    InsertPokemonIntoDatabase();
                    break;
                case 2:
                    ViewAllCapturedPokemon();
                    break;
                case 3:
                    ViewCapturedPokemonbyPokeID();
                    break;
                case 4:
                    UpdateCapturedPokemonData();
                    break;
                case 5:
                    ReleasedCapturedPokemon();
                    break;
                case 0:
                    isRunning = CloseApplication();
                    break;
                default:
                    WriteLine("Invalid Selection, Press any key to continue.");
                    ReadKey();
                    break;
            }
        }
    }

    private bool CloseApplication()
    {
        WriteLine("Thanks for using IPokeCatcher! Press any key to continue.");
        ReadKey();
        return false;
    }

    private void ReleasedCapturedPokemon()
    {
        Clear();
        var pokeInDB = _pRepo.GetPokemonFromDB();
        foreach (var pokemon in pokeInDB)
        {
            DisplayPokemonNameandID(pokemon);
        }

        try
        {
            WriteLine("Plese select a pokemon by its PokeID for Release:");
            var pokeID = int.Parse(ReadLine());

            Clear();
            var pokeToBeReleased = _pRepo.GetCapturedPokemon(pokeID);
            DisplayPokemonInUI(pokeToBeReleased);
            WriteLine("Are you sure you want to release this Pokemon? y/n");

            var userInput = ReadLine();
            if (userInput == "Y".ToLower())
            {
                var success = _pRepo.ReleasePokemon(pokeToBeReleased.PokeID);
                if (success)
                    WriteLine($"{pokeToBeReleased.Name} Found A New Home!");
                else
                    WriteLine($"{pokeToBeReleased.Name} Failed to be Released!");
            }
        }
        catch (Exception ex)
        {
            WriteLine($"Somthing went wrong, {ex}.");
            WriteLine("Press any key to continue.");
        }

        ReadKey();
    }

    private void UpdateCapturedPokemonData()
    {
        Clear();
        Pokemon pokemon = new Pokemon();

        try
        {
            WriteLine("Please enter The Pokemon Catchers name:");
            pokemon.PokemonCatcherName = Console.ReadLine();

            var pokemonListFromDb = _pRepo.GetAllPokemonsByPokemonCatcher(pokemon.PokemonCatcherName);

            Clear();
            foreach (var p in pokemonListFromDb)
            {
                DisplayPokemonNameandID(p);
            }

            WriteLine("Please enter a valid pokemon Id For An Update:");
            var pokeID = int.Parse(Console.ReadLine());

            var pokemonToBeUpdated = _pRepo.GetCapturedPokemon(pokeID);

            WriteLine("Please enter a valid name of the Pokemon");
            pokemon.Name = Console.ReadLine();

            var successful = _pRepo.UpdatePokemonAsync(pokemonToBeUpdated.PokeID, pokemon).Result;
            if (successful)
            {
                WriteLine($"{pokemon.PokemonCatcherName} Is Updated {pokemon.Name} to the Database!");
            }
            else
            {
                WriteLine($"{pokemon.Name} Is Not Updated to the Database!");
            }
        }
        catch (Exception ex)
        {
            WriteLine($"Something went wrong: {ex}");
        }

        ReadKey();

    }

    private void ViewCapturedPokemonbyPokeID()
    {
        Clear();

        try
        {
            WriteLine("Please enter a valid PokeID:");
            var pokeID = int.Parse(ReadLine());
            var pokemon = _pRepo.GetCapturedPokemon(pokeID);
            if (pokemon is null)
                WriteLine($"Sorry, the pokemon with the id: {pokeID} cannot be found.");
            else
            {
                Clear();
                DisplayPokemonInUI(pokemon);
            }

        }
        catch (Exception ex)
        {
            WriteLine($"Somthing went wrong, {ex}.");
            WriteLine("Press any Key to Continue.");
            ReadKey();
        }

        ReadKey();
    }

    private void ViewAllCapturedPokemon()
    {
        Clear();
        var capturedPokemonInDB = _pRepo.GetPokemonFromDB();
        foreach (var pokemon in capturedPokemonInDB)
        {
            DisplayPokemonNameandID(pokemon);
        }

        ReadKey();
    }

    //helper methods....
    private void DisplayPokemonInUI(Pokemon pokemon)
    {
        System.Console.WriteLine($"PokeCatcherName: {pokemon.PokemonCatcherName}\n" +
                                 $"Pokemon Id: {pokemon.Id}\n" +
                                 $"PokeID: {pokemon.PokeID}\n" +
                                 $"Pokemon Name: {pokemon.Name}\n" +
                                 $"Pokemon BaseExperience: {pokemon.BaseExperience}\n" +
                                 $"Pokemon Height: {pokemon.Height}\n" +
                                 $"Pokemon IsDefault: {pokemon.IsDefault}\n" +
                                 $"Pokemon Order: {pokemon.Order}\n" +
                                 $"Pokemon Weight: {pokemon.Weight}\n" +
                                 $"Pokemon Capture Date: {pokemon.CaptureDate}\n" +
                                 $"-------------------  List of Forms ----------------------\n");

        foreach (var form in pokemon.Forms)
        {
            WriteLine($"FormName:{form.Name}");
        }
        WriteLine("-------------------  List of Abilities ----------------------\n");
        foreach (var ability in pokemon.Abilities)
        {
            WriteLine($"IsHidden: {ability.IsHidden}\n" +
                      $"Slot: {ability.Slot}\n" +
                      $"Ability: {ability.Ability.Name}\n");
        }
        WriteLine("============================================================\n");
    }

    private void DisplayPokemonNameandID(Pokemon pokemon)
    {
        Write($"PokeID: {pokemon.PokeID} -- PokemonName: {pokemon.Name}\n");
    }
    private void InsertPokemonIntoDatabase()
    {
        Clear();

        Pokemon pokemon = new Pokemon();
        WriteLine("Please enter the PokeCatchers Name:");
        pokemon.PokemonCatcherName = ReadLine();

        WriteLine("Please enter a valid Pokemon Name:");
        pokemon.Name = ReadLine();

        var success = _pRepo.AddPokemonToDatabase(pokemon.Name, pokemon.PokemonCatcherName).Result;
        if (success)
            WriteLine($"{pokemon.PokemonCatcherName} added {pokemon.Name} to the Database.");
        else
            WriteLine($"{pokemon.Name} was not added to the Database!");

        ReadKey();
    }

    private async Task SeedData()
    {
        Pokemon pokemonA = new Pokemon("pikachu", "Terry Brown");
        Pokemon pokemonB = new Pokemon("charizard", "Phil Smith");
        Pokemon pokemonC = new Pokemon("mewtwo", "Andrew Torr");

        await _pRepo.AddPokemonToDatabase(pokemonA.Name, pokemonA.PokemonCatcherName);
        await _pRepo.AddPokemonToDatabase(pokemonB.Name, pokemonB.PokemonCatcherName);
        await _pRepo.AddPokemonToDatabase(pokemonC.Name, pokemonC.PokemonCatcherName);
    }
}
