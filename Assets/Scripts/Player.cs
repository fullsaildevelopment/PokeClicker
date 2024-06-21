using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [Header("----- Party -----")]
    public int currSlot;
    public List<Pokemon> party = new List<Pokemon>(6);
    public bool takingDamage;
    public bool gainingExp;
    public bool CanAttack;
    int txtHP;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CanAttack = false;    
        takingDamage = false;
        gainingExp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingDamage)
        {
            GameManager.Instance.PlayerHP.fillAmount -= 1 * Time.deltaTime;
            txtHP = (int)(GameManager.Instance.PlayerHP.fillAmount * party[currSlot].maxHP);
            GameManager.Instance.PlayerHPNum.text = txtHP.ToString() + "/" + party[currSlot].maxHP;
            GameManager.Instance.PlayerHP.color = GameManager.Instance.GetHPColor(GameManager.Instance.PlayerHP.fillAmount);
            if (GameManager.Instance.PlayerHP.fillAmount <= (float)party[currSlot].currHP / party[currSlot].maxHP
                && txtHP <= party[currSlot].currHP)
            {
                takingDamage = false;
                GameManager.Instance.PlayerHP.fillAmount = (float)party[currSlot].currHP / party[currSlot].maxHP;
                txtHP = party[currSlot].currHP;
                GameManager.Instance.PlayerHPNum.text = txtHP.ToString() + "/" + party[currSlot].maxHP;
            }
        }
        //if (gainingExp)
        //{
        //    GameManager.Instance.PlayerEXP.fillAmount += 1 * Time.deltaTime;
        //    if (GameManager.Instance.PlayerEXP.fillAmount >= party[currSlot].exp / (float)Math.Pow(party[currSlot].level, 3))
        //    {
        //        gainingExp = false;
        //        
        //    }
        //}
    }
    public void SelectStarter(Pokemon starter)
    {
        AddToParty(starter);
        SetActivePokemon(0);
        CanAttack = true;
    }
    public void AddToParty(Pokemon pokemon)
    {
        if (party.Count < party.Capacity)
        {
            party.Add(pokemon);
            GameManager.Instance.PartySlots[party.Count - 1].interactable = true;
        }
    }
    public void AddEXP(int exp, int slot)
    {
        party[slot].exp += exp;
        if (slot == currSlot)
            GameManager.Instance.PlayerEXP.fillAmount = party[currSlot].exp / (float)Math.Pow(party[currSlot].level, 3);
        CanLevelUp(party[slot], slot);
    }
    void CanLevelUp(Pokemon pokemon, int slot)
    {
        while (pokemon.exp >= Math.Pow(pokemon.level, 3))
        {
            pokemon = LevelUp(pokemon);
            party[slot] = pokemon;
        }
    }
    Pokemon LevelUp(Pokemon pokemon)
    {

        pokemon.exp = Math.Abs(pokemon.exp - (int)Math.Pow(pokemon.level, 3));
        pokemon.level++;
        int HPUp = UnityEngine.Random.Range(2, 3);
        pokemon.maxHP += HPUp;
        pokemon.currHP += HPUp * 2;
        if (pokemon.currHP > pokemon.maxHP)
            pokemon.currHP = pokemon.maxHP;
        txtHP = party[currSlot].currHP;
        GameManager.Instance.PlayerHP.fillAmount = (float)pokemon.currHP / pokemon.maxHP;
        GameManager.Instance.PlayerHPNum.text = pokemon.currHP.ToString() + "/" + pokemon.maxHP.ToString();
        GameManager.Instance.PlayerHP.color = GameManager.Instance.GetHPColor(GameManager.Instance.PlayerHP.fillAmount);
        GameManager.Instance.PlayerEXP.fillAmount = pokemon.exp / (float)Math.Pow(party[currSlot].level, 3);
        GameManager.Instance.PlayerLevel.text = "lv." + pokemon.level.ToString();
        return pokemon;
    }
    public void SetActivePokemon(int slot)
    {
        takingDamage = false;
        currSlot = slot;
        GameManager.Instance.PlayerSprite.sprite = Resources.Load<Sprite>("Pokemon/NormalBack/" + PokemonList.pokemonIDs[party[slot].dexID].ToLower());
        GameManager.Instance.PlayerSprite.enabled = true;
        GameManager.Instance.PlayerLevel.text = "lv." + party[slot].level.ToString();
        GameManager.Instance.PlayerName.text = PokemonList.pokemonNames[party[slot].dexID];
        GameManager.Instance.PlayerHP.fillAmount = (float)party[slot].currHP / party[slot].maxHP;
        GameManager.Instance.PlayerHP.color = GameManager.Instance.GetHPColor(GameManager.Instance.PlayerHP.fillAmount);
        GameManager.Instance.PlayerHPNum.text = party[slot].currHP.ToString() + "/" + party[slot].maxHP.ToString();
        GameManager.Instance.PlayerEXP.fillAmount = party[slot].exp / (float)Math.Pow(party[slot].level, 3);
        txtHP = party[currSlot].currHP;
        EnemyAI.Instance.PauseAttack(false);
        CanAttack = true;
    }
    public void TakeDamage(int damage)
    {
        party[currSlot].currHP -= damage;
        if (party[currSlot].currHP < 0) 
            party[currSlot].currHP = 0;
        takingDamage = true;
        //GameManager.Instance.PlayerHPNum.text = party[currSlot].currHP.ToString() + "/" + party[currSlot].maxHP.ToString();
        if (party[currSlot].currHP <= 0)
        {
            EnemyAI.Instance.PauseAttack(true);
            GameManager.Instance.PlayerSprite.enabled = false;
            CanAttack = false;
            if (PartyWipe())
            {
                GameManager.Instance.GameOverScreen.SetActive(true);
            }
        }
    }
    bool PartyWipe()
    {
        foreach (Pokemon pokemon in party)
        {
            if (pokemon.currHP > 0)
            {
                return false;
            }
        }
        return true;
    }
    
}
