using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [Header("----- Party -----")]
    public int currSlot;
    public List<Pokemon> party = new List<Pokemon>(6);

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectStarter(Pokemon starter)
    {
        AddToParty(starter);
        SetActivePokemon(0);
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
        {
            GameManager.Instance.PlayerEXP.fillAmount = party[currSlot].exp / (float)Math.Pow(party[currSlot].level, 3);
        }
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
        GameManager.Instance.PlayerHP.fillAmount = (float)pokemon.currHP / pokemon.maxHP;
        GameManager.Instance.PlayerHPNum.text = pokemon.currHP.ToString() + "/" + pokemon.maxHP.ToString();
        GameManager.Instance.PlayerEXP.fillAmount = pokemon.exp / (float)Math.Pow(party[currSlot].level, 3);
        GameManager.Instance.PlayerLevel.text = "lv." + pokemon.level.ToString();
        return pokemon;
    }
    public void SetActivePokemon(int slot)
    {
        currSlot = slot;
        GameManager.Instance.PlayerSprite.sprite = Resources.Load<Sprite>("Pokemon/NormalBack/" + PokemonList.pokemonIDs[party[slot].dexID].ToLower());
        GameManager.Instance.PlayerSprite.enabled = true;
        GameManager.Instance.PlayerLevel.text = "lv." + party[slot].level.ToString();
        GameManager.Instance.PlayerName.text = PokemonList.pokemonNames[party[slot].dexID];
        GameManager.Instance.PlayerHP.fillAmount = (float)party[slot].currHP / party[slot].maxHP;
        GameManager.Instance.PlayerHPNum.text = party[slot].currHP.ToString() + "/" + party[slot].maxHP.ToString();
        GameManager.Instance.PlayerEXP.fillAmount = party[slot].exp / (float)Math.Pow(party[slot].level, 3);
        EnemyAI.Instance.CanDealDamage = true;
    }
    public void TakeDamage(int damage)
    {
        party[currSlot].currHP -= damage;
        GameManager.Instance.PlayerHP.fillAmount = (float)party[currSlot].currHP / party[currSlot].maxHP;
        GameManager.Instance.PlayerHPNum.text = party[currSlot].currHP.ToString() + "/" + party[currSlot].maxHP.ToString();
        if (party[currSlot].currHP <= 0)
        {
            EnemyAI.Instance.CanDealDamage = false;
            GameManager.Instance.PlayerSprite.enabled = false;
        }
    }
}
