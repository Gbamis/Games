using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class PlayerEngine : MonoBehaviour
{
    public PlayerContext Context;
    public List<PlayerAbility> Abilities;
    public PlayerEvent PlayerEvent;


    private void Awake()
    {
        Context.input = new PlayerInput();
        Context.anim = GetComponent<Animator>();
        Context.controller = GetComponent<CharacterController>();
        PlayerEvent = GetComponent<PlayerEvent>();
        Context.colliders = GetComponents<Collider>();
        Abilities = GetComponentsInChildren<PlayerAbility>().ToList();
    }
    private void Start()
    {
        foreach (PlayerAbility p in Abilities)
        {
            p.OnStart(PlayerEvent, Context);
        }
    }
    private void OnEnable()
    {
        Context.input.Player.Enable();
    }
    private void OnDisable()
    {
        Context.input.Player.Disable();
    }

    private void Update()
    {
        foreach (PlayerAbility p in Abilities)
        {
            p.OnUpdate();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PhoneBooth"))
        {
            CheckPointAbility cp = GetAbility<CheckPointAbility>();
            cp.Save(transform.position);
            DogSFX_Manager.Instance.CheckPoint();
            Context.checkPointUI.SetActive(true);
            StartCoroutine(Disable());
            Time.timeScale = 0.5f;
        }
        if (other.CompareTag("Bullet"))
        {
            HealthAbility hb = GetAbility<HealthAbility>();
            hb.TakeDamage();
        }
    }
    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(1);
        Context.checkPointUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayStep()
    {
        DogSFX_Manager.Instance.StepSFX();
    }

    public void MakeAttack(string attack)
    {
        CombatAbility cb = GetAbility<CombatAbility>();
        cb.MakeAttack(attack);
    }


    private T GetAbility<T>()
    {
        PlayerAbility def = null;
        foreach (PlayerAbility pa in Abilities)
        {
            if (pa.GetType() == typeof(T))
            {
                return (T)Convert.ChangeType(pa, typeof(T));
            }
        }
        return (T)Convert.ChangeType(def, typeof(T));
    }

}

[System.Serializable]
public class PlayerContext
{
    public PlayerInput input;
    public Animator anim;
    public CharacterController controller;
    public GameObject checkPointUI;
    public GameObject status;
    public Transform mainCamTransform;
    public Transform mid;
    public Image healthUI;
    public float checkDistance;
    public float lookSpeed;
    public List<GameObject> gamePadKeyUI;
    public Collider[] colliders;
    public AudioSource punchSFX, kickSFX;

}
