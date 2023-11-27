using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    [CreateAssetMenu(fileName = "HoleIO_data", menuName = "Duelit/HoleIO/GameData")]
    public class GameData : ScriptableObject
    {
        [Header("Player Configuration")]
        public float playerSpeed;
        public float playerBoostSpeed;
        public float playerBoostSpeedTimeLimit;
        public float playerScore;
        public float swallowForce;
        public float totalPlayTime;
        public float windmillSpeed;


        [HideInInspector] public float elapsedTime;
        public float playerTurnSpeed;

        [Header("Camera Configurations")]
        public float cameraFollowSpeed;
        public Vector3 cameraOffset;

        [Header("Pool Data")]
        public int cityRotationIndex;
        public int playerSpawnIndex;
        public int randThemeIndex;
        public int randDroneSpawnPoint;
        public float randomDroneStartTime;
        public float randomDronRunTime;
        public int[] vehicleTypeIndex;
        public int[] randomPeopleTypeIndex;


        public void ResetValues()
        {
            playerScore = 0;
            playerSpeed = 4;
            elapsedTime = 60;

            cityRotationIndex = playerSpawnIndex = randThemeIndex = randDroneSpawnPoint = -1;
            randomDroneStartTime = randomDronRunTime = -1;
            vehicleTypeIndex = new int[] { };
            randomPeopleTypeIndex = new int[] { };
        }
    }

}
/*Randomess
{
	"version": "1.001",
	"game_id": "hollow",
	"level": {
        "cityRotationIndex": [<+0 to +3> (...1 x)], (0 decimal)
        "playerSpawnIndex": [<+0 to +5> (...1 x)], (0 decimal)
        "randThemeIndex: [<+0 to +2> (...1 x)], (0 decimal)
        "vehicleTypeIndex" : [<+0 to +9> (...10 x)], (0 decimal)
        "randomPeopleTypeIndex": [<+0 to +5> (...6 x)], (0 decimal)
        "randDroneSpawnPoint": [<+0 to +3> (...1 x)], (0 decimal)
        "randomDroneStartTime" : [<+15.0 to +20.0> (...1 x)], (1 decimal)
        "randomDroneRunTime" : [<+15.0 to +20.0> (...1 x)], (1 decimal)
	},
}
Config
{
    Beginner - 20 - 400
    Intermediate - 400 - 1700
    Expert - 1700 - 2300
    Increments [-50,-40,-10,50,100,500,800,1200]
}
Upper Limit
{
    'name': 'Hollow',
    'gameId': 'hollow',
    'limit': 2500
  }

Description
{
    'Hollow': 'Eat up the whole city'
}
*/
