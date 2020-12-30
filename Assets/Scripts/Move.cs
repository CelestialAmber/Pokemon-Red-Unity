[System.Serializable]
public class Move {
    public int moveIndex;
    public string name;
    public int pp;
    public int maxpp;
    public Types type;


    public Move(int index)
    {
        this.moveIndex = index;

        MoveData moveData = PokemonData.GetMove(index);
        name = moveData.name;
        maxpp = moveData.maxpp;
        pp = maxpp;
        type = moveData.type;
    }
    public Move()
    {
        this.moveIndex = (int)Moves.None;

        maxpp = 0;
        pp = 0;
        type = Types.None; //Look at ths later
    }
}

public enum Status {
    Ok,
    Sleep,
    Burn,
    Poison,
    Paralyzed,
    Frozen,
    Fainted
}