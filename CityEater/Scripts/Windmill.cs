using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class Windmill : MonoBehaviour
    {
        public Transform motor;

        private float rate;

        private void Start(){rate = Random.Range(10,50); }

        private void Update(){
            motor.Rotate(new Vector3(0,0,rate) * RealTime.deltaTime * GameManager.Instance.gameData.windmillSpeed);
        }
    }
}
