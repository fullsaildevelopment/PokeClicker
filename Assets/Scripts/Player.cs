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
    public bool takingDamage;
    public bool gainingExp;
    public bool CanAttack;
    int txtHP;
    public bool CanAttackAnim;
    public bool startingAttackAnim;
    bool endingAttackAnim;
    bool isFainting;
    public int damageTaken;

    bool flashPlayerSuperEffective;
    bool flashPlayerNotVeryEffective;
    bool flashPlayerImmune;
    float deltaTime;

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
        CanAttackAnim = true;
        damageTaken = 0;

        flashPlayerImmune = false;
        flashPlayerSuperEffective = false;
        flashPlayerNotVeryEffective = false;
        deltaTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingDamage) //Will be set to true when the player takes damage
        { //This will lower the players HP bar smoothly
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
        //if (gainingExp) //idk
        //{
        //    GameManager.Instance.PlayerEXP.fillAmount += 1 * Time.deltaTime;
        //    if (GameManager.Instance.PlayerEXP.fillAmount >= party[currSlot].exp / (float)Math.Pow(party[currSlot].level, 3))
        //    {
        //        gainingExp = false;
        //        
        //    }
        //}
        if (startingAttackAnim) //attacking peak 40, 20 //normal location: 0, 0
        {
            CanAttackAnim = false;
            Vector3 playerLoc = GameManager.Instance.PlayerSprite.transform.localPosition;
            if (!endingAttackAnim)
            {
                
                playerLoc.x += Time.deltaTime * 400;
                playerLoc.y += Time.deltaTime * 200;
                if (playerLoc.x >= 40 &&  playerLoc.y >= 20)
                    endingAttackAnim = true;
                
            }
            else
            {
                playerLoc.x -= Time.deltaTime * 400;
                playerLoc.y -= Time.deltaTime * 200;
                if (playerLoc.x <= 0 && playerLoc.y <= 0)
                {
                    startingAttackAnim = false;
                    endingAttackAnim = false;
                    playerLoc = Vector3.zero;
                    StartCoroutine(AttackAnimCooldown());
                }
            }
            GameManager.Instance.PlayerSprite.transform.localPosition = playerLoc;
        }
        if (isFainting)
        {
            Vector3 loc = GameManager.Instance.PlayerSprite.transform.localPosition;
            loc.y -= Time.deltaTime * 3000;
            GameManager.Instance.PlayerSprite.transform.localPosition = loc;
            if (loc.y <= -300)
            {
                isFainting = false;
                GameManager.Instance.PlayerSprite.enabled = false;
                loc.y = 0;
                GameManager.Instance.PlayerSprite.transform.localPosition = loc;
            }
        }

        if (flashPlayerSuperEffective)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.PlayerSuperEffectiveText.transform.localScale = Vector3.one;
                flashPlayerSuperEffective = false;
            }
        }
        if (flashPlayerNotVeryEffective)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.PlayerNotVeryEffectiveText.transform.localScale = Vector3.one;
                flashPlayerNotVeryEffective = false;
            }
        }
        if (flashPlayerImmune)
        {
            if (deltaTime > 0.2f)
            {
                GameManager.Instance.PlayerImmuneText.transform.localScale = Vector3.one;
                flashPlayerImmune = false;
            }
        }
        if (GameManager.Instance.PlayerNotVeryEffectiveText.gameObject.activeSelf ||
            GameManager.Instance.PlayerSuperEffectiveText.gameObject.activeSelf ||
            GameManager.Instance.PlayerImmuneText.gameObject.activeSelf)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime > 1.5f)
            {
                GameManager.Instance.PlayerSuperEffectiveText.gameObject.SetActive(false);
                GameManager.Instance.PlayerNotVeryEffectiveText.gameObject.SetActive(false);
                GameManager.Instance.PlayerImmuneText.gameObject.SetActive(false);
            }
        }
    }
    public void SelectStarter(Pokemon starter) //Called from the button functuons script when selecting a starter
    {
        AddToParty(starter);
        SetActivePokemon(0);
        CanAttack = true;
    }
    public void AddToParty(Pokemon pokemon) //Called when you catch a Pokemon or get your starter
    {
        if (party.Count < party.Capacity)
        {
            party.Add(pokemon);
            GameManager.Instance.PartySlots[party.Count - 1].interactable = true;
            GameManager.Instance.UpdatePartyButton(party.Count - 1, pokemon);
        }
    }
    public void AddEXP(int exp, int slot) //Called when the player defeats an enemy
    {
        party[slot].exp += exp;
        if (slot == currSlot)
            GameManager.Instance.PlayerEXP.fillAmount = party[currSlot].exp / (float)Math.Pow(party[currSlot].level, 3);
        CanLevelUp(party[slot], slot);
    }
    void CanLevelUp(Pokemon pokemon, int slot) //checks if the pokemon can level up
    {
        while (pokemon.exp >= Math.Pow(pokemon.level, 3))
        {
            pokemon = LevelUp(pokemon);
            party[slot] = pokemon;
        }
        
    }
    Pokemon LevelUp(Pokemon pokemon) //Levels the pokemon up by 1.
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
        if (pokemon.exp < (int)Math.Pow(pokemon.level, 3))
        {
            for (int i = 0; i < pokemon.evolveLevels.Count; i++) //If the Pokemon can evolve after its done leveling up
            {
                if (pokemon.level >= pokemon.evolveLevels[i] && pokemon.evolveMethods[i] == EvolveMethod.LevelUp)
                {
                    EvolvePokemon(pokemon, pokemon.evoDexIDs[i]); //Will force evolution without player input for now
                    break;
                }
            }
        }
        GameManager.Instance.UpdatePartyButton(currSlot, pokemon); //Update the pokemon in the party button data
        return pokemon;
    }
    void EvolvePokemon(Pokemon pokemon, int evoDexID) //Called from level up, other methods not yet implemented
    {
        Pokemon evolvedForm = null;
        if (pokemon.reginalForm == RegionalForm.None)
        {
            if (isOtherStarterPokemon(pokemon.dexID)) //temporarly used until all starter Pokemon are moved to the regular list
            {
                foreach (Pokemon OtherPokemon in PokemonList.OtherStarters)
                {
                    if (OtherPokemon.dexID == evoDexID && OtherPokemon.reginalForm == RegionalForm.None)
                    {
                        evolvedForm = OtherPokemon;
                        break;
                    }
                }
            }
            else
                evolvedForm = PokemonList.PokemonData[evoDexID];
        } 
        else
        {
            if (isOtherStarterPokemon(pokemon.dexID))
            {
                foreach (Pokemon OtherPokemon in PokemonList.OtherStarters)
                {
                    if (OtherPokemon.dexID == evoDexID && OtherPokemon.reginalForm == pokemon.reginalForm)
                    {
                        evolvedForm = OtherPokemon;
                        break;
                    }
                }
            }
            else
            {
                foreach (Pokemon regionalPokemon in PokemonList.RegionalPokemonData)
                {
                    if (regionalPokemon.dexID == evoDexID && pokemon.reginalForm == regionalPokemon.reginalForm)
                    {
                        evolvedForm = regionalPokemon;
                        break;
                    }
                }
                if (evolvedForm == null) //can't find regional form. Using default.
                {
                    if (isOtherStarterPokemon(pokemon.dexID))
                    {
                        foreach (Pokemon OtherPokemon in PokemonList.OtherStarters)
                        {
                            if (OtherPokemon.dexID == evoDexID && OtherPokemon.reginalForm == pokemon.reginalForm)
                            {
                                evolvedForm = OtherPokemon;
                                break;
                            }
                        }
                    }
                    else
                        evolvedForm = PokemonList.PokemonData[evoDexID];
                }
                    
            }
        }

        pokemon.dexID = evolvedForm.dexID;
        pokemon.evolveLevels.Clear();
        pokemon.evolveMethods.Clear();
        pokemon.evoDexIDs.Clear();
        pokemon.evolveLevels.AddRange(evolvedForm.evolveLevels);
        pokemon.evolveMethods.AddRange(evolvedForm.evolveMethods);
        pokemon.evoDexIDs.AddRange(evolvedForm.evoDexIDs);
        pokemon.type1 = evolvedForm.type1;
        pokemon.type2 = evolvedForm.type2;
        pokemon.staticForm = evolvedForm.staticForm;
        GameManager.Instance.PlayerSprite.sprite = Resources.Load<Sprite>("Pokemon/NormalBack/" + PokemonList.pokemonIDs[pokemon.dexID].ToLower());
        GameManager.Instance.PlayerName.text = PokemonList.pokemonNames[pokemon.dexID];
    }
    public void SetActivePokemon(int slot) //Usually called from clicking a party slot button to switch out Pokemon
    {
        ResetFaintAnim(); //Don't want a non-fainted pokemon to appear fainted
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
        GameManager.Instance.ThrowBall.interactable = true;
    }
    public void TakeDamage(int damage) //Called when the enemy attacks from EnemyAI script. Damage math is in the EnemyAI script
    {
        party[currSlot].currHP -= damage;
        if (party[currSlot].currHP < 0) 
            party[currSlot].currHP = 0;
        takingDamage = true;
        damageTaken = damage;
        Instantiate(GameManager.Instance.DamageCounter, GameManager.Instance.PlayerSprite.transform.parent);
        if (party[currSlot].currHP <= 0)
        {
            EnemyAI.Instance.PauseAttack(true);
            isFainting = true;
            GameManager.Instance.ThrowBall.interactable = false;
            CanAttack = false;
            if (PartyWipe())
            {
                GameManager.Instance.GameOverScreen.SetActive(true);
            }
        }
        GameManager.Instance.UpdatePartyButton(currSlot, party[currSlot]);
    }
    bool PartyWipe() //Your entire party of Pokemon are fainted
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
    bool isOtherStarterPokemon(int dexID) //used for starters in generations not yet implemented
    {
        foreach (Pokemon pokemon in PokemonList.OtherStarters)
        {
            if (pokemon.dexID == dexID)
                return true;
        }
        return false;
    }
    IEnumerator AttackAnimCooldown() //while clicking to attack has cooldown, spamming an animation doesn't look good.
    {
        yield return new WaitForSeconds(0.3f);
        CanAttackAnim = true;
    }
    void ResetFaintAnim() //Called after a Pokemon faints or if the player switches to a new Pokemon during the animation
    {
        isFainting = false;
        GameManager.Instance.PlayerSprite.transform.localPosition = Vector3.zero;
    }
    public void FlashPlayerEffectiveness(float multiplier)
    {
        if (multiplier > 1)
        {
            GameManager.Instance.PlayerSuperEffectiveText.gameObject.SetActive(true);
            GameManager.Instance.PlayerNotVeryEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.PlayerImmuneText.gameObject.SetActive(false);
            if (!flashPlayerSuperEffective)
            {
                GameManager.Instance.PlayerSuperEffectiveText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashPlayerSuperEffective = true;
            }
        }
        else if (multiplier == 0.1f)
        {
            GameManager.Instance.PlayerSuperEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.PlayerNotVeryEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.PlayerImmuneText.gameObject.SetActive(true);
            if (!flashPlayerImmune)
            {
                GameManager.Instance.PlayerImmuneText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashPlayerImmune = true;
            }
        }
        else if (multiplier < 1)
        {
            GameManager.Instance.PlayerSuperEffectiveText.gameObject.SetActive(false);
            GameManager.Instance.PlayerNotVeryEffectiveText.gameObject.SetActive(true);
            GameManager.Instance.PlayerImmuneText.gameObject.SetActive(false);
            if (!flashPlayerNotVeryEffective)
            {
                GameManager.Instance.PlayerNotVeryEffectiveText.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                deltaTime = 0;
                flashPlayerNotVeryEffective = true;
            }
        }
    }
}
