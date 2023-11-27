using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAbility : PlayerAbility
{
    public float health;
    public float recoveryTime;
    private float timmer;

    public override void OnStart(PlayerEvent p, PlayerContext c)
    {
        base.OnStart(p, c);
        health = 10;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Heal();
    }
    private void Heal()
    {
        if (health < 10)
        {
            timmer += Time.deltaTime * recoveryTime;
            if (timmer > 1)
            {
                health += 0.1f;
                Context.healthUI.fillAmount = health / 10;
                timmer = 0;
            }
        }
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health -= 0.8f;
            Context.healthUI.fillAmount = health / 10;
            return;
        }
        Context.status.SetActive(true);
        Context.anim.SetTrigger("die");
        Context.controller.enabled = false;
        foreach(Collider c in Context.colliders){
            c.enabled = false;
        }
        Event.Restart();
    }
}
