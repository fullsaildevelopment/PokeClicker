using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy Instance;

    [Header("----- Stats -----")]
    [SerializeField] int skillShards;
    [SerializeField] int money;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        skillShards = 0;
        money = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateSkillShards(int amount)
    {
        skillShards += amount;
        GameManager.Instance.SkillShardText.text = "Skill Shards: " + skillShards.ToString();
    }
    public void UpdateMoney(int amount)
    {
        money += amount;
        GameManager.Instance.MoneyText.text = "Money: " + money.ToString();
    }
    public int GetMoney()
    {
        return money;
    }
    public int GetShards()
    {
        return skillShards;
    }
}
