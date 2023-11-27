using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

namespace Duelit.Hole
{
    public class CameraOccludee : MonoBehaviour
    {
        public float rayLength;
        private Collider lastCollider;
        private List<Collider> colliders = new List<Collider>();
        public Material transparentTemplate;

        private void Update()
        {
            CastRayToCam();
        }

        private void CastRayToCam()
        {
            RaycastHit hit;
            Vector3 origin = GameManager.Instance.playerBlackHole.transform.position;
            Vector3 dir = (transform.position - GameManager.Instance.playerBlackHole.transform.position) * rayLength;
            Debug.DrawRay(origin, dir, Color.red);

            if (Physics.Raycast(origin, dir, out hit))
            {
                if (hit.collider != null)
                {
                    Material hitMaterial = hit.collider.GetComponent<MeshRenderer>().material;
                    
                    Material standard = new Material(transparentTemplate);
                    standard.mainTexture = hitMaterial.mainTexture;
                    
                    Color color = standard.color;
                    color.a = 0;

                    hit.collider.GetComponent<MeshRenderer>().material = standard;

                    lastCollider = hit.collider;
                    colliders.Add(hit.collider);
                }
                else
                {
                    
                    lastCollider.GetComponent<MeshRenderer>().material.shader = Shader.Find("BlackHole/Fallthrough Texture");
                }
            }
            else
            {
                if (colliders.Count > 0)
                {
                    foreach (Collider col in colliders)
                    {
                        col.GetComponent<MeshRenderer>().material.shader = Shader.Find("BlackHole/Fallthrough Texture");
                    }
                    colliders.Clear();

                }
            }
        }

    }
}
