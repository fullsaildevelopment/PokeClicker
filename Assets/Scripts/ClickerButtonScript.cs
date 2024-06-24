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
    int currHPBar;
    int currHPSlider;

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
            if (GameManager.Instance.stageType == StageType.Regular || GameManager.Instance.stageType == StageType.Trainer || GameManager.Instance.stageType == StageType.Special)
            {
                GameManager.Instance.EnemyHP.fillAmount -= 2 * Time.deltaTime;
                GameManager.Instance.PlayerHP.color = GameManager.Instance.GetHPColor(GameManager.Instance.PlayerHP.fillAmount);
                if (GameManager.Instance.EnemyHP.fillAmount <= (float)currHP / maxHP)
                {
                    takingDamage = false;
                    GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
                }
            }
            else if (GameManager.Instance.stageType == StageType.MiniBoss)
            {
                GameManager.Instance.MiniBossHPBars[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.MiniBossHPBars[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.MiniBossHPBars[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHP.fillAmount = 0;
                        if (currHPSlider != 0)
                        {
                            currHPSlider--;
                        }
                    }
                }
            }
            else if (GameManager.Instance.stageType == StageType.Boss)
            {
                GameManager.Instance.BossHPBars[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.BossHPBars[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.BossHPBars[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHP.fillAmount = 0;
                        if (currHPSlider != 0)
                        {
                            currHPSlider--;
                        }
                    }
                }
            }
            else if (GameManager.Instance.stageType == StageType.BigBoss)
            {
                GameManager.Instance.BigBossHPBars[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.BigBossHPBars[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHP.fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.BigBossHPBars[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHP.fillAmount = 0;
                        if (currHPSlider != 0)
                        {
                            currHPSlider--;
                        }
                    }
                }
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
            if (GameManager.Instance.stageType == StageType.Regular || GameManager.Instance.stageType == StageType.Trainer || GameManager.Instance.stageType == StageType.Special)
            {
                if (currHP <= 0)
                {
                    EnemyAI.Instance.PauseAttack(true);
                    StartCoroutine(FaintPokemon());
                }
            }
            else if (GameManager.Instance.stageType == StageType.MiniBoss || GameManager.Instance.stageType == StageType.Boss || GameManager.Instance.stageType == StageType.BigBoss)
            {
                if (currHP <= 0 && currHPBar != 0)
                {
                    currHPBar--;
                    currHP += maxHP;
                }
                else if (currHP <= 0 && currHPBar <= 0)
                {
                    EnemyAI.Instance.PauseAttack(true);
                    StartCoroutine(FaintPokemon());
                }
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
        Economy.Instance.UpdateSkillShards(exp / 2);
        GameManager.Instance.IncreaseStageEnemiesDefeated();
        newPokemon();
    }
    public void newPokemon()
    {
        
        if (GameManager.Instance.stageType == StageType.Regular)
        {
            enemy = new Pokemon();
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = (float)enemy.currHP / enemy.maxHP;
        }
        else if (GameManager.Instance.stageType == StageType.Trainer)
        {
            enemy = new Pokemon();
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = (float)enemy.currHP / enemy.maxHP;
        }
        else if (GameManager.Instance.stageType == StageType.MiniBoss)
        {
            enemy = new Pokemon(6, 0, EvolveMethod.None, 0, PokemonType.Fire, PokemonType.Flying, RegionalForm.None, StaticPokemonForms.None, (GameManager.Instance.StageNumber / 2) + 3);
            for (int i = 0; i < 2; ++i)
            {
                GameManager.Instance.MiniBossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.MiniBossHPBars[i].fillAmount = (float)enemy.currHP / enemy.maxHP;
            }
            currHPBar = 1;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.Boss)
        {
            enemy = new Pokemon(646, 0, EvolveMethod.None, 0, PokemonType.Dragon, PokemonType.Ice, RegionalForm.None, StaticPokemonForms.None, (GameManager.Instance.StageNumber / 2) + 3);
            for (int i = 0; i < 4; ++i)
            {
                GameManager.Instance.BossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.BossHPBars[i].fillAmount = (float)enemy.currHP / enemy.maxHP;
            }
            currHPBar = 3;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.BigBoss)
        {
            enemy = new Pokemon(493, 0, EvolveMethod.None, 0, PokemonType.Normal, PokemonType.None, RegionalForm.None, StaticPokemonForms.None, (GameManager.Instance.StageNumber / 2) + 3);
            for (int i = 0; i < 6; ++i)
            {
                GameManager.Instance.BigBossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.BigBossHPBars[i].fillAmount = (float)enemy.currHP / enemy.maxHP;
            }
            currHPBar = 5;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.Special)
        {
            enemy = new Pokemon();
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = (float)enemy.currHP / enemy.maxHP;
        }

        GameManager.Instance.EnemyLevel.text = "lv." + enemy.level.ToString();
        GameManager.Instance.EnemySprite.sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[enemy.dexID].ToLower());
        GameManager.Instance.EnemyName.text = PokemonList.pokemonNames[enemy.dexID];
        SwitchEnemyStatBox();
        level = enemy.level;
        maxHP = enemy.maxHP;
        currHP = enemy.currHP;
        EnemyAI.Instance.PauseAttack(false);
        Player.Instance.CanAttack = true;
    }

    void SwitchEnemyStatBox()
    {
        if (GameManager.Instance.stageType == StageType.Regular || GameManager.Instance.stageType == StageType.Trainer || GameManager.Instance.stageType == StageType.Special)
        {
            GameManager.Instance.EnemyStats.SetActive(true);
            GameManager.Instance.MiniBossStats.SetActive(false);
            GameManager.Instance.BossStats.SetActive(false);
            GameManager.Instance.BigBossStats.SetActive(false);
        }
        else if (GameManager.Instance.stageType == StageType.MiniBoss)
        {
            GameManager.Instance.EnemyStats.SetActive(false);
            GameManager.Instance.MiniBossStats.SetActive(true);
            GameManager.Instance.BossStats.SetActive(false);
            GameManager.Instance.BigBossStats.SetActive(false);
        }
        else if (GameManager.Instance.stageType == StageType.Boss)
        {
            GameManager.Instance.EnemyStats.SetActive(false);
            GameManager.Instance.MiniBossStats.SetActive(false);
            GameManager.Instance.BossStats.SetActive(true);
            GameManager.Instance.BigBossStats.SetActive(false);
        }
        else if (GameManager.Instance.stageType == StageType.BigBoss)
        {
            GameManager.Instance.EnemyStats.SetActive(false);
            GameManager.Instance.MiniBossStats.SetActive(false);
            GameManager.Instance.BossStats.SetActive(false);
            GameManager.Instance.BigBossStats.SetActive(true);
        }
    }
}