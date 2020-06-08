[System.Serializable]
public class Move
{

    public string name;
    public int pp;
    public int maxpp;
    public string type;
    public Move(string name)
    {
        this.name = name;

        maxpp = PokemonData.GetMove(name).maxpp;
        pp = maxpp;
        type = PokemonData.GetMove(name).type;
    }
    public Move()
    {
        this.name = "";

        maxpp = 0;
        pp = 0;
        type = null;
    }
}
public enum Status
{
    Ok,
    Sleep,
    Burn,
    Poison,
    Paralyzed,
    Frozen,
    Fainted
}