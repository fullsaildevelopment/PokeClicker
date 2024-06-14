using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
public enum TimeOfDay
{
    Night, Day, Dusk
}
public enum UnownLetters
{
    None, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, exclamation, question
}

public enum DeoxysForms
{
    None, Normal, Attack, Defense, Speed
}

public enum KyuremForms
{
    None, Black, White
}
public enum ZygardeForms
{
    None, Ten, Fifty, Complete
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("----- UI -----")]
    public GameObject Canvas;
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
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
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
    public int DexID = 0;
    public int maxHP = 1;
    public int currHP = 1;
    public int level = 1;
    public int exp = 0;

    //pokemon in this game can have multiple held items
    public List<HeldItem> items = new List<HeldItem>();

    public PokemonType type1 = PokemonType.None;
    public PokemonType type2 = PokemonType.None; //not all pokemon have a second type

    //Generational Gimmicks / Forms
    //(any pre-evolutions may have this set for when it evolves)
    public bool Mega = false, currMega = false; //is active until the pokemon faints or is stored away
    public bool GMax = false; //is active for 30 seconds
    public TerraType terraType = TerraType.Normal; //is active until the pokemon faints or switches out


    //Reginal Forms
    //(any pre-evolutions may have this set for when it evolves)
    public bool Alolan = false;
    public bool Galarian = false;
    public bool Hisuian = false;
    public bool Paldean = false;

    //Specific Pokemon Forms / Gimmicks
    //(any pre-evolutions may have this set for when it evolves)
    public bool isShedinja = false; //Shedinja 1 hp gimmick

    public bool Origin = false; //Origin form Dialga, Palkia, Giratina
    public bool GenieAlt = false; //Therian form Tornadus, Thundarus, Landarus, Enamorus
    public bool Primal = false; //Primal Groudon, Kyogre

    public bool Amped = false; //Toxtricity LowKey (false) or Amped (true)
    public bool Bloodmoon = false; //Ursaluna Normal (false) or Bloodmoon (true)
    public bool Hero = false; //Palafin Zero (false) or Hero (true)

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
    public PokemonType TypeForms = PokemonType.None;

    public TimeOfDay Lycanroc = TimeOfDay.Day; //Lycanroc forms, Day, Night, Dusk
    public UnownLetters Unown = UnownLetters.None; //Unown Letter Forms
    public DeoxysForms Deoxys = DeoxysForms.None; //Deoxys forms, Normal, Attack, Speed, Defense
    public KyuremForms Kyurem = KyuremForms.None; //Kyurem fusion forms, Reshiram, Zekrom
    public ZygardeForms Zygarde = ZygardeForms.None; //Zygarde 10%, 50%, Complete
}