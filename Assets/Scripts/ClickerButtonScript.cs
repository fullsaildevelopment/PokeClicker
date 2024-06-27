using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool IsFainting;
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
        if (IsFainting)
        {
            Vector3 pos = GameManager.Instance.EnemySprite.gameObject.transform.localPosition;
            pos.y -= 4000 * Time.deltaTime;
            GameManager.Instance.EnemySprite.gameObject.transform.localPosition = pos;
            GameManager.Instance.EnemyFaintMask.SetActive(true);
            if (GameManager.Instance.EnemySprite.gameObject.transform.localPosition.y <= -400)
            {
                IsFainting = false;
                GameManager.Instance.EnemySprite.gameObject.transform.localPosition = new Vector3(500, 0, 0);
                GameManager.Instance.EnemySprite.enabled = false;
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
        yield return new WaitForSeconds(0.3f);
        IsFainting = true;
        yield return new WaitForSeconds(0.7f);
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
            int id = 0;
            while (id == 0)
            {
                id = Random.Range(1, 149);
                switch (id) //Don't want to spawn a legendary
                {
                    case 144:
                        id = 0;
                        break;
                    case 145:
                        id = 0;
                        break;
                    case 146:
                        id = 0;
                        break;
                }
            }
            enemy = new Pokemon(PokemonList.PokemonData[id]);
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = 1;
        }
        else if (GameManager.Instance.stageType == StageType.Trainer)
        {
            int id = 0;
            while (id == 0)
            {
                id = Random.Range(1, 149);
                switch (id) //Don't want to spawn a legendary
                {
                    case 144:
                        id = 0;
                        break;
                    case 145:
                        id = 0;
                        break;
                    case 146:
                        id = 0;
                        break;
                }
            }
            enemy = new Pokemon(PokemonList.PokemonData[id]);
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = 1;
        }
        else if (GameManager.Instance.stageType == StageType.MiniBoss)
        {
            List<int> MiniBossIDs = new List<int>()
            {
                3, 6, 9, 18, 31, 34, 38, 59, 65, 68, 89, 94, 95, 112, 130, 131, 142, 143, 149
            };
            int bossLvl = (GameManager.Instance.StageNumber - (GameManager.Instance.StageNumber % 10)) + Random.Range(1, 4);
            enemy = new Pokemon(PokemonList.PokemonData[MiniBossIDs[Random.Range(0, MiniBossIDs.Count - 1)]], bossLvl);
            for (int i = 0; i < 2; ++i)
            {
                GameManager.Instance.MiniBossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.MiniBossHPBars[i].fillAmount = 1;
            }
            currHPBar = 1;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.Boss)
        {
            int bossLvl = (GameManager.Instance.StageNumber - (GameManager.Instance.StageNumber % 10)) + Random.Range(1, 4);
            List<int> BossIDs = new List<int>()
            {
                144, 145, 146
            };
            enemy = new Pokemon(PokemonList.PokemonData[BossIDs[Random.Range(0, BossIDs.Count - 1)]], bossLvl);
            for (int i = 0; i < 4; ++i)
            {
                GameManager.Instance.BossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.BossHPBars[i].fillAmount = 1;
            }
            currHPBar = 3;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.BigBoss)
        {
            int bossLvl = (GameManager.Instance.StageNumber - (GameManager.Instance.StageNumber % 10)) + Random.Range(1, 4);
            enemy = new Pokemon(PokemonList.PokemonData[150], bossLvl);
            for (int i = 0; i < 6; ++i)
            {
                GameManager.Instance.BigBossHPBars[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.BigBossHPBars[i].fillAmount = 1;
            }
            currHPBar = 5;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.Special)
        {
            enemy = new Pokemon(PokemonList.PokemonData[151]);
            GameManager.Instance.EnemyHP.color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHP.fillAmount = 1;
        }

        GameManager.Instance.EnemyLevel.text = "lv." + enemy.level.ToString();
        GameManager.Instance.EnemySprite.sprite = Resources.Load<Sprite>("Pokemon/Normal/" + PokemonList.pokemonIDs[enemy.dexID].ToLower());
        GameManager.Instance.EnemyName.text = PokemonList.pokemonNames[enemy.dexID];
        SwitchEnemyStatBox();
        GameManager.Instance.EnemyFaintMask.SetActive(false);
        GameManager.Instance.EnemySprite.enabled = true;
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