using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TerraType
{
    Stellar,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}
public enum HeldItem
{
    None
}
public enum EvolveMethod 
{
    //For move levelup, just change it to level up the level after the move could be learned in the normal games
    //if the Pokemon has multiple evo paths that use the same methods or isn't listed, let the player choose or make it random.

    None, LevelUp,
    WaterStone, ThunderStone, FireStone, DawnStone, DuskStone, LeafStone, IceStone, SunStone, MoonStone, 
    LinkingCable, GalaricaCuff, GalaricaWreath, BlackAugurite,
    LinkKingsRock, LinkMetalCoat, LinkProtector, LinkDragonScale, LinkElectirizer, LinkMagmarizer,
    LinkUpgrade, LinkDubiousDisc,
    LvlUpFriendshipToken, LvlUpOvalStone,
    Crit3Times

        //If its not listed yet, add it here
        //Don't add level up by move or time of day methods
        //Friendship levelups work by having the pokemon hold the friendship token and leveling up.
}
public enum Weather
{
    None, Sun, Rain, Sand, Snow, Fog
}

public enum KyuremForms
{
    None, Black, White
}
public enum StaticPokemonForms   //These forms cannot change and are set on creation
{
    None, //put this if this doesn't apply to the Pokemon
    PlantCloak, SandyCloak, TrashCloak, //Burmy or Wormadam
    
    RedStripe, BlueStripe, WhiteStripe, //Basculin
    DarmanNormal, DarmanZen, //Darmanitan
    spring, summer, fall, winter, //Deerling or Sawsbuck
    PomPomStyle, BaileStyle, PauStyle, SensuStyle, //Oricorio
    LycanNight, LycanDay, LycanDusk, //Lycanroc
    BlueCore, GreenCore, IndigoCore, OrangeCore, RedCore, VioletCore, YellowCore, //Minior
    Amped, LowKey, //Toxtricity
    SingleStrike, RapidStrike, //Urshifu
    CombatBreed, BlazeBreed, AquaBreed, //Paldean Tauros
    GreenPlumage, YellowPlumage, BluePlumage, WhitePlumage, //Squakabilly
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, exclamation, question, //Unown
    WestSea, EastSea, //Shellos or Gastrodon
    //Add Vivillion later, just use None for now.
    RedFlower, BlueFlower, OrangeFlower, WhiteFlower, YellowFlower, //Flabebe, Floett, or Florges
    //Add Alcemie later, just use None for now.
    Family3, Family4, //Maushold
    Curly, Stretchy, Droopy, //Tatsugiri
    Segment2, Segment3, //Dundunsparce
    UrsaNormal, UrsaBloodmoon,  //Ursaluna

}
public enum DynamicPokemonForms //these forms are changeable and is None by default
{
    None, //use this if the Pokemon is in its normal form or doesn't apply here
    DeoxysAttack, DeoxysDefense, DeoxysSpeed, //Deoxys (changes with meteorite)
    RotoMow, RotoFrost, RotoHeat, RotoFan, RotoWash, //Rotom (changes with appliances)
    Origin, //Dialga, Palkia, Giratina (changes with their orbs)
    Sky, //Shaymin (changes with gracidea flower)
    Therian, //Tornadus, Thundurus, Landorus, Enamorus (changes with Reflective Mirror)
    Black, White, //Kyurem (fused with Reshiram or Zekrom)
    Resolution, //Keldeo (for this game, give it a Secret Sword held item)
    Pirouette, //Meloetta (for this game, give it an ancient music sheet held item)
    //BattleBond, //Greninja (for this game, idk)
    //Blade, //Aegislash
    ZTen, ZFifty, ZComplete, //Zygarde (changes with Z-Cube)
    Unbound, //Hoopa (changes with Prison Bottle)
    School, //Wishiwashi (if over lv20 and 25% hp, use school form)
    Core, //Minior (reveal core if less than 50% hp)
    DuskMane, DawnWings, Ultra, //Necrozma (fused with Solgaleo or Lunala, Ultra is with Z-Move)
    Crowned, //Zacian, Zamazenta (if they have their sword or shield)
    IceRider, ShadowRider, //Calyrex (fuses with Glastrier or Spectrier)
    Hero, //Palafin (becomes hero if switched out from battle)
    Sunny, Rainy, Snowy, //Castform (changes with weather) also Cherrim (only Sunny)
    Hangry, //Morpeko (in this game, will switch after 10 seconds)
    Wellspring, Hearthflame, Cornerstone //Ogerpon (mask held item)
}

