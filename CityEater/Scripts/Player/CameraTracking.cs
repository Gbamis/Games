using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            Vector3 a = transform.position;
            Vector3 b = target.position;
            b += GameManager.Instance.gameData.cameraOffset;
            float lerpValue = GameManager.Instance.gameData.cameraFollowSpeed;
            transform.position = Vector3.Lerp(a, b, Time.deltaTime * lerpValue);
        }
    }
}
