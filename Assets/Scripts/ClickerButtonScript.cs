using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerButtonScript : MonoBehaviour
{
    public static ClickerButtonScript Instance;
    [Header("----- Stats -----")]
    public int maxHP;
    public int currHP;
    public int level;
    public Pokemon enemy;
    public bool takingDamage;

    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        takingDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingDamage)
        {
            GameManager.Instance.EnemyHP.fillAmount -= 2 * Time.deltaTime;
            GameManager.Instance.PlayerHP.color = GameManager.Instance.GetHPColor(GameManager.Instance.PlayerHP.fillAmount);
            if (GameManager.Instance.EnemyHP.fillAmount <= (float)currHP / maxHP)
            {
                takingDamage = false;
                GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
            }
        }
    }

    public void click()
    {
        if (Player.Instance.party[Player.Instance.currSlot].currHP > 0 && Player.Instance.CanAttack)
        {
            int damage = (int)(Player.Instance.party[Player.Instance.currSlot].level * 1.5f);
            damage = (int)(damage * GameManager.Instance.TypeMatchup(Player.Instance.party[Player.Instance.currSlot], enemy));
            if (crit())
                damage *= 2;
            currHP -= damage;
            takingDamage = true;
            //StartCoroutine(UpdateHPBar((float)currHP / maxHP));
            if (currHP <= 0)
            {
                StartCoroutine(FaintPokemon());
            }
        }
    }
    private bool crit()
    {
        float rng = Random.Range(0.0f, 1.0f);
        if (rng >= 0.99f)
        {
            Instantiate(GameManager.Instance.EnemyCritBox, GameManager.Instance.MainScreen.transform);
            return true;
        }
        return false;
    }
    IEnumerator FaintPokemon()
    {
        Player.Instance.CanAttack = false;
        yield return new WaitForSeconds(1);
        int exp = (100 * level) / 7;
        Player.Instance.AddEXP(exp, Player.Instance.currSlot);
        GameManager.Instance.IncreaseStageEnemiesDefeated();
        newPokemon();
    }
    public void newPokemon()
    {
        EnemyAI.Instance.ResetAttack();
        enemy = new Pokemon();
        level = enemy.level;
        maxHP = enemy.maxHP;
        currHP = enemy.currHP;
        GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
        GameManager.Instance.EnemyLevel.text = "lv." + level.ToString();
        GameManager.Instance.EnemySprite.sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[enemy.dexID].ToLower());
        GameManager.Instance.EnemyName.text = PokemonList.pokemonNames[enemy.dexID];
        GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
        Player.Instance.CanAttack = true;
    }
}