using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class Vehicle : MonoBehaviour
    {
        private Path path;
        private int index = 0;

        private bool canMove = true;
        [SerializeField] private float sensorLength;
        [SerializeField] private float vehicleSpeed;
        [SerializeField] private float vehicleTurnSpeed;
        [SerializeField] private float vehicleNextCheck;

        private bool m_HitDetect;
        private bool allowSense = true;

        [SerializeField] private Collider m_Collider;
        private RaycastHit m_Hit;


        public void Spawned(Path paths)
        {
            path = paths;
            transform.position = paths.waypoints[0].position;
            transform.rotation = paths.waypoints[0].rotation;
            gameObject.SetActive(true);
            m_Collider = transform.GetChild(0).GetComponent<BoxCollider>();
            allowSense = true;

            StartCoroutine(MoveToNextWayPoint());
        }

        private IEnumerator MoveToNextWayPoint()
        {
            Vector3 des = path.waypoints[index].position;
            Quaternion rot = path.waypoints[index].rotation;

            while (GameManager.Instance.isGameRunning)
            {
                vehicleTurnSpeed = Vector3.Angle(transform.forward, path.waypoints[index].forward);
                float dis = Vector3.Distance(des, transform.position);

                transform.position = Vector3.MoveTowards(transform.position, des, Time.deltaTime * vehicleSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * vehicleTurnSpeed * dis);

                if (dis < vehicleNextCheck)
                {
                    index++;
                    if (index >= path.waypoints.Count) { index = 0; }
                    des = path.waypoints[index].position;
                    rot = path.waypoints[index].rotation;
                }
                yield return new WaitUntil(() => CanMove());
            }
        }

        private bool CanMove()
        {
            bool value = canMove && !m_HitDetect;
            if (m_HitDetect)
            {
                GameManager.Instance.sFXManager.PlayCarHonk();
            }
            return value;
        }

        private void FixedUpdate()
        {
            if (gameObject.activeSelf && m_Collider != null)
            {
                Physics.BoxCast(m_Collider.bounds.center, transform.localScale / 2.5f, transform.forward, out m_Hit, transform.rotation, sensorLength);
                if (m_Hit.collider.CustomIsTag("vehicle"))
                {
                    m_HitDetect = true;
                    vehicleTurnSpeed *= -10;
                }
                else { m_HitDetect = false; vehicleTurnSpeed *= 1; }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (m_HitDetect)
            {
                Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
                Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale / 2.5f);
            }
            else
            {
                Gizmos.DrawRay(transform.position, transform.forward * sensorLength);
                Gizmos.DrawWireCube(transform.position + transform.forward * sensorLength, transform.localScale / 2.5f);
            }
        }
    }
}
