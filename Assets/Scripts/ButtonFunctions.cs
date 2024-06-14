using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    bool StartShakeBall;
    bool FinishShakeBall;
    int BallCurrentRotationZ;
    float deltaTime1;
    private void Start()
    {
        StartShakeBall = false;
        FinishShakeBall = false;
        BallCurrentRotationZ = 0;
        deltaTime1 = 0;
    }
    private void Update()
    {
        if (StartShakeBall)
        {
            deltaTime1 += Time.deltaTime;
            if (deltaTime1 >= 0.0333f)
            {
                deltaTime1 = 0;
                if (!FinishShakeBall)
                {
                    GameManager.Instance.ThrownBall.transform.Rotate(0, 0, 8);
                    BallCurrentRotationZ += 8;
                    if (BallCurrentRotationZ > 20)
                        FinishShakeBall = true;
                }
                else
                {
                    GameManager.Instance.ThrownBall.transform.Rotate(0, 0, -8);
                    BallCurrentRotationZ -= 8;
                    if (BallCurrentRotationZ < 1)
                    {
                        StartShakeBall = false;
                        FinishShakeBall = false;
                    }

                }
            }
        }
    }
    public void ThrowBall(int BallID)
    {
        BallType ball = PokemonList.BallTypes[BallID];
        GameManager.Instance.ThrowBall.interactable = false;
        GameManager.Instance.EnemySprite.enabled = false;
        GameManager.Instance.ThrownBall.enabled = true;

        Pokemon pokemon = ClickerButtonScript.Instance.enemy;
        
        //Shakes on Successes
        float CatchChance = Mathf.Floor((3 * pokemon.maxHP - 2 * pokemon.currHP) * 4096 + 0.5f);
        //CatchChance *= SpeciesRate;
        CatchChance *= ball.BaseCatchRate;
        //CatchChance *= StatusCondition;
        float ShakeChance = 65536 / (float)Math.Pow(((255 * 4096) / CatchChance), 0.1875);
        StartCoroutine(BallShakes(ShakeChance));
        
    }
    public void SetActivePartyPokemon(int slot)
    {
        if (Player.Instance.party[slot] != null)
        {
            Player.Instance.SetActivePokemon(slot);
        }
    }




    IEnumerator BallShakes(float ShakeChance)
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1);
            bool success = UnityEngine.Random.Range(0, 65536) < ShakeChance;
            if (!success)
            {
                //Break out
                GameManager.Instance.EnemySprite.enabled = true;
                GameManager.Instance.ThrownBall.enabled = false;
                GameManager.Instance.ThrowBall.interactable = true;
                yield break;
            }
            if (i != 3)
            {
                StartShakeBall = true;
            }

        }
        Player.Instance.AddToParty(new Pokemon(ClickerButtonScript.Instance.enemy));
        ClickerButtonScript.Instance.newPokemon();
        GameManager.Instance.EnemySprite.enabled = true;
        GameManager.Instance.ThrownBall.enabled = false;
        GameManager.Instance.ThrowBall.interactable = true;
    }
}
