using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    protected PlayerEvent Event;
    protected PlayerContext Context;

    public virtual void OnStart(PlayerEvent p, PlayerContext c){
        Event = p;
        Context = c;
    }

    public virtual void OnUpdate(){}
}
