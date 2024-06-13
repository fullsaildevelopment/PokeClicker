using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickerButtonScript : MonoBehaviour
{
    public static ClickerButtonScript Instance;
    [Header("----- Stats -----")]
    public int maxHP;
    public int currHP;
    public int level;
    public Pokemon enemy;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy = new Pokemon();
        newPokemon();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void click()
    {
        if (crit())
            currHP -= 20;
        else
            currHP -= 5;

        if (currHP <= 0)
        {
            Player.Instance.AddEXP(level * 2, Player.Instance.currSlot);
            newPokemon();
        }
            
        GameManager.Instance.EnemyHP.fillAmount = (float)currHP/maxHP;
    }
    private bool crit()
    {
        float rng = Random.Range(0.0f, 1.0f);
        if (rng >= 0.99f)
        {
            Instantiate(GameManager.Instance.EnemyCritBox, GameManager.Instance.Canvas.transform);
            return true;
        }
        return false;
    }
    private void newPokemon()
    {
        level = Random.Range(5, 60);
        maxHP = (int)(level * Random.Range(2, 3));
        currHP = maxHP;
        GameManager.Instance.EnemyLevel.text = "lv." + level.ToString();
        int DexID = Random.Range(1, 1025);
        GameManager.Instance.EnemySprite.sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[DexID].ToLower());
        GameManager.Instance.EnemyName.text = PokemonList.pokemonNames[DexID];
        enemy.level = level;
        enemy.maxHP = maxHP;
        enemy.currHP = currHP;
        enemy.DexID = DexID;
    }
}
