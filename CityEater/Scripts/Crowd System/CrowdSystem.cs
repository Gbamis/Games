using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class CrowdSystem : MonoBehaviour
    {
        [SerializeField] private Person[] peoplePrefabs;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform spawnedPeople;

        public void InitCapacity() { GameManager.Instance.gameData.randomPeopleTypeIndex = new int[6]; }

        public void InitLocalPool()
        {
            for (int i = 0; i < 4; i++)
            {
                Random.InitState(System.DateTime.Now.Millisecond + i);
                GameManager.Instance.gameData.randomPeopleTypeIndex[i] = Random.Range(0, 6);
            }
        }

        public void InitServerPool(int index, int value) { GameManager.Instance.gameData.randomPeopleTypeIndex[index] = value; }

        public void StartGame() { SpawnPeople(); }

        private void SpawnPeople()
        {
            int personIndex = 0;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                int index = GameManager.Instance.gameData.randomPeopleTypeIndex[personIndex];
                Person person = Instantiate(peoplePrefabs[index]);
                person.Spawned(spawnPoints[i].position, spawnPoints[i].rotation);
                person.transform.parent = spawnedPeople;

                if (personIndex >= peoplePrefabs.Length) { personIndex = 0; }
            }
        }
    }
}
