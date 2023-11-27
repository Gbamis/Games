using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class Optimize : MonoBehaviour
    {
        private Animator anim;

        private void Start() { anim = GetComponentInParent<Animator>(); }

        private void OnBecameVisible() { anim.enabled = true; }

        private void OnBecameInvisible() { anim.enabled = false; }
    }
}
