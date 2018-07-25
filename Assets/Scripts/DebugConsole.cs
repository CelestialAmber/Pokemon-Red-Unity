using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugConsole : MonoBehaviour
{
    public GameObject template, container, window;
    public InputField field;
    public static bool isActive;
    // Update is called once per frame
    void Update()
    {
        DebugConsole.isActive = window.activeSelf;
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            window.SetActive(!window.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            FieldParse();
            field.text = "";
        }
    }
    public void FieldParse()
    {
        ParseCommand(field.text);
    }
    void ParseCommand(string input)
    {

        string token = "";
        int branch = -1;
        if (input.Contains("/set pokedex seen ") || input.Contains("/set pokedex own "))
        {
            if (input.Contains("/set pokedex seen "))
            {
                token = "/set pokedex seen ";
                branch = 0;
            }
            if (input.Contains("/set pokedex own "))
            {
                token = "/set pokedex own ";
                branch = 1;
            }
            int arg = -1;
            input = input.Substring(input.IndexOf(token) + token.Length);
            if (input.Contains("true"))
            {
                arg = 1;
            }
            if (input.Contains("false"))
            {
                arg = 0;
            }
            if (arg > -1)
            {
                switch (arg)
                {
                    case 0:
                        token = "false ";
                        break;
                    case 1:
                        token = "true ";
                        break;

                }
                input = input.Substring(input.IndexOf(token) + token.Length);
                int var = 0;
                if (int.TryParse(input, out var))
                {
                    switch (branch)
                    {
                        case 0:
                            SaveData.pokedexlist[var - 1].seen = (arg == 1 ? true : false);
                            Message("Set " + PokemonData.IndexToMon(var) + (arg == 0 ? " as not seen." : " as seen."));
                            break;
                        case 1:
                            SaveData.pokedexlist[var - 1].seen = (arg == 1 ? true : false);
                            SaveData.pokedexlist[var - 1].caught = (arg == 1 ? true : false);
                            Message("Set " + PokemonData.IndexToMon(var) + (arg == 0 ? " as not caught." : " as caught."));
                            break;
                    }
                }else Message("Invalid command.");

            }else Message("Invalid command.");

        }else if(input == "/gen party"){
            Get.pokeMenu.party.Clear();
            for (int i = 0; i < 6; i++)
            {
                
                Get.pokeMenu.party.Add(new Pokemon(PokemonData.IndexToMon(Random.Range(1, 152)), Random.Range(1, 101)));
                Get.pokeMenu.healthbars[i] = Get.pokeMenu.partyslots[i].transform.GetChild(1).GetChild(0).GetComponent<Image>();
            }
            Message("Generated a new party.");

        }else if(input == "/credits"){
            Player.disabled = true;
            Get.player.credits.SetActive(true);
        }
        else Message("Invalid command.");

    }
    void Message(string message)
    {
        GameObject newLog = Instantiate(template, container.transform);
        newLog.GetComponent<Text>().text = message;
        List<GameObject> messagesToKeep = new List<GameObject>();
        for (int i = container.transform.childCount - 1; i > container.transform.childCount - 1 - 10; i--){
            if (i < 0) break;
            messagesToKeep.Add(container.transform.GetChild(i).gameObject);

        }
        foreach(Transform child in container.transform){
            if (!messagesToKeep.Contains(child.gameObject)) Destroy(child.gameObject);
        }
    }
}
