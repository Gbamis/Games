using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AI.Trees;

public class GameEvent : MonoBehaviour
{
    private static GameEvent m_instance;
    public TextMeshProUGUI punchesText;
    private int hits;
    [SerializeField] public Sequence sequence;
    public GameObject helpUI;
    private bool show;

    public static GameEvent Instance
    {
        set
        {
            m_instance = value;
        }
        get
        {
            return m_instance;
        }
    }

    public delegate void EnemyHit(Transform e);
    public event EnemyHit OnEnemyHit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        show = true;
        helpUI.SetActive(show);
    }


    public void GetHelp(){
        show = !show;
        helpUI.SetActive(show);
    }
    public void HitEnemy(Transform e)
    {
        OnEnemyHit(e);

    }

    public void SetPunch()
    {
        hits += 1;
        punchesText.text = hits.ToString();
    }
}
