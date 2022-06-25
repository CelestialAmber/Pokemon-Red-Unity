using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleDebug : MonoBehaviour
{
     public class BattleStatus{
        public BattleStatus(){}
        public int attackLevel, defenseLevel, speedLevel, specialLevel;
        public bool[] isDisabled = new bool[4];
    }

    public GameObject moveMenu, bagMenu, partyMenu;
    public GameObject battleOptionsContainer, backButtonObject;
    public GameObject itemPrefab;
    public TMP_Text playerMonNameText, playerMonLevelText, playerMonHPText;
    public TMP_Text enemyMonNameText, enemyMonLevelText, enemyMonHPText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
