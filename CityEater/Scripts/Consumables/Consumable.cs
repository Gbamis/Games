using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]

    public class Consumable : MonoBehaviour
    {
        public POINTS point;
        private Rigidbody rb;
        public bool isNegative = false;


        private void Awake() { rb = GetComponent<Rigidbody>(); }

        public void Consume()
        {
            if (isNegative) { GameManager.Instance.gameData.playerScore -= (int)point * 10; }
            else { GameManager.Instance.gameData.playerScore += (int)point * 10; }

            GameManager.Instance.sFXManager.PlayScoreGained();
            GameManager.Instance.uiManager.Pop((int)point * 10, isNegative);
        }

        public void WakeUp()
        {
            if (rb.IsSleeping())
            {
                rb.WakeUp();
                rb.AddForce(Vector3.down * GameManager.Instance.gameData.swallowForce);
            }
        }

    }
}
