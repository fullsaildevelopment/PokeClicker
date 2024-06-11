using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickerButtonScript : MonoBehaviour
{
    [Header("----- Stats -----")]
    public int maxHP;
    public int currHP;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
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
            newPokemon();
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
        maxHP = (int)(level * Random.Range(1.5f, 2.5f));
        currHP = maxHP;
        GameManager.Instance.EnemyLevel.text = "lv." + level.ToString();
        int DexID = Random.Range(1, 1025);
        GameManager.Instance.EnemySprite.sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[DexID].ToLower());
        GameManager.Instance.EnemyName.text = PokemonList.pokemonNames[DexID];

    }
}
