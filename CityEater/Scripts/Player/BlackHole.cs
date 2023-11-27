using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class BlackHole : MonoBehaviour
    {
        private Vector3 fw;
        private Vector3 rt;
        private Vector2 move;
        private Vector3 dir;

        public GameObject model;
        public FloatingJoystick floatingJoystick;
        public Collider groundCollider;
        public BoxCollider sizeCheckerCollider;

        public Vector2 minClamp;
        public Vector2 maxClamp;


        private void Update()
        {
            fw = GameManager.Instance.mainCamera.transform.forward;
            rt = GameManager.Instance.mainCamera.transform.right;

            if (GameManager.Instance.isGameRunning)
            {
                float horizontal = floatingJoystick.Horizontal;
                float vertical = floatingJoystick.Vertical;
                move = new Vector2(horizontal, vertical);
                fw.y = 0;
                rt.y = 0;
                dir = (fw * move.y) + (rt * move.x);
                transform.Translate(dir * GameManager.Instance.gameData.playerSpeed * Time.deltaTime);
            }
            Direct();
            ClampToCity();
        }
        private void Direct()
        {
            fw.y = 0;
            rt.y = 0;
            fw.Normalize();
            rt.Normalize();
            var dis = fw * move.y + rt * move.x;
            Vector3 direction = Vector3.RotateTowards(model.transform.forward, dis, GameManager.Instance.gameData.playerTurnSpeed * Time.deltaTime, 0.0f);
            model.transform.rotation = Quaternion.LookRotation(direction);
        }
        private void ClampToCity()
        {
            float xC = transform.position.x;
            float zC = transform.position.z;

            xC = Mathf.Clamp(xC, minClamp.x, maxClamp.x);
            zC = Mathf.Clamp(zC, minClamp.y, maxClamp.y);

            Vector3 pos = new Vector3(xC, transform.position.y, zC);
            transform.position = pos;
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 otherSize = other.bounds.size;
            Vector3 selfSize = sizeCheckerCollider.bounds.size;

            float m_size = selfSize.x + selfSize.z;
            float o_size = otherSize.x + otherSize.z;

            if (other.CompareTag("Collectable") || other.CustomIsTag("Vehicle"))
            {
                if (m_size > o_size) { Physics.IgnoreCollision(groundCollider, other, true); }
            }

            if (other.CustomIsTag("Bomb"))
            {
                GameManager.Instance.droneSystem.TakeDamge();

                /*ParticleSystem efx = Instantiate(GameManager.Instance.droneSystem.explosionFX);
                efx.transform.position = transform.position;
                efx.gameObject.SetActive(true);
                efx.Play();*/
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Vector3 otherSize = other.bounds.size;
            Vector3 selfSize = sizeCheckerCollider.bounds.size;

            float m_size = selfSize.x + selfSize.z;
            float o_size = otherSize.x + otherSize.z;

            if (other.CompareTag("Collectable") || other.CustomIsTag("Vehicle"))
            {
                Consumable consumable = other.GetComponent<Consumable>();
                if (m_size > o_size) { Physics.IgnoreCollision(groundCollider, other, true); consumable.WakeUp(); }
            }
        }

        private void OnTriggerExit(Collider other) { Physics.IgnoreCollision(groundCollider, other, false); }
    }
}