public enum RegionalForm
{
    None,
    Alolan,
    Galarian,
    Hisuian,
    Paldean
}
public enum EXPType
{
    MediumFast,     //1,000,000 exp at lv 100
    Erratic,        //600,000 exp at lv 100
    Fluctuating,    //1,640,000 exp at lv 100
    MediumSlow,     //1,059,860 exp at lv 100
    Fast,           //800,000 exp at lv 100
    Slow            //1,250,000 exp at lv 100
}
public enum StageType
{
    Regular,
    Trainer,
    MiniBoss,
    Boss,
    BigBoss,
    Special
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("----- UI -----")]
    public GameObject MainScreen;
    public GameObject StarterSelection;
    public GameObject GameOverScreen;
    public Image ThrownBall;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI SkillShardText;

    [Header("----- Game Stats -----")]
    public int StageNumber;
    public int StageEnemiesDefeated;

    [Header("----- Enemy -----")]
    public Image EnemySprite;
    public GameObject EnemyCritBox;
    public TextMeshProUGUI EnemyLevel;
    public TextMeshProUGUI EnemyName;
    public GameObject EnemyFaintMask;

    public Image BossStatBGShadow;
    public Image BossStatBG;
    public List<GameObject> EnemyHPBars;
    public List<Image> EnemyHPSliders;

    

    [Header("----- Player -----")]
    public Image PlayerSprite;
    public Image PlayerHP;
    public TextMeshProUGUI PlayerHPNum;
    public Image PlayerEXP;
    //public GameObject PlayerCritBox;
    public TextMeshProUGUI PlayerLevel;
    public TextMeshProUGUI PlayerName;
    [Header("----- Buttons -----")]
    public List<Button> PartySlots;
    public List<Image> PartySlotSprites;
    public List<TextMeshProUGUI> PartySlotNames;
    public List<TextMeshProUGUI> PartySlotLevels;
    public List<TextMeshProUGUI> PartySlotHPTexts;
    public List<Image> PartySlotHPBars;
    public Button ThrowBall;
    public List<Button> Starters;

    public StageType stageType;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        StarterSelection.SetActive(true);
        setStage(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float TypeMatchup(Pokemon AttackingPokemon, Pokemon DefendingPokemon)
    {
        PokemonType aType1 = AttackingPokemon.type1;
        PokemonType aType2 = AttackingPokemon.type2;
        PokemonType dType1 = DefendingPokemon.type1;
        PokemonType dType2 = DefendingPokemon.type2;
        float multiplier = 1;
        foreach (PokemonType type in PokemonList.SuperEffectiveTypes[(int)aType1])
        {
            if (type == dType1)
                multiplier *= 2;
            if (type == dType2)
                multiplier *= 2;
        }
        foreach (PokemonType type in PokemonList.SuperEffectiveTypes[(int)aType2])
        {
            if (type == dType1)
                multiplier *= 2;

            if (type == dType2)
                multiplier *= 2;
        }
        foreach (PokemonType type in PokemonList.NotVeryEffectiveTypes[(int)aType1])
        {
            if (type == dType1)
                multiplier /= 2;
            if (type == dType2)
                multiplier /= 2;
        }
        foreach (PokemonType type in PokemonList.NotVeryEffectiveTypes[(int)aType2])
        {
            if (type == dType1)
                multiplier /= 2;
            if (type == dType2)
                multiplier /= 2;
        }
        foreach (PokemonType type in PokemonList.ImmuneTypes[(int)aType1])
        {
            if (type == dType1)
                multiplier = 0.1f;
            if (type == dType2)
                multiplier = 0.1f;
        }
        foreach (PokemonType type in PokemonList.ImmuneTypes[(int)aType2])
        {
            if (type == dType1)
                multiplier = 0.1f;
            if (type == dType2)
                multiplier = 0.1f;
        }
        return multiplier;
    }
    Pokemon CreatePokemon(int _dexID, RegionalForm rForm = RegionalForm.None, StaticPokemonForms sForm = StaticPokemonForms.None,
                          bool RandomizeRegionalForm = false, bool RandomizeStaticForm = false)
    {   //IMPLEMENT THIS FUNCTION AFTER THE POKEMON DATABASE IS SETUP
        RegionalForm regionalForm = rForm;
        StaticPokemonForms staticForm = sForm;
        //Randomization if enabled
        if (RandomizeRegionalForm)
        {
            List<Pokemon> forms = new List<Pokemon>();
            forms.Add(PokemonList.PokemonData[_dexID]);
            for (int i = 0; i < PokemonList.RegionalPokemonData.Count; ++i)
            {
                if (PokemonList.RegionalPokemonData[i].dexID == _dexID)
                    forms.Add(PokemonList.RegionalPokemonData[i]);
            }
            regionalForm = forms[Random.Range(1, forms.Count) - 1].reginalForm;
        }
        if (RandomizeStaticForm)
        {
            List<Pokemon> forms = new List<Pokemon>();
            forms.Add(PokemonList.PokemonData[_dexID]);
            for (int i = 0; i < PokemonList.StaticFormPokemonData.Count; ++i)
            {
                if (PokemonList.StaticFormPokemonData[i].dexID == _dexID)
                {
                    forms.Add(PokemonList.StaticFormPokemonData[i]);
                }
            }
            staticForm = forms[Random.Range(1, forms.Count) - 1].staticForm;
        }

        //Searching for and returning the Pokemon
        if ((regionalForm != RegionalForm.None && staticForm != StaticPokemonForms.None) || staticForm != StaticPokemonForms.None)
        { //if the Pokemon has a static form and/or a regional form too
            foreach (Pokemon pokemon in PokemonList.StaticFormPokemonData)
            {
                if (pokemon.dexID == _dexID && pokemon.reginalForm == regionalForm && pokemon.staticForm == staticForm)
                    return new Pokemon(pokemon);
            }
        }
        else if (regionalForm != RegionalForm.None)
        { //if the pokemon just has a regional form
            foreach (Pokemon pokemon in PokemonList.RegionalPokemonData)
            {
                if (pokemon.dexID == _dexID && pokemon.reginalForm == regionalForm)
                    return new Pokemon(pokemon);
            }
        }
        return new Pokemon(PokemonList.PokemonData[_dexID]);
    }
    public void setStage(int stagenum)
    {
        StageNumber = stagenum;
        StageText.text = "Stage " + StageNumber.ToString();
        StageEnemiesDefeated = 0;
        if (StageNumber % 100 == 0)
        {
            stageType = StageType.BigBoss;
            ThrowBall.interactable = false;
        }
        else if (StageNumber % 50 == 0)
        {
            stageType = StageType.Boss;
            ThrowBall.interactable = false;
        }
        else if (StageNumber % 10 == 0)
        {
            stageType = StageType.MiniBoss;
            ThrowBall.interactable = false;
        }
        else if (StageNumber % 25 == 0 && StageNumber > 100)
        {
            stageType = StageType.Special;
            ThrowBall.interactable = true;
        }
        else if (StageNumber % 5 == 0)
        {
            //stageType = StageType.Trainer;
            stageType = StageType.Regular;
            ThrowBall.interactable = true;
        }
        else
        {
            stageType = StageType.Regular;
            ThrowBall.interactable = true;
        }
    }
    public void IncreaseStageEnemiesDefeated()
    {
        StageEnemiesDefeated++;
        if ((StageEnemiesDefeated >= 10 && stageType == StageType.Regular) || (StageEnemiesDefeated != 0 && stageType != StageType.Regular))
        {
            setStage(StageNumber + 1);
        }
    }
    public Color GetHPColor(float percent)
    {
        if (percent <= 0.2f)
            return new Color((float)225 / 255, 0, 0);
        else if (percent <= 0.5f)
            return Color.yellow;
        else
            return new Color(0, (float)225 / 255, 0);
    }

    public void UpdatePartyButton(int partySlot, Pokemon pokemon)
    {
        PartySlotSprites[partySlot].sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[pokemon.dexID].ToLower());
        PartySlotSprites[partySlot].enabled = true;
        PartySlotNames[partySlot].text = PokemonList.pokemonNames[pokemon.dexID];
        PartySlotLevels[partySlot].text = "lv." + pokemon.level.ToString();
        PartySlotHPBars[partySlot].fillAmount = (float)pokemon.currHP / pokemon.maxHP;
        PartySlotHPBars[partySlot].transform.parent.gameObject.SetActive(true);
        PartySlotHPTexts[partySlot].text = pokemon.currHP.ToString() + "/" + pokemon.maxHP.ToString();
    }
    public void ClearPartySlot(int partySlot)
    {
        PartySlotSprites[partySlot].enabled = false;
        PartySlotNames[partySlot].text = "";
        PartySlotLevels[partySlot].text = "";
        PartySlotHPBars[partySlot].transform.parent.gameObject.SetActive(false);
        PartySlotHPTexts[partySlot].text = "";
    }
}
public class Pokemon
{
    public int dexID;                   //The pokemon's National Pokedex numeber
    public List<int> evolveLevels = new List<int>();      //The level a Pokemon needs to evolve (0 if no level required or can't evolve)
    public List<EvolveMethod> evolveMethods = new List<EvolveMethod>();
    public List<int> evoDexIDs = new List<int>();         //the national dex number the pokemon evolves into (put its own ID if the pokemon can't evolve)
    public PokemonType type1;
    public PokemonType type2;

