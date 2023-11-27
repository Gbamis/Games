using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Duelit.Hole
{
    [System.Serializable]
    public struct BombTracking
    {
        public float rayLength;
        [Range(0, 1)] public float bombTrackAccuracy;
        public Transform rayOrigin;
        public float bombRate;
    }

    public class DroneSystem : MonoBehaviour
    {
        private bool isTrackingPlayer;
        private bool isCloseToPlayer;
        private bool canSeePlayer;

        public GameObject drone;
        public GameObject droneTimer;
        public Image droneTimeLevel;

        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float distanceChecker;
        [SerializeField] private float shootChecker;
        [SerializeField] private float followSpeed;
        [SerializeField] private float spawnHieight;
        [SerializeField] private int takeDamge;
        
        [SerializeField] private float bombForce;

        [SerializeField] private List<Transform> blades;
        [SerializeField] private float bladeSpeed;
        [SerializeField] private List<Transform> spawnPoints;

        private float nextFire = 0;
        private int randIndex;
        private float droneRunning;
        private float shootCount = 0;

        public BombTracking bombTracking;

        public void InitLocalPool()
        {
            shootCount = 0;
            randIndex = GameManager.Instance.gameData.randDroneSpawnPoint = Random.Range(0, 4);
            GameManager.Instance.gameData.randomDroneStartTime = Random.Range(15, 20);
            GameManager.Instance.gameData.randomDronRunTime = Random.Range(15, 20);
        }

        public void InitServerPool(int droneSpawn, float droneSart, float droneRun)
        {
            shootCount = 0;
            randIndex = GameManager.Instance.gameData.randDroneSpawnPoint = droneSpawn;
            GameManager.Instance.gameData.randomDroneStartTime = droneSart;
            GameManager.Instance.gameData.randomDronRunTime = droneRun;
        }

        public void StartGame()
        {
            StartCoroutine(TrackPlayer());
            droneRunning = GameManager.Instance.gameData.randomDronRunTime;
            droneTimeLevel.fillAmount = droneRunning / GameManager.Instance.gameData.randomDronRunTime;
        }

        private void Update()
        {
            Fire(); CastRay();

        }

        private IEnumerator TrackPlayer()
        {
            yield return new WaitForSeconds(GameManager.Instance.gameData.randomDroneStartTime);
            ActivateDrone(true);
            StartCoroutine(FollowPlayer());
            yield return new WaitUntil(() => (shootCount > GameManager.Instance.gameData.randomDronRunTime));
            ActivateDrone(false);
        }

        private IEnumerator FollowPlayer()
        {
            float passTime = 0;
            drone.transform.position = spawnPoints[randIndex].position;
            while (isTrackingPlayer)
            {
                Vector3 pos = drone.transform.position;
                Vector3 player = GameManager.Instance.playerBlackHole.transform.position;

                float distance = Vector3.Distance(pos, player);
                if (distance > shootChecker) { isCloseToPlayer = false; }
                else { isCloseToPlayer = true; }

                if (distance > distanceChecker)
                {
                    Vector3 des = Vector3.Lerp(pos, player, Time.deltaTime * followSpeed);
                    des.y = spawnHieight;
                    drone.transform.position = des;
                }
                Vector3 dir = GameManager.Instance.playerBlackHole.transform.position;
                dir.y = drone.transform.position.y;
                drone.transform.LookAt(dir);

                passTime += Time.deltaTime;

                droneRunning -= Time.deltaTime;

                yield return null;
            }
        }

        private void Fire()
        {
            if (Time.time > nextFire && isCloseToPlayer && drone.activeSelf && canSeePlayer)
            {
                GameObject bomb = Instantiate(bombPrefab);
                bomb.transform.SetParent(transform);
                bomb.transform.position = drone.transform.position;
                bomb.SetActive(true);

                StartCoroutine(MoveBomb(bomb));
                //MoveBombForce(bomb);

                GameManager.Instance.sFXManager.PlayMissileFired();

                nextFire = Time.time + bombTracking.bombRate;
                shootCount++;
                float value = (GameManager.Instance.gameData.randomDronRunTime - shootCount) / GameManager.Instance.gameData.randomDronRunTime;
                droneTimeLevel.fillAmount = value;

            }
            foreach (Transform child in blades)
            {
                child.Rotate(Vector3.up * bladeSpeed);
            }

        }

        private IEnumerator MoveBomb(GameObject bomb)
        {
            float time = 0;
            Vector3 pos = drone.transform.position;
            Vector3 endPos = GameManager.Instance.playerBlackHole.transform.position;

            while (time < bombForce)
            {
                float step = time / bombForce;
                if (step < bombTracking.bombTrackAccuracy) { endPos = GameManager.Instance.playerBlackHole.transform.position; }
                Vector3 des = Vector3.Lerp(pos, endPos, step);
                bomb.transform.position = des;
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(bomb, 1.5f);
        }

        private void MoveBombForce(GameObject bomb)
        {
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(drone.transform.forward * bombForce);
            Destroy(bomb, 1.5f);
        }

        private void CastRay()
        {
            RaycastHit hit;
            Vector3 origin = bombTracking.rayOrigin.position;
            Vector3 dir = (GameManager.Instance.playerBlackHole.transform.position - bombTracking.rayOrigin.position) * bombTracking.rayLength;
            if (Physics.Raycast(origin, dir, out hit))
            {
                if (hit.collider.CompareTag("Player")) { canSeePlayer = true; }
                else { canSeePlayer = false; }
            }
            else { canSeePlayer = false; }

            Debug.DrawRay(origin, dir, Color.red);
        }

        private void ActivateDrone(bool value)
        {
            isTrackingPlayer = value; drone.SetActive(value);
            GameManager.Instance.sFXManager.PlayPoliceSiren(value);
            GameManager.Instance.sFXManager.PlayDroneFlying(value);
            droneTimer.SetActive(value);

            if (value) { GameManager.Instance.gameData.playerSpeed += .2f; }
            else { GameManager.Instance.gameData.playerSpeed -= .2f; }

        }

        public void TakeDamge()
        {
            GameManager.Instance.gameData.playerScore -= takeDamge * 10;
            GameManager.Instance.uiManager.Pop(takeDamge * 10, true);
            GameManager.Instance.scoreDetector.DecreasePlayer();
        }
    }
}
