using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class Throat : MonoBehaviour
    {
        // public POINTS currentPoint = POINTS.TINY;

        private int swallowCount = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable")) { GameManager.Instance.sFXManager.PlaySwallow(); }
        }
    }

}