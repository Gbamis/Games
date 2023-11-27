using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Proyecto26;
using System.Globalization;

namespace Duelit.Hole
{
    public class GameManager : MonoBehaviour
    {
        [Header("Online tournament")]
        public bool isTest = false;
        private string basePath;
        private RequestHelper currentRequest;
        private JsonData Data;
        private string gameID;
        private string tournamentID;
        private string updateTime;
        private string isDebug;
        private string token;
        private int seed = 1;
        public GameObject backButton;
        public GameObject loading;

        public static GameManager Instance { set; get; }
        public GameData gameData;

        [Header("Managers/Systems")]
        public CityManager cityManager;
        public TrafficSystem trafficSystem;
        public SFXManager sFXManager;
        public CrowdSystem crowdSystem;
        public DroneSystem droneSystem;
        public UIManager uiManager;
        public ScoreDetector scoreDetector;

        public BlackHole playerBlackHole;
        public Camera mainCamera;

        public bool isGameRunning;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            gameData.ResetValues();

            if (isTest)
            {
                TestAPIs testAPIs = new TestAPIs();
                tournamentID = PlayerPrefs.GetString("tournamentID");
            }
            else
            {
                tournamentID = PlayerPrefs.GetString("tournamentID");
            }

            if (tournamentID != "1")
            {
                // backButton.SetActive(false);
                SetCapacity();
                DownloadInfo();
                StartCoroutine(UpdateTotalPoints());
            }
            else
            {
                LoadLocal();
            }
        }

        private void Start()
        {
            //LoadLocal();
            ForfeitController.instance.forfeitButton.onClick.AddListener(() => StartCoroutine(StartCountDownEndGame()));
        }

        private void LoadLocal()
        {
            SetCapacity();

            cityManager.InitLocalPool();
            trafficSystem.InitLocalPool();
            droneSystem.InitLocalPool();
            crowdSystem.InitLocalPool();

            StartGame();
        }

        private void SetCapacity()
        {
            trafficSystem.InitCapacity();
            crowdSystem.InitCapacity();
        }

        public void DownloadInfo()
        {

            basePath = PlayerPrefs.GetString("url");
            gameID = PlayerPrefs.GetString("gameID");
            updateTime = PlayerPrefs.GetString("updateTime");
            isDebug = PlayerPrefs.GetString("isDebug");
            token = PlayerPrefs.GetString("token");

            seed = 1;
            int.TryParse(tournamentID, out seed);
            UnityEngine.Random.InitState(seed);

            RestClient.DefaultRequestHeaders["token"] = token;
            currentRequest = new RequestHelper
            {
                Uri = basePath,
                Body = new Request
                {
                    tournament_id = int.Parse(tournamentID)
                }
                //EnableDebug = true
            };
            RestClient.Post<Duelit.Hole.Root>(currentRequest)
            .Then(res =>
            {
                Debug.Log("downloaded");

                int cityRot, playerIndex, themeIndex, droneIndex;
                float droneStart, droneRun;
                int vehicleIndex, peopleTypeIndex;

                int.TryParse(res.level.cityRotationIndex[0].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out cityRot);
                int.TryParse(res.level.playerSpawnIndex[0].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out playerIndex);
                int.TryParse(res.level.randThemeIndex[0].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out themeIndex);
                int.TryParse(res.level.randDroneSpawnPoint[0].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out droneIndex);
                float.TryParse(res.level.randomDroneStartTime[0].ToString(), NumberStyles.Float, new CultureInfo("en-US"), out droneStart);
                float.TryParse(res.level.randomDroneRunTime[0].ToString(), NumberStyles.Float, new CultureInfo("en-US"), out droneRun);

                cityManager.InitServerPool(cityRot, playerIndex, themeIndex);
                droneSystem.InitServerPool(droneIndex, droneStart, droneRun);
                

                for (int i = 0; i < res.level.vehicleTypeIndex.Length; i++)
                {
                    if (int.TryParse(res.level.vehicleTypeIndex[i].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out vehicleIndex)) { trafficSystem.InitServerPool(i, vehicleIndex); }
                }
                for (int i = 0; i < res.level.randomPeopleTypeIndex.Length; i++)
                {
                    if (int.TryParse(res.level.randomPeopleTypeIndex[i].ToString(), NumberStyles.Integer, new CultureInfo("en-US"), out peopleTypeIndex)) { crowdSystem.InitServerPool(i, peopleTypeIndex); }
                }


                StartGame();
                Debug.Log("Success: " + JsonUtility.ToJson(res, false));
                RestClient.ClearDefaultHeaders();

            })
            .Catch(err =>
            {
                Debug.Log("Error: " + err.Message);
                MainMenu.Exit();
                MainMenu.GetError(err.Message);
            });
        }

        public void StartGame()
        {
            try
            {
#if UNITY_WP8 || UNITY_ANDROID || UNITY_IOS
                Time.maximumDeltaTime = 0.33f;
                Time.fixedDeltaTime = 0.02f;
#else
		Time.maximumDeltaTime = 0.33f;
		Time.fixedDeltaTime = 0.02f;
#endif
                isGameRunning = true;
                gameData.elapsedTime = gameData.totalPlayTime;
                gameData.playerScore = 0;

                cityManager.StartGame();
                trafficSystem.StartGame();
                crowdSystem.StartGame();
                droneSystem.StartGame();

                loading.SetActive(false);
                MainMenu.GameLoaded();
            }
            catch (System.Exception ex) { MainMenu.GetError(ex.Message); }
        }

        public void Update()
        {
            if (isGameRunning)
            {
                if (gameData.elapsedTime <= 0) { isGameRunning = false; StartCoroutine(StartCountDownEndGame()); }
                else
                {
                    gameData.elapsedTime = Mathf.Clamp(gameData.elapsedTime - Time.deltaTime, 0, 100);
                    gameData.playerScore = Mathf.Clamp(gameData.playerScore, 0, 200000);
                }
            }
        }
        private IEnumerator UpdateTotalPoints()
        {
            float updateTime = 0;
            updateTime = Mathf.Clamp(updateTime, 1, int.Parse(PlayerPrefs.GetString("updateTime")));
            while (true)
            {
                yield return new WaitForSeconds(updateTime);
#if !UNITY_EDITOR
                MainMenu.UpdateTotalPoints(gameData.playerScore.ToString());
#endif
            }
        }
        private IEnumerator StartCountDownEndGame()
        {
            SendTotalPoints();
            yield return new WaitForSeconds(1.0f);
            //Clear Unused Textures
            Resources.UnloadUnusedAssets();
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        }
        public void SendTotalPoints()
        {
#if !UNITY_EDITOR
            MainMenu.SetTotalPoints(gameData.playerScore.ToString());
#endif
        }
    }
}
