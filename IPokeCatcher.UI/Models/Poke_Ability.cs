
using Newtonsoft.Json;

public class Poke_Ability
{

    [JsonProperty("is_hidden")]
    public bool IsHidden { get; set; }

    [JsonProperty("slot")]
    public int Slot { get; set; }

    [JsonProperty("ability")]
    public Poke_Ability_Main Ability { get; set; }
}
