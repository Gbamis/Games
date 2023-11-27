using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public enum POINTS { TINY = 1, SMALL = 2, MEDIUM = 3, LARGE = 5 }

    public class CityManager : MonoBehaviour
    {
        private int[] rotations = new int[] { 0, 90, 180, 270 };

        public Transform[] playerSpawnPoints;

        public void InitLocalPool()
        {
            GameManager.Instance.gameData.cityRotationIndex = Random.Range(0, 4);
            GameManager.Instance.gameData.playerSpawnIndex = Random.Range(0, 6);
            GameManager.Instance.gameData.randThemeIndex = Random.Range(0, 3);
        }

        public void InitServerPool(int rotIndex, int playerSpawn, int randMusic){
            GameManager.Instance.gameData.cityRotationIndex = rotIndex;
            GameManager.Instance.gameData.playerSpawnIndex = playerSpawn;
            GameManager.Instance.gameData.randThemeIndex = randMusic;
        }

        public void StartGame()
        {
            Physics.gravity = new Vector3(0, -15, 0);
            GameManager.Instance.sFXManager.PlayerTheme(4, GameManager.Instance.gameData.randThemeIndex);
            transform.Rotate(Vector3.up * rotations[GameManager.Instance.gameData.cityRotationIndex]);
            int index = GameManager.Instance.gameData.playerSpawnIndex;
            GameManager.Instance.playerBlackHole.transform.position = playerSpawnPoints[index].position;
        }

    }
}
