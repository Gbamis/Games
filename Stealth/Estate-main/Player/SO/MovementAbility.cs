using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAbility : PlayerAbility
{
    [SerializeField] private Vector2 move;
    [SerializeField] private Vector3 dir;
    [SerializeField] private float moveWalue = 1;

    private Vector3 fw;
    private Vector3 rt;

    public override void OnStart(PlayerEvent p, PlayerContext c)
    {
        base.OnStart(p, c);
        Context.input.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        Context.input.Player.Move.canceled +=ctx => move = Vector2.zero;
        Context.input.Player.Run.performed += ctx =>
        {
            moveWalue = 2;
        };
        Context.input.Player.Run.canceled += ctx => moveWalue = 1;

        Context.input.Player.Crouch.started += ctx =>
        {
            bool c = Context.anim.GetBool("crouch");
            Context.anim.SetBool("crouch", !c);
        };
        Context.input.Player.Help.performed += ctx =>
        {
            GameEvent.Instance.GetHelp();
        };
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        fw = Context.mainCamTransform.forward;
        rt = Context.mainCamTransform.right;

        Move();
        Direct();
    }
    private void Move()
    {
        float x = Mathf.Abs(move.x);
        float y = Mathf.Abs(move.y);


        float max = Mathf.Max(x, y);
        if (move.x != 0 || move.y != 0)
        {
            Context.anim.SetFloat("forward", moveWalue);
        }
        else
        {
            Context.anim.SetFloat("forward", 0);
        }
        fw.y = 0;
        rt.y = 0;
        dir = (fw * move.y) + (rt * move.x);
        Context.controller.Move(dir * Time.deltaTime);

    }
    public void Direct()
    {
        fw.y = 0;
        rt.y = 0;
        fw.Normalize();
        rt.Normalize();
        var dis = fw * move.y + rt * move.x;
        Vector3 direction = Vector3.RotateTowards(Context.controller.transform.forward, dis, Context.lookSpeed * Time.deltaTime, 0.0f);
        Context.controller.transform.rotation = Quaternion.LookRotation(direction);
    }
}
