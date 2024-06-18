using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public enum TerraType
{
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
    Fairy,
    Stellar
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
    None, LevelUp, UseItem, HeldItemAndLevel
}
public enum Weather
{
    None, Sun, Rain, Sand, Snow, Fog
}

public enum KyuremForms
{
    None, Black, White
}
public enum ExtraPokemonForms
{
    None,       //put this if this doesn't apply to the Pokemon
    Zen,        //put this if the Pokemon is the Zen Mode variant of Darmanitan
    LowKey,     //put this if the Pokemon is the Low Key variant of Toxtricity
    Bloodmoon,  //put this if the Pokemon is the bloodmoon variant of ursaluna
    LycanNight, LycanDay, LycanDusk, //put one of these if the Pokemon is a Lycanroc
    DeoxysNormal, DeoxysAttack, DeoxysDefense, DeoxysSpeed, //put one of these if the Pokemon is a Deoxys
    ZTen, ZFifty, ZComplete, //put one of these is the Pokemon is a Zygarde
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, exclamation, question, //put one of these if the Pokemon is Unown
    spring, summer, fall, winter //put one of these if the Pokemon is Deerling or Sawsbuck

}
public enum ReginalForm
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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("----- UI -----")]
    public GameObject MainScreen;
    public GameObject StarterSelection;
    public Image ThrownBall;
    [Header("----- Enemy -----")]
    public Image EnemySprite;
    public Image EnemyHP;
    public GameObject EnemyCritBox;
    public TextMeshProUGUI EnemyLevel;
    public TextMeshProUGUI EnemyName;
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
    public Button ThrowBall;
    public List<Button> Starters;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        StarterSelection.SetActive(true);
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
            {
                multiplier *= 2;
            }
            if (type == dType2)
            {
                multiplier *= 2;
            }
        }
        foreach (PokemonType type in PokemonList.SuperEffectiveTypes[(int)aType2])
        {
            if (type == dType1)
            {
                multiplier *= 2;
            }
            if (type == dType2)
            {
                multiplier *= 2;
            }
        }
        foreach (PokemonType type in PokemonList.NotVeryEffectiveTypes[(int)aType1])
        {
            if (type == dType1)
            {
                multiplier /= 2;
            }
            if (type == dType2)
            {
                multiplier /= 2;
            }
        }
        foreach (PokemonType type in PokemonList.NotVeryEffectiveTypes[(int)aType2])
        {
            if (type == dType1)
            {
                multiplier /= 2;
            }
            if (type == dType2)
            {
                multiplier /= 2;
            }
        }
        foreach (PokemonType type in PokemonList.ImmuneTypes[(int)aType1])
        {
            if (type == dType1)
            {
                multiplier = 0.1f;
            }
            if (type == dType2)
            {
                multiplier = 0.1f;
            }
        }
        foreach (PokemonType type in PokemonList.ImmuneTypes[(int)aType2])
        {
            if (type == dType1)
            {
                multiplier = 0.1f;
            }
            if (type == dType2)
            {
                multiplier = 0.1f;
            }
        }
        return multiplier;
    }
}
public class Pokemon
{
    public int dexID;                   //The pokemon's National Pokedex numeber
    public int evolveLevel;             //The level a Pokemon needs to evolve (0 if no level required or can't evolve)
    public EvolveMethod evolveMethod;
    public int evoDexID;                //the national dex number the pokemon evolves into (0 if the pokemon can't evolve)
    public PokemonType type1;
    public PokemonType type2;



    //Forms for:
    //Paldean Tauros, (None(Combat), Fire(Blaze), Water(Aqua))
    //Burmy/Wormadam, (Grass(Plant), Ground(Sandy), Steel(Trash))
    //Rotom, (Ghost(Normal), Fire(Heat), Water(Wash), Ice(Frost), Flying(Fan), Grass(Mow)) 
    //Oricorio, (Fire(Baile), Electric(Pom-Pom), Psychic(Pa'u), Ghost(Sensu))
    //Calyrex, (Grass(Unmounted), Ice(Ice Rider), Ghost(Shadow Rider))
    //Ogerpon, (None(Teal), Water(Wellspring), Fire(Hearthflame), Rock(Cornerstone))
    //Urshifu, (Dark(Single), Water(Rapid))
    //Necrozma, (None, Steel(Dusk Mane), Ghost(Dawn Wings), Dragon(Ultra))
    //Shaymin, (None(Land), Flying(Sky))
    //Hoopa, (Ghost(Confined), Dark(Unbound))
    //Meloetta (Psychic(Aria), Fighting(Pirouette))
    public PokemonType typeForm;
    public ExtraPokemonForms extraPokemonForm;
    public ReginalForm reginalForm;




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

    public bool Origin; //Origin form Dialga, Palkia, Giratina
    public bool GenieAlt; //Therian form Tornadus, Thundarus, Landarus, Enamorus
    public bool Primal; //Primal Groudon, Kyogre
    public bool Hero; //Palafin Zero (false) or Hero (true)
    public KyuremForms kyurem; //Kyurem fusion forms, Reshiram, Zekrom


    public Pokemon()    //Remove this constructor when the Pokemon Database is setup
    {
        dexID = Random.Range(1, 1025);
        evolveLevel = 0;
        evolveLevel = 0;
        evoDexID = 0;
        type1 = PokemonType.None; 
        type2 = PokemonType.None;

        level = Random.Range(5, 60);
        maxHP = (int)(level * Random.Range(2, 3));
        currHP = maxHP;
        exp = 0;
        typeForm = PokemonType.None;
        extraPokemonForm = ExtraPokemonForms.None;
        reginalForm = ReginalForm.None;
        terraType = TerraType.Normal;
    }
    public Pokemon(Pokemon pokemon)
    {
        dexID = pokemon.dexID;
        evolveLevel = pokemon.evolveLevel;
        evolveMethod = pokemon.evolveMethod;
        evoDexID = pokemon.evoDexID;
        type1 = pokemon.type1;
        type2 = pokemon.type2;


        level = pokemon.level;
        maxHP = pokemon.maxHP;
        currHP = pokemon.currHP;
        exp = 0;

        typeForm = pokemon.typeForm;
        extraPokemonForm = pokemon.extraPokemonForm;
        reginalForm = pokemon.reginalForm;
        terraType = pokemon.terraType;
    }
    public Pokemon(int _dexID, int _evolveLevel, EvolveMethod _evolveMethod, int _evoDexID, PokemonType _type1, PokemonType _type2, int _level = 0)
    {
        dexID= _dexID;
        evolveLevel= _evolveLevel;
        evolveMethod= _evolveMethod;
        evoDexID = _evoDexID;
        type1 = _type1;
        type2 = _type2;
        if (_level == 0)
            level = Random.Range(5, 60);
        else
            level = _level;
        maxHP = (int)(level * Random.Range(2, 3));
        currHP = maxHP;
        exp = 0;
        typeForm = PokemonType.None;
        extraPokemonForm = ExtraPokemonForms.None;
        reginalForm = ReginalForm.None;
        terraType = TerraType.Normal;
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