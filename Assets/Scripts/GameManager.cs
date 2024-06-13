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
    Fairy
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
}
public class Pokemon
{
    public int DexID = 0;
    public int maxHP = 1;
    public int currHP = 1;
    public int level = 1;
    public int exp = 0;

    public bool Mega = false;
    public bool GMax = false;

    public bool Alolan = false;
    public bool Galarian = false;
    public bool Hisuian = false;
    public bool Paldean = false;
    public TerraType terraType = TerraType.Normal;
}