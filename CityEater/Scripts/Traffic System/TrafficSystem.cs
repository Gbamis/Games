using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    [System.Serializable]
    public struct Path
    {
        public string pathName;
        public List<Transform> waypoints;
    }

    public class TrafficSystem : MonoBehaviour
    {
        [SerializeField] private List<Vehicle> vehiclePrefab;
        [SerializeField] private List<Path> vehiclepaths = new List<Path>();
        [SerializeField] private Transform spawnedRoot;

        public void InitCapacity() { GameManager.Instance.gameData.vehicleTypeIndex = new int[10]; }
        
        public void InitLocalPool()
        {
            for (int index = 0; index < 10; index++)
            {
                Random.InitState(System.DateTime.Now.Millisecond + index);
                GameManager.Instance.gameData.vehicleTypeIndex[index] = Random.Range(0, 10);
            }
        }

        
        public void InitServerPool(int index, int value) { GameManager.Instance.gameData.vehicleTypeIndex[index] = value; }


        public void StartGame()
        {
            int vehicleIndex = 0;
            for (int index = 0; index < vehiclepaths.Count; index++)
            {
                int rand = GameManager.Instance.gameData.vehicleTypeIndex[vehicleIndex];
                Vehicle vehicle = Instantiate(vehiclePrefab[rand]);
                vehicle.Spawned(vehiclepaths[index]);
                vehicle.transform.SetParent(spawnedRoot);
                vehicleIndex++;

                if (vehicleIndex >= 10) { vehicleIndex = 0; }
            }
        }
    }
}
