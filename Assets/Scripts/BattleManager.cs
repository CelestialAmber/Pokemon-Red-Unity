using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public enum BattleType
{
    Wild,
    Trainer,
    Safari,
    OldMan
}
public enum BattleState
{
    Intro,
    PlayerTurn,
    Attacking,
    End
}

public class BattleManager : MonoBehaviour
{
    public class BattleStatus{
    public BattleStatus(){
    }
    public int attackLevel, defenseLevel, speedLevel, specialLevel;
    public bool[] isDisabled = new bool[4];
    }

    public int battleID;

    public List<Pokemon> enemyMons = new List<Pokemon>();
    public GameObject playerstats, enemystatsObject;
    public Animator battleMainAnim, battleTransitionAnim;
    public GameObject playerpokeballs, enemyballs;
    public Image battleoverlay, frontportrait, backportrait;
    public GameObject playerMonObject, playerObject, trainerObject, enemyMonObject;
    //current loaded playermon stats
    public Pokemon playermon;
    public CustomText playerHpText, playerName, playermonLeveltext;
    public Image playerHPBar;
    public Image[] playerPartyBalls;
    //current loaded enemymon stats

    public List<Pokemon> enemyParty;
    public Pokemon enemymon;
    public CustomText enemymonLeveltext, enemymonname;
    public Image enemyHPBar;
    public Image[] enemyPartyBalls;
    //
    public GameObject currentmenu;
    public GameObject battlemenu, movesmenu;
    public GameObject[] allmenus;
    public GameCursor cursor;
    public BattleType battleType;
    public int selectedOption;
    public int currentLoadedMon;
    public GameObject battleBG;
    public Sprite[] partyBallSprites;
    public Sprite[] battleOverlaySprites;
    public Sprite blank;
    /*
	Battle Overlay Documentation:
	0:initial1
	1:initial2
	2:hp_all
	3:hp_enemy
	4:trainer_party
	5:player_only
	 */
    public AudioClip sendOutMonClip, runClip;
    public GameObject bgTextbox;
    public BattleState battleState;

    public int transitionType;

    public bool isFadingIn;

    public GameObject battleTransitionShaderObj;

    public Material shrinkSplitMat; //material for the shader transition effects

    public TilemapRenderer grassTilemap;


    // Use this for initialization

    public void Initialize()
    {
        if (GameData.instance.party.Count == 0) throw new UnityException("The player has no Pokemon!");
        battleState = BattleState.Intro;
        if (battleType == BattleType.Trainer)
        {
            // switch(battleID)


        }
        currentLoadedMon = 0;
        foreach (Pokemon pokemon in enemyMons)
        {
            pokemon.RecalculateStats();
        }
        enemymon = enemyMons[0];
        playermon = GameData.instance.party[0];
        playermon.RecalculateStats();
        enemyHPBar.fillAmount = (Mathf.Round(enemymon.currentHP * 48 / enemymon.maxHP)) / 48;
        playerHPBar.fillAmount = (Mathf.Round(playermon.currentHP * 48 / playermon.maxHP)) / 48;
        playerName.text = playermon.nickname;
        battleoverlay.gameObject.SetActive(true);
        playerstats.SetActive(false);
        enemystatsObject.SetActive(false);
        playerpokeballs.SetActive(false);
        enemyballs.SetActive(false);
        DetermineBackSprite();
        DetermineFrontSprite();
        StartCoroutine(BattleInit());

    }

    public IEnumerator BattleInit()
    {
        Player.instance.holdingDirection = false;
        DetermineBattleTransition();
        while (isFadingIn) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2.5f);
        battleBG.SetActive(true);
        bgTextbox.SetActive(true);
        ScreenEffects.flashLevel = 0;


