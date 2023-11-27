
using UnityEngine;

namespace Duelit.Hole
{
    public class Person : MonoBehaviour
    {
        private Animator anim;
        public void Spawned(Vector3 pos, Quaternion rot)
        {
            transform.position = pos;
            transform.rotation = rot;
            gameObject.SetActive(true);
            anim = GetComponent<Animator>();
            InvokeRepeating("ChangeAnimation", 1, 3);
        }

        private void ChangeAnimation() { int rand = Random.Range(0, 4); anim.SetInteger("index", rand); }
    }
}
