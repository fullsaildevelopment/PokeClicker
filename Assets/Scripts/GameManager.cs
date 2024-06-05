using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("----- Enemy -----")]
    public Image EnemyHP;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        EnemyHP.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
