using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatAbility : PlayerAbility
{
    public float cooldown;
    private float comboCount;
    public float kickDamage;
    public float punchDamage;
    public float checkDistance;
    public Thug_AI currentThug;
    RaycastHit hit;

    public override void OnStart(PlayerEvent p, PlayerContext c)
    {
        base.OnStart(p, c);
        Context.input.Player.Combat.performed += ctx =>
        {
            comboCount += 1;
            Context.anim.SetTrigger("punch");
        };
        Context.input.Player.Combat.canceled += ctx =>
        {
            Context.anim.ResetTrigger("punch");
        };

        Context.input.Player.Knife.performed += ctx =>
        {
            Context.anim.SetTrigger("knife");
        };
        Context.input.Player.Knife.canceled += ctx =>
        {
            Context.anim.ResetTrigger("knife");
        };
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        UpdateCombo();
    }
    private void UpdateCombo()
    {
        if (comboCount > 0)
        {
            Context.anim.SetFloat("comboCount", comboCount);
            comboCount -= Time.deltaTime * cooldown;

        }
        CheckForThugRay();
    }
    private void CheckForThugRay()
    {
        Vector3 origin = Context.mid.position;
        Vector3 dir = Context.controller.transform.forward * checkDistance;

        if (Physics.Raycast(origin, dir, out hit, checkDistance))
        {
            if (hit.collider.CompareTag("Thug"))
            {
                currentThug = hit.collider.gameObject.GetComponent<Thug_AI>();
                Context.gamePadKeyUI[1].SetActive(true);
                Context.gamePadKeyUI[2].SetActive(true);
            }
        }
        else
        {
            currentThug = null;
            Context.gamePadKeyUI[1].SetActive(false);
            Context.gamePadKeyUI[2].SetActive(false);
        }

    }
    public void MakeAttack(string attack)
    {
        if (currentThug != null)
        {
            Context.controller.transform.LookAt(currentThug.transform);
            GameEvent.Instance.SetPunch();
            switch (attack)
            {
                case "punch":
                    Context.punchSFX.Play();
                    currentThug.TakeDamage("punch", punchDamage, Context.controller.transform, currentThug.transform);
                    break;
                case "kick":
                    Context.kickSFX.Play();
                    currentThug.TakeDamage("kick", kickDamage, Context.controller.transform, currentThug.transform);
                    break;
            }
        }
    }
}
