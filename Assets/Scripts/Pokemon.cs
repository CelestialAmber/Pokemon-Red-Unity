using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Pokemon
{
    public int id;
    public int maxHP;
    public int attack;
    public int defense;
    public int speed;
    public int special;
    public int currentHP;
    public bool isWildPokemon;
    public Status status;
    public int[] ivs = new int[5];
    public int[] evs = new int[5];
    public string name;
    public int level;
    public string nickname;
    public int ownerID;
    public string owner;
    public int experience;
    public Types[] types;
    public Move[] moves;
    public int numberOfMoves;

    public Pokemon(PokemonEnum pokemonId, int level, bool isWildPokemon)
    {
        PokemonDataEntry pokemonDataEntry = PokemonData.pokemonData[(int)pokemonId - 1];
        this.id = pokemonDataEntry.id;
        this.name = pokemonDataEntry.name;
        this.level = level;
        this.isWildPokemon = isWildPokemon;
        this.nickname = this.name;
        types = pokemonDataEntry.types;
        GenerateIvs();
        moves = new Move[4];
        for(int i = 0; i < 4; i++){
            moves[i] = new Move();
        }
        UpdateMovesToLevel();
        SetExpToLevel();
        RecalculateStats();
        //If the Pokemon hasn't been registered as caught before, register it
        if (!GameData.instance.pokedexlist[id - 1].caught)
        {
            RegisterInDex();
        }
        if(!isWildPokemon){

            ownerID = GameData.instance.trainerID;
            owner = GameData.instance.playerName;
        }

    }

    public void RegisterInDex()
    {

        GameData.instance.pokedexlist[id - 1].seen = true;
        if(!isWildPokemon) GameData.instance.pokedexlist[id - 1].caught = true;

    }
    public void RecalculateStats()
    {
        PokemonDataEntry pokemonDataEntry = PokemonData.pokemonData[id - 1];

        maxHP = Mathf.FloorToInt(((pokemonDataEntry.baseStats[0] + ivs[0]) * 2 + Mathf.Sqrt(evs[0]) / 4) * level / 100) + level + 10;
        attack = Mathf.FloorToInt(((pokemonDataEntry.baseStats[1] + ivs[1]) * 2 + Mathf.Sqrt(evs[1]) / 4) * level / 100) + 5;
        defense = Mathf.FloorToInt(((pokemonDataEntry.baseStats[2] + ivs[2]) * 2 + Mathf.Sqrt(evs[2]) / 4) * level / 100) + 5;
        speed = Mathf.FloorToInt(((pokemonDataEntry.baseStats[3] + ivs[3]) * 2 + Mathf.Sqrt(evs[3]) / 4) * level / 100) + 5;
        special = Mathf.FloorToInt(((pokemonDataEntry.baseStats[4] + ivs[4]) * 2 + Mathf.Sqrt(evs[4]) / 4) * level / 100) + 5;
        currentHP = maxHP;
    }
    public void GenerateIvs()
    {
        ivs[1] = Random.Range(0, 16);
        ivs[2] = Random.Range(0, 16);
        ivs[3] = Random.Range(0, 16);
        ivs[4] = Random.Range(0, 16);
        ivs[0] = (ivs[1] % 2 == 1 ? 8 : 0) + (ivs[2] % 2 == 1 ? 4 : 0) + (ivs[3] % 2 == 1 ? 2 : 0) + (ivs[4] % 2 == 1 ? 1 : 0);
    }
    public void SetExpToLevel()
    {
        experience = CalculateExp(level).UnderflowUInt24(); //Underflow by 16,777,216;
    }
    public int ExpToNextLevel()
    {
        if (level >= 100) return experience;
        int result = CalculateExp(level + 1).UnderflowUInt24(); //Underflow by 16,777,216;
         return (result - experience).UnderflowUInt24();
        
    }
    public int CalculateExp(int level){
        switch (PokemonData.pokemonData[id - 1].expGroup)
        {
            case 0: //Slow
                return Mathf.FloorToInt(5 * Mathf.Pow(level, 3) / 4f);
            case 1: //Medium Slow
                return Mathf.FloorToInt((6f / 5f) * Mathf.Pow(level, 3) - 15 * Mathf.Pow(level, 2) + 100 * level - 140);
            case 2: //Medium Fast
                return Mathf.FloorToInt(Mathf.Pow(level, 3));
            case 3: //Fast
                return Mathf.FloorToInt(4 * Mathf.Pow(level, 3) / 5f);
            default:
                throw new UnityException("Invalid experience index was given.");

        }
    }

    int CalculateLevelFromExp(){
        for(int i = 0; i <= 100; i++){ //iterate through each level to find the level from the current exp
            int exp = CalculateExp(i);
            if(exp > experience) return i - 1; //if the current level exp is higher, return the previous level
        }
        return 100; //if the current experience is higher than the level 100 experience, return 100
    }

    bool AlreadyHasMove(Moves moveIndex)
    {
        foreach (Move move in moves)
        {
            if (move.moveIndex == (int)moveIndex)
                return true;

        }
        return false;
    }

    Move MoveAtCurrentLevel(){
        /*
        foreach(LevelUpMove move in PokemonData.pokemonData[id - 1].levelupLearnset){
            if(move.Item2 == level) return new Move(move.Item1);
        }
        */
        return null;
    }
    
    public void UpdateMovesToLevel()
    {
        /*
        for (int i = 0; i < PokemonData.pokemonData[id - 1].levelupLearnset.Length; i++)
        {
            LevelUpMove movetocheck = PokemonData.pokemonData[id - 1].levelupLearnset[i];
            if (level >= movetocheck.level)
            {
                if (!AlreadyHasMove(movetocheck.move))
                    if (numberOfMoves < 4)
                    {
                        moves[numberOfMoves] = new Move(movetocheck.move);
                    }
                    else
                    {
                        moves[0] = new Move(movetocheck.move);
                    }
            }
        }
        */
        //iterate through all moves learned by level, and adjust the move pool accordingly
    }
    public void AddMove(int moveIndex){
        if(numberOfMoves == 4){
             Debug.Log("Cannot add a new move, there are already 4 moves");
        }
        for(int i = 0; i < 4; i++){
            if(!SlotHasMove(i)){
             moves[i] = new Move(moveIndex);
            numberOfMoves++;
             break;
            }       
        }
        return;
    }
    public void SetMove(Moves move,int moveIndex){
        if(moveIndex > 3 || moveIndex < 0) throw new UnityException("Invalid move index");
            moves[moveIndex] = (new Move((int)move));

    }

    public bool SlotHasMove(int index){
        return moves[index].moveIndex != (int)Moves.None;
    }
}

public class PokemonNameLevel {
    public string name;
    public int level;

    public PokemonNameLevel(){}
}