        if (battleType == BattleType.Trainer)
        {
            StartCoroutine(TrainerBattleStart());
        }
        if (battleType == BattleType.Wild)
        {
            StartCoroutine(WildBattleStart());
        }

    }
    public void DetermineBattleTransition()
    {
        foreach (MapCollider mapCol in MapManager.instance.mapColliders)
        {
            mapCol.grassTilemap.GetComponent<TilemapRenderer>().sortingOrder = 0;
        }
        DoBattleTransition();
    }
    public void DoBattleTransition()
    {
        switch (transitionType)
        {
            case 6:
                StartCoroutine(BattleTransitionShrink());
                break;
            case 7:
                StartCoroutine(BattleTransitionSplit());
                break;
            default:
                StartCoroutine(BattleTransitionMain());
                break;
        }
    }

    public IEnumerator BattleTransitionMain()
    {
        battleTransitionAnim.SetFloat("fadeType", transitionType);
        battleTransitionAnim.SetTrigger("fadeIn");
        isFadingIn = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(battleTransitionAnim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Finished transition");
        FinishedBattleTransition();
    }
    public IEnumerator BattleTransitionShrink()
    {
        isFadingIn = true;
        battleTransitionShaderObj.SetActive(true);
        int offset = 0;
        shrinkSplitMat.SetInt("uvOffset", offset);
        for (int i = 0; i < 9; i++)
        {
            offset -= 8;
            shrinkSplitMat.SetInt("uvOffset", offset);
            yield return new WaitForSeconds(6f / 60f); //wait 6 frames
        }
        battleTransitionShaderObj.SetActive(false);
        FinishedBattleTransition();

    }
    public IEnumerator BattleTransitionSplit()
    {
        isFadingIn = true;
        battleTransitionShaderObj.SetActive(true);
        int offset = 0;
        shrinkSplitMat.SetInt("uvOffset", offset);
        for (int i = 0; i < 9; i++)
        {
            offset += 8;
            shrinkSplitMat.SetInt("uvOffset", offset);
            yield return new WaitForSeconds(6f / 60f); //wait 6 frames
        }
        battleTransitionShaderObj.SetActive(false);
        FinishedBattleTransition();

    }

    public IEnumerator TrainerBattleStart()
    {
        yield return 0;
    }
    public IEnumerator WildBattleStart()
    {
        //enemy x pos starts at -28, and goes to 124
        //player x pos starts at 188, and goes to 36
        UpdatePokeBallUI();
        UpdateStatsUI();
        playerObject.SetActive(true);
        enemyMonObject.SetActive(true);

        float initialTimer = 0f;

        while (initialTimer < 1.2f)
        {
            initialTimer += Time.deltaTime;
            playerObject.transform.localPosition = Vector3.Lerp(new Vector3(188, 76, 0), new Vector3(38, 76, 0), initialTimer / 1.2f);
            enemyMonObject.transform.localPosition = Vector3.Lerp(new Vector3(-28, 116, 0), new Vector3(124, 116, 0), initialTimer / 1.2f);
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(SoundManager.instance.PlayCryCoroutine(enemymon.id - 1));

        battleoverlay.sprite = battleOverlaySprites[0];
        playerpokeballs.SetActive(true);
        Dialogue.instance.fastText = true;
        yield return Dialogue.instance.text("Wild " + enemyMons[0].nickname + "&lappeared!");
        enemystatsObject.SetActive(true);
        playerpokeballs.SetActive(false);
        battleoverlay.sprite = battleOverlaySprites[3];
        initialTimer = 0f;
        yield return new WaitForSeconds(1f);

        playermon = GameData.instance.party[0];
        enemymon = enemyMons[0];
        Dialogue.instance.keepTextOnScreen = true;
        Dialogue.instance.needButtonPress = false;
        yield return Dialogue.instance.text("Go! " + playermon.nickname + "!");

        while (initialTimer < 0.6f)
        {
            initialTimer += Time.deltaTime;
            playerObject.transform.localPosition = Vector3.Lerp(new Vector3(38, 76, 0), new Vector3(-26, 76, 0), initialTimer / 0.6f);
            yield return new WaitForEndOfFrame();
        }

        battleMainAnim.SetTrigger("sendOutMon");
        yield return new WaitForSeconds(1f);

        while (SoundManager.instance.isPlayingCry)
        {
            yield return new WaitForEndOfFrame();
        }

        Dialogue.instance.Deactivate();
        selectedOption = 0;
        battleState = BattleState.PlayerTurn;
        currentmenu = battlemenu;
        cursor.SetActive(true);
    }
    public void FinishedBattleTransition()
    {
        ScreenEffects.flashLevel = -3;
        foreach (MapCollider mapCol in MapManager.instance.mapColliders)
        {
            mapCol.grassTilemap.GetComponent<TilemapRenderer>().sortingOrder = 3;
        }
        isFadingIn = false;
    }
    public void SendOutMonSound()
    {
        Debug.Log("playing sendout clip");
        SoundManager.instance.sfx.PlayOneShot(sendOutMonClip);
    }
    public void SetBackMonImageActive()
    {
        playerMonObject.SetActive(true);
        playerstats.SetActive(true);
        SoundManager.instance.PlayCry(playermon.id - 1);
        battleoverlay.sprite = battleOverlaySprites[2];
    }
    void UpdateMenus()
    {
        foreach (GameObject menu in allmenus)
        {
            if (menu != currentmenu)
            {
                menu.SetActive(false);
            }
            else
            {

                menu.SetActive(true);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        UpdateMenus();
        if (battleState == BattleState.PlayerTurn && Dialogue.instance.finishedText)
        {
            UpdateStatsUI();

            if (currentmenu == battlemenu)
            {
                cursor.SetPosition(48 * (selectedOption % 2) + 72, -16 * Mathf.FloorToInt((float)selectedOption / 2f) + 24);

                if (InputManager.Pressed(Button.Left))
                {

                    if (selectedOption == 1)
                    {
                        selectedOption = 0;
                        return;
                    }

                    if (selectedOption == 3)
                    {
                        selectedOption = 2;
                        return;
                    }
                }

                if (InputManager.Pressed(Button.Right))
                {

                    if (selectedOption == 0)
                    {
                        selectedOption = 1;
                        return;
                    }

                    if (selectedOption == 2)
                    {
                        selectedOption = 3;
                        return;
                    }
                }

                if (InputManager.Pressed(Button.Up))
                {
                    if (selectedOption == 2)
                    {
                        selectedOption = 0;
                        return;
                    }
                    if (selectedOption == 3)
                    {
                        selectedOption = 1;
                        return;
                    }
                }

                if (InputManager.Pressed(Button.Down))
                {
                    if (selectedOption == 0)
                    {
                        selectedOption = 2;
                        return;
                    }
                    if (selectedOption == 1)
                    {
                        selectedOption = 3;
                        return;
                    }
                }

                if (InputManager.Pressed(Button.A))
                {
                    if (selectedOption == 3)
                    {
                        currentmenu = null;
                        cursor.SetActive(false);
                        SoundManager.instance.sfx.PlayOneShot(runClip);
                        Player.instance.RunFromBattle();
                        UpdateMenus();
                    }
				}
            }
        }
        else
        {
            currentmenu = null;
        }
    }

    public void DetermineFrontSprite()
    {
        frontportrait.overrideSprite = GameData.instance.frontMonSprites[enemymon.id - 1];

    }

    public void DetermineBackSprite()
    {
        backportrait.overrideSprite = GameData.instance.backMonSprites[playermon.id - 1];

    }

    void UpdateStatsUI()
    {
        enemymonLeveltext.text = (enemymon.level != 100 ? "<LEVEL>" : "") + enemymon.level.ToString();
        playermonLeveltext.text = (playermon.level != 100 ? "<LEVEL>" : "") + playermon.level.ToString();
        playerHpText.text = (playermon.currentHP > 99 ? "" : playermon.currentHP > 9 ? " " : "  ") + playermon.currentHP + " " + playermon.maxHP;
        enemymonname.text = enemymon.name.ToUpper();
    }

    void UpdatePokeBallUI()
    {
        for (int i = 0; i < 6; i++)
        {
            if (GameData.instance.party.Count >= i + 1)
            {

                if (GameData.instance.party[i].status == Status.Ok)
                {
                    playerPartyBalls[i].sprite = partyBallSprites[0];
                }
                else if (GameData.instance.party[i].status == Status.Fainted)
                {
                    playerPartyBalls[i].sprite = partyBallSprites[3];
                }
                else
                {
                    playerPartyBalls[i].sprite = partyBallSprites[1];
                }
            }
            else
            {
                playerPartyBalls[i].sprite = partyBallSprites[2];
            }

            if (enemyParty.Count >= i + 1)
            {
                if (enemyParty[i].status == Status.Ok)
                {
                    enemyPartyBalls[i].sprite = partyBallSprites[0];
                }
                else if (enemyParty[i].status == Status.Fainted)
                {
                    enemyPartyBalls[i].sprite = partyBallSprites[3];
                }
                else
                {
                    enemyPartyBalls[i].sprite = partyBallSprites[1];
                }
            }
            else
            {
                enemyPartyBalls[i].sprite = partyBallSprites[2];
            }
        }
    }

    void UseMove(Moves move)
    {
        MoveData moveData = PokemonData.GetMove((int)move);

        switch (moveData.effect)
        {
            case MoveEffect.NoEffect: break;
            case MoveEffect.TwoFiveEffect: break;
            case MoveEffect.PayDayEffect: break;
            case MoveEffect.BurnSideEffect1: break;
            case MoveEffect.FreezeSideEffect: break;
            case MoveEffect.ParalyzeSideEffect1: break;
            case MoveEffect.OhkoEffect: break;
            case MoveEffect.ChargeEffect: break;
            case MoveEffect.AttackUp2Effect: break;
            case MoveEffect.SwitchTeleportEffect: break;
            case MoveEffect.FlyEffect: break;
            case MoveEffect.TrappingEffect: break;
            case MoveEffect.FlinchSideEffect2: break;
            case MoveEffect.DoubleAttackEffect: break;
            case MoveEffect.JumpKickEffect: break;
            case MoveEffect.AccuracyDown1Effect: break;
            case MoveEffect.ParalyzeSideEffect2: break;
            case MoveEffect.RecoilEffect: break;
            case MoveEffect.ThrashEffect: break;
            case MoveEffect.DefenseDown1Effect: break;
            case MoveEffect.PoisonSideEffect1: break;
            case MoveEffect.TwinNeedleEffect: break;
            case MoveEffect.FlinchSideEffect1: break;
            case MoveEffect.AttackDown1Effect: break;
            case MoveEffect.SleepEffect: break;
            case MoveEffect.ConfusionEffect: break;
            case MoveEffect.SpecialDamageEffect: break;
            case MoveEffect.DisableEffect: break;
            case MoveEffect.DefenseDownSideEffect: break;
            case MoveEffect.MistEffect: break;
            case MoveEffect.ConfusionSideEffect: break;
            case MoveEffect.SpeedDownSideEffect: break;
            case MoveEffect.AttackDownSideEffect: break;
            case MoveEffect.HyperBeamEffect: break;
            case MoveEffect.DrainHpEffect: break;
            case MoveEffect.LeechSeedEffect: break;
            case MoveEffect.SpecialUp1Effect: break;
            case MoveEffect.PoisonEffect: break;
            case MoveEffect.ParalyzeEffect: break;
            case MoveEffect.SpeedDown1Effect: break;
            case MoveEffect.SpecialDownSideEffect: break;
            case MoveEffect.AttackUp1Effect: break;
            case MoveEffect.SpeedUp2Effect: break;
            case MoveEffect.RageEffect: break;
            case MoveEffect.MimicEffect: break;
            case MoveEffect.DefenseDown2Effect: break;
            case MoveEffect.EvasionUp1Effect: break;
            case MoveEffect.HealEffect: break;
            case MoveEffect.DefenseUp1Effect: break;
            case MoveEffect.DefenseUp2Effect: break;
            case MoveEffect.LightScreenEffect: break;
            case MoveEffect.HazeEffect: break;
            case MoveEffect.ReflectEffect: break;
            case MoveEffect.FocusEffect: break;
            case MoveEffect.BideEffect: break;
            case MoveEffect.MetronomeEffect: break;
            case MoveEffect.MirrorMoveEffect: break;
            case MoveEffect.ExplodeEffect: break;
            case MoveEffect.PoisonSideEffect2: break;
            case MoveEffect.BurnSideEffect2: break;
            case MoveEffect.SwiftEffect: break;
            case MoveEffect.SpecialUp2Effect: break;
            case MoveEffect.DreamEaterEffect: break;
            case MoveEffect.TransformEffect: break;
            case MoveEffect.SplashEffect: break;
            case MoveEffect.ConversionEffect: break;
            case MoveEffect.SuperFangEffect: break;
            case MoveEffect.SubstituteEffect: break;
        }
    }


    float moveEffectiveness(Move move, Pokemon target)
    {
        float result = PokemonData.TypeEffectiveness[move.type][target.types[0]];
        if (target.types[1] != Types.None) result *= PokemonData.TypeEffectiveness[move.type][target.types[1]];
        return result;
    }


    int CalculateGainedExperience()
    {
        int exp = 0; //exp = a*b*t*L/(7*s)

        //a = 1.5 if trainer pokemon, 1 if wild
        //b = base experience
        //t = 1.5 if a traded pokemon, 1 otherwise
        //L = player pokemon level
        //s = number of alive pokemon that participated in battle, x2 if the player has the exp all in their bag
        //if(hasexpallinbag) exp /= 2;
        //find out how exp all bonus exp is calculated

        return exp;
    }


    //These functions animate health.
    IEnumerator AnimateOurHealth(int amount){
        int newHealth = playermon.currentHP + amount;
        if (newHealth < 0) newHealth = 0;
        if (newHealth > playermon.maxHP) newHealth = playermon.maxHP;
        int result = Mathf.RoundToInt(newHealth - playermon.currentHP);

        WaitForSeconds wait = new WaitForSeconds(5 / playermon.maxHP);

        for (int l = 0; l < Mathf.Abs(result); l++){
            yield return wait;

            playermon.currentHP += 1 * Mathf.Clamp(result, -1, 1);
            int pixelCount = Mathf.RoundToInt((float)playermon.currentHP * 48 / (float)playermon.maxHP);
            playerHPBar.fillAmount = (float)pixelCount / 48;
        }
        yield return null;
    }


    IEnumerator AnimateEnemyHealth(int amount){
        int newHealth = enemymon.currentHP + amount;
        if (newHealth < 0) newHealth = 0;
        if (newHealth > enemymon.maxHP) newHealth = enemymon.maxHP;
        int result = Mathf.RoundToInt(newHealth - enemymon.currentHP);
        WaitForSeconds wait = new WaitForSeconds(5 / enemymon.maxHP);

        for (int l = 0; l < Mathf.Abs(result); l++){
            yield return wait;

            enemymon.currentHP += 1 * Mathf.Clamp(result, -1, 1);

            int pixelCount = Mathf.RoundToInt((float)enemymon.currentHP * 48 / (float)enemymon.maxHP);
            enemyHPBar.fillAmount = (float)pixelCount / 48;
        }
        yield return null;
    }


    public void Deactivate(){
        battleBG.SetActive(false);
        cursor.SetActive(false);
        playerstats.SetActive(false);
        battleoverlay.sprite = blank;
        battleoverlay.gameObject.SetActive(false);
        enemystatsObject.SetActive(false);
        playerMonObject.SetActive(false);
        enemyMonObject.SetActive(false);
        bgTextbox.SetActive(false);
    }

}
