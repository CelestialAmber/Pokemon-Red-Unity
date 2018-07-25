using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Pokedex : MonoBehaviour
{
    public GameObject entriescontainer;
    public List<GameObject> entries;
    public Get get = new Get();
    public Cursor cursor;
    public CustomText seentext, owntext;
    public int selectedSlot, topSlotIndex;
    public bool selectingMon;
    int seen
    {
        get
        {
            int seennumber = 0;
            foreach (PokedexEntry entry in SaveData.pokedexlist)
            {
                if (entry.seen) seennumber++;
            }
            return seennumber;
        }
    }
    int caught
    {
        get
        {
            int caughtnumber = 0;
            foreach (PokedexEntry entry in SaveData.pokedexlist)
            {
                if (entry.caught) caughtnumber++;
            }
            return caughtnumber;
        }
    }
    void Awake()
    {
        entries = new List<GameObject>();
        entries.Clear();
        for (int i = 0; i < 7; i++)
        {
            entries.Add(entriescontainer.transform.GetChild(i).gameObject);
        }
    }
    public void Init()
    {
        topSlotIndex = 1;
        selectedSlot = 0;
        seentext.text = seen.ToString();
        owntext.text = caught.ToString();

        UpdateScreen();

    }

    void UpdateScreen()
    {

        for (int i = 0; i < 7; i++)
        {
            int slotNo = topSlotIndex + i;
            entries[i].transform.GetChild(0).GetComponent<CustomText>().text = slotNo.ZeroFormat("00x") + "\n" + (!SaveData.pokedexlist[slotNo - 1].seen ? "   ----------" : "   " + PokemonData.IndexToMonUpper(slotNo));
            entries[i].transform.GetChild(1).gameObject.SetActive(SaveData.pokedexlist[slotNo - 1].caught);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!selectingMon)
            cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20.1f, -3.2f - 1 * (selectedSlot));
        if(! cursor.isActive)
        cursor.SetActive(true);
        if (get.Bio().displayingbio) cursor.SetActive(false);
        if (get.menu().currentmenu == get.menu().pokedexmenu)
        {
            if (Inputs.pressed("b") && !get.Bio().displayingbio)
            {
                if (Player.disabled) Player.disabled = false;
                if (selectingMon)
                {
                    selectingMon = false;
                }
                else
                {
                    get.menu().donewaiting = false;
                    get.menu().currentmenu = get.menu().thismenu;

                    gameObject.SetActive(false);
                }
            }
            if (Inputs.pressed("a") && !get.Bio().displayingbio)
            {

                if (!selectingMon && SaveData.pokedexlist[topSlotIndex + selectedSlot - 1].seen)
                {
                    selectingMon = true;
                    cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(-12.78f, -6.7f);
                }
                else if (SaveData.pokedexlist[topSlotIndex + selectedSlot - 1].seen)
                {
                    StartCoroutine(get.Bio().DisplayABio(topSlotIndex + selectedSlot));
                }

            }
            if (Inputs.pressed("down"))
            {
                if (!selectingMon)
                {
                    selectedSlot++;
                    if (selectedSlot > 6)
                    {
                        selectedSlot = 6;
                        if (topSlotIndex < SaveData.pokedexlist.Count - 6)
                        {
                            topSlotIndex += 1;
                        }
                        UpdateScreen();
                    }
                }
            }
            if (Inputs.pressed("up"))
            {
                if (!selectingMon)
                {
                    selectedSlot--;
                    if (selectedSlot < 0)
                    {
                        selectedSlot = 0;
                        if (topSlotIndex > 1)
                        {
                            topSlotIndex -= 1;
                        }
                        UpdateScreen();
                    }
                }
            }
            if (Inputs.pressed("right"))
            {
                if (!selectingMon)
                {
                    topSlotIndex += 10;
                    if (topSlotIndex > SaveData.pokedexlist.Count - 6) topSlotIndex = SaveData.pokedexlist.Count - 6;
                    UpdateScreen();
                }
            }
            if (Inputs.pressed("left"))
            {
                if (!selectingMon)
                {
                    topSlotIndex -= 10;
                    if (topSlotIndex < 1) topSlotIndex = 1;
                    UpdateScreen();
                }
            }
        }

    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Pokedex))]
public class PokedexDebug : Editor
{
    public int OverwriteIndex;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        OverwriteIndex = EditorGUILayout.IntField("Selected Index", OverwriteIndex);
        if (GUILayout.Button("Set Pokedex Entry to Seen"))
        {
            SaveData.pokedexlist[OverwriteIndex - 1].seen = true;
        }
        if (GUILayout.Button("Set Pokedex Entry to Owned"))
        {
            SaveData.pokedexlist[OverwriteIndex - 1].caught = true;
        }
    }
}
#endif

