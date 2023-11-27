using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushAbility", menuName = "Player/Ability/PushAbility")]
public class PushAbility : PlayerAbility
{

    [SerializeField] private bool canPush;
    [SerializeField] private bool isPushing;
    [SerializeField] private float pushForceDef;
    private Rigidbody pushObject;
    private RaycastHit hit;

    public override void OnStart(PlayerEvent p, PlayerContext c)
    {
        base.OnStart(p, c);
        Context.input.Player.Push.performed += ctx =>
        {

            if (canPush)
            {
                Context.anim.SetBool("push", true);
                Context.controller.radius = 0.89f;
                Context.lookSpeed = 1;
                isPushing = true;
            }

        };
        Context.input.Player.Push.canceled += ctx =>
        {
            Context.anim.SetBool("push", false);
            Context.controller.radius = 0.4f;
            Context.lookSpeed = 5;
            isPushing = false;
        };
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Push();
        //CheckForThugRay();
    }
    private void Push()
    {
        if (pushObject != null)
        {
            if (isPushing)
            {
                Vector3 push = Context.controller.transform.forward / pushForceDef;
                pushObject.AddForce(push);
            }
            else
            {
                pushObject.velocity = Vector3.zero;

            }
        }

    }
    private void CheckForThugRay()
    {
        Vector3 origin = Context.mid.position;
        Vector3 dir = Context.controller.transform.forward * Context.checkDistance;
        Debug.DrawRay(origin, dir, Color.cyan);
        if (Physics.Raycast(origin, dir, out hit, Context.checkDistance))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                canPush = true;
                pushObject = hit.collider.GetComponent<Rigidbody>();
                Context.gamePadKeyUI[0].SetActive(true);
            }
            else
            {
                canPush = false;
                pushObject = null;
                Context.gamePadKeyUI[0].SetActive(false);
            }
        }
        else
        {
            canPush = false;
            pushObject = null;
            Context.gamePadKeyUI[0].SetActive(false);
        }
    }

}
