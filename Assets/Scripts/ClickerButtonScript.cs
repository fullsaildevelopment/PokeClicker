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
    public int takenDamage;

    bool flashEnemySuperEffective;
    bool flashEnemyNotVeryEffective;
    bool flashEnemyImmune;
    float deltaTime;

    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        takingDamage = false;
        flashEnemyImmune = false;
        flashEnemySuperEffective = false;
        flashEnemyNotVeryEffective = false;
        deltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingDamage)
        {
            if (GameManager.Instance.stageType == StageType.Regular || GameManager.Instance.stageType == StageType.Trainer || GameManager.Instance.stageType == StageType.Special)
            {
                GameManager.Instance.EnemyHPSliders[0].fillAmount -= 2 * Time.deltaTime;
                GameManager.Instance.EnemyHPSliders[0].color = GameManager.Instance.GetHPColor(GameManager.Instance.EnemyHPSliders[0].fillAmount);
                if (GameManager.Instance.EnemyHPSliders[0].fillAmount <= (float)currHP / maxHP)
                {
                    takingDamage = false;
                    GameManager.Instance.EnemyHPSliders[0].fillAmount = (float)currHP / maxHP;
                }
            }
            else if (GameManager.Instance.stageType == StageType.MiniBoss)
            {
                GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                GameManager.Instance.EnemyHPSliders[currHPSlider].color = 
                    GameManager.Instance.GetHPColor(GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount);
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = 0;
                        if (currHPSlider != 0)
                        {
                            currHPSlider--;
                        }
                    }
                }
            }
            else if (GameManager.Instance.stageType == StageType.Boss)
            {
                GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                GameManager.Instance.EnemyHPSliders[currHPSlider].color =
                    GameManager.Instance.GetHPColor(GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount);
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = 0;
                        if (currHPSlider != 0)
                        {
                            currHPSlider--;
                        }
                    }
                }
            }
            else if (GameManager.Instance.stageType == StageType.BigBoss)
            {
                GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount -= 2 * Time.deltaTime;
                GameManager.Instance.EnemyHPSliders[currHPSlider].color =
                    GameManager.Instance.GetHPColor(GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount);
                if (currHPSlider == currHPBar)
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= (float)currHP / maxHP)
                    {
                        takingDamage = false;
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = (float)currHP / maxHP;
                    }

                }
                else
                {
                    if (GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount <= 0)
                    {
                        GameManager.Instance.EnemyHPSliders[currHPSlider].fillAmount = 0;
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
            pos.y -= 3000 * Time.deltaTime;
            GameManager.Instance.EnemySprite.gameObject.transform.localPosition = pos;
            GameManager.Instance.EnemyFaintMask.SetActive(true);
            if (GameManager.Instance.EnemySprite.gameObject.transform.localPosition.y <= -300)
            {
                IsFainting = false;
                GameManager.Instance.EnemySprite.gameObject.transform.localPosition = Vector3.zero;
                GameManager.Instance.EnemySprite.enabled = false;
            }
        }
        if (flashEnemySuperEffective)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.EnemySuperEffectiveText.transform.localScale = Vector3.one;
                flashEnemySuperEffective = false;
            }
        }
        if (flashEnemyNotVeryEffective)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.EnemyNotVeryEffectiveText.transform.localScale = Vector3.one;
                flashEnemyNotVeryEffective = false;
            }
        }
        if (flashEnemyImmune)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.EnemyImmuneText.transform.localScale = Vector3.one;
                flashEnemyImmune = false;
            }
        }
        if (GameManager.Instance.EnemyNotVeryEffectiveText.gameObject.activeSelf ||
            GameManager.Instance.EnemySuperEffectiveText.gameObject.activeSelf ||
            GameManager.Instance.EnemyImmuneText.gameObject.activeSelf)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > 1.5f)
            {
                GameManager.Instance.EnemySuperEffectiveText.gameObject.SetActive(false);
                GameManager.Instance.EnemyNotVeryEffectiveText.gameObject.SetActive(false);
                GameManager.Instance.EnemyImmuneText.gameObject.SetActive(false);
            }
        }
    }

    public void click()
    {
        if (Player.Instance.party[Player.Instance.currSlot].currHP > 0 && Player.Instance.CanAttack)
        {
            takenDamage = (int)(Player.Instance.party[Player.Instance.currSlot].level * 1.5f);
            float multiplier = GameManager.Instance.TypeMatchup(Player.Instance.party[Player.Instance.currSlot], enemy);

            takenDamage = (int)(takenDamage * multiplier);
            if (crit())
                takenDamage *= 2;
            if (takenDamage == 0)
                takenDamage = 1;
            currHP -= takenDamage;
            FlashEnemyEffectiveness(multiplier);

            Instantiate(GameManager.Instance.DamageCounter, GameManager.Instance.EnemySprite.transform.parent);
            takingDamage = true;
            if (Player.Instance.CanAttackAnim)
            {
                Player.Instance.startingAttackAnim = true;
            }
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
                    GameManager.Instance.ThrowBall.interactable = false;
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
            Instantiate(GameManager.Instance.EnemyCritBox, GameManager.Instance.BattleScreen.transform);
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
                id = Random.Range(10, 149);
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
            GameManager.Instance.EnemyHPSliders[0].color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHPSliders[0].fillAmount = 1;
        }
        else if (GameManager.Instance.stageType == StageType.Trainer)
        {
            int id = 0;
            while (id == 0)
            {
                id = Random.Range(10, 149);
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
            GameManager.Instance.EnemyHPSliders[0].color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHPSliders[0].fillAmount = 1;
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
                GameManager.Instance.EnemyHPSliders[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.EnemyHPSliders[i].fillAmount = 1;
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
                GameManager.Instance.EnemyHPSliders[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.EnemyHPSliders[i].fillAmount = 1;
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
                GameManager.Instance.EnemyHPSliders[i].color = new Color(0, (float)225 / 255, 0);
                GameManager.Instance.EnemyHPSliders[i].fillAmount = 1;
            }
            currHPBar = 5;
            currHPSlider = currHPBar;
        }
        else if (GameManager.Instance.stageType == StageType.Special)
        {
            enemy = new Pokemon(PokemonList.PokemonData[151]);
            GameManager.Instance.EnemyHPSliders[0].color = new Color(0, (float)225 / 255, 0);
            GameManager.Instance.EnemyHPSliders[0].fillAmount = 1;
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
        GameManager.Instance.ThrowBall.interactable = true;
    }

    void SwitchEnemyStatBox()
    {
        if (GameManager.Instance.stageType == StageType.Regular || GameManager.Instance.stageType == StageType.Trainer || GameManager.Instance.stageType == StageType.Special)
        {
            GameManager.Instance.BossStatBG.transform.localPosition = new Vector3(0, 20, 0);
            GameManager.Instance.BossStatBGShadow.transform.localPosition = new Vector3(0, 20, 0);
            for (int i = 1; i < 6; ++i)
            {
                GameManager.Instance.EnemyHPBars[i].SetActive(false);
            }
        }
        else if (GameManager.Instance.stageType == StageType.MiniBoss)
        {
            GameManager.Instance.BossStatBG.transform.localPosition = new Vector3(0, -11, 0);
            GameManager.Instance.BossStatBGShadow.transform.localPosition = new Vector3(0, -11, 0);
            GameManager.Instance.EnemyHPBars[1].SetActive(true);
        }
        else if (GameManager.Instance.stageType == StageType.Boss)
        {
            GameManager.Instance.BossStatBG.transform.localPosition = new Vector3(0, -73, 0);
            GameManager.Instance.BossStatBGShadow.transform.localPosition = new Vector3(0, -73, 0);
            for (int i = 1; i < 4; ++i)
            {
                GameManager.Instance.EnemyHPBars[i].SetActive(true);
            }
        }
        else if (GameManager.Instance.stageType == StageType.BigBoss)
        {
            GameManager.Instance.BossStatBG.transform.localPosition = new Vector3(0, -135, 0);
            GameManager.Instance.BossStatBGShadow.transform.localPosition = new Vector3(0, -135, 0);
            for (int i = 1; i < 6; ++i)
            {
                GameManager.Instance.EnemyHPBars[i].SetActive(true);
            }
        }
    }
    void FlashEnemyEffectiveness(float multiplier)
    {
        if (multiplier > 1)
        {
            GameManager.Instance.EnemySuperEffectiveText.gameObject.SetActive(true);
            GameManager.Instance.EnemyNotVeryEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.EnemyImmuneText.gameObject.SetActive(false);
            if (!flashEnemySuperEffective)
            {
                GameManager.Instance.EnemySuperEffectiveText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashEnemySuperEffective = true;
            }
        }
        else if (multiplier == 0.1f)
        {
            GameManager.Instance.EnemySuperEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.EnemyNotVeryEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.EnemyImmuneText.gameObject.SetActive(true);
            if (!flashEnemyImmune)
            {
                GameManager.Instance.EnemyImmuneText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashEnemyImmune = true;
            }
        }
        else if (multiplier < 1)
        {
            GameManager.Instance.EnemySuperEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.EnemyNotVeryEffectiveText.gameObject.SetActive(true);
            GameManager.Instance.EnemyImmuneText.gameObject.SetActive(false);
            if (!flashEnemyNotVeryEffective)
            {
                GameManager.Instance.EnemyNotVeryEffectiveText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashEnemyNotVeryEffective = true;
            }
        }
    }
}