using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class Tile : MonoBehaviour
    {
        public Transform buildingRoot;
        public List<Transform> spawnPoints;


        private void Start(){
        //Spawn();
        }

        public void Spawn(){
            /*for(int i =0; i < spawnPoints.Count; i++){
                int rand = Random.Range(0,4);
                GameObject house = Instantiate(GameManager.Instance.cityManager.buildings[rand]);
                house.SetActive(true);
                house.transform.position = spawnPoints[i].position;
                house.transform.parent = buildingRoot;
            }*/
        }
    }
}