    public DynamicPokemonForms dynamicForm;
    public StaticPokemonForms staticForm;
    public RegionalForm reginalForm;

    public int maxHP;
    public int currHP;
    public int level;
    public int exp;

    //pokemon in this game can have multiple held items
    public List<HeldItem> items = new List<HeldItem>();



    //Generational Gimmicks / Forms
    //(any pre-evolutions may have this set for when it evolves)
    public bool Mega = false;   //can the Pokemon MegaEvolve?
    public bool isMega = false; //is active until the pokemon faints or is stored away
    public bool GMax = false; //can the Pokemon Gigantamax?
    public bool isDyna = false; //is active for 3 turns or until the Pokemon faints or switches out
    public TerraType terraType; //is active until the pokemon faints or switches out

    //Main Constructor to use when generating Pokemon to use.
    public Pokemon(Pokemon pokemon, int _level = 0)
    {
        dexID = pokemon.dexID;
        evolveLevels.AddRange(pokemon.evolveLevels);
        evolveMethods.AddRange(pokemon.evolveMethods);
        evoDexIDs.AddRange(pokemon.evoDexIDs);
        type1 = pokemon.type1;
        type2 = pokemon.type2;

        if (_level != 0)
        {
            level = _level;
            maxHP = (int)(level * Random.Range(3, 8));
            currHP = maxHP;
        }
        else
        {
            int min = GameManager.Instance.StageNumber - (GameManager.Instance.StageNumber % 5);
            int max = min + 8;
            if (min <= 1)
                min = 3;
            level = Random.Range(min, max);
            maxHP = (int)(level * Random.Range(3, 8));
            currHP = maxHP;
        }
        exp = 0;

        dynamicForm = DynamicPokemonForms.None;
        staticForm = pokemon.staticForm;
        reginalForm = pokemon.reginalForm;
        if (type2 != PokemonType.None)
        {
            if (Random.Range(0, 1) >= 0.5f)
                terraType = (TerraType)type1;
            else
                terraType = (TerraType)type2;
        }
        else
            terraType = (TerraType)type1;
    }
    //Constructor for preset Pokemon that have only 1 evolve path or none.
    public Pokemon(int _dexID, int _evolveLevel, EvolveMethod _evolveMethod, int _evoDexID,
                   PokemonType _type1, PokemonType _type2,
                   RegionalForm _reginalForm = RegionalForm.None, 
                   StaticPokemonForms _staticForm = StaticPokemonForms.None)
    {
        dexID = _dexID;
        evolveLevels.Add(_evolveLevel);
        evolveMethods.Add(_evolveMethod);
        evoDexIDs.Add(_evoDexID);
        type1 = _type1;
        type2 = _type2;
        reginalForm = _reginalForm;
        staticForm = _staticForm;
    }
    //Constructor for preset Pokemon that have more than 1 evolve path.
    public Pokemon(int _dexID, int[] _evolveLevels, EvolveMethod[] _evolveMethods, int[] _evoDexIDs, 
                   PokemonType _type1, PokemonType _type2,
                   RegionalForm _reginalForm = RegionalForm.None,
                   StaticPokemonForms _staticForm = StaticPokemonForms.None)
    {
        dexID = _dexID;
        evolveLevels.AddRange(_evolveLevels);
        evolveMethods.AddRange(_evolveMethods);
        evoDexIDs.AddRange(_evoDexIDs);
        type1 = _type1;
        type2 = _type2;
        reginalForm = _reginalForm;
        staticForm = _staticForm;
    }


    
}
public class BallType
{
    public string BallName;
    //Sprite BallSprite;
    public float BaseCatchRate;
    public BallType(string ballName, float baseCatchRate)
    {
        BallName = ballName;
        BaseCatchRate = baseCatchRate;
    }
}