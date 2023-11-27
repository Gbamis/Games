using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Duelit.Hole
{

    public class UIManager : MonoBehaviour
    {
        [Header("Score popups")]
        //[SerializeField] private GameObject popUpPfrefab;
        [SerializeField] private ScorePopUp scorePopUp;
        public Transform PopPoint;

        public Transform canvas;
        private bool isLeft;

        public GameObject sizeUpPop;

        [Header("Duelit")]
        public Text timer;
        public Text score;


        private string minutes;
        private string seconds;

        private void Update()
        {
            minutes = Mathf.Floor(GameManager.Instance.gameData.elapsedTime / 60).ToString("00");
            seconds = (GameManager.Instance.gameData.elapsedTime % 60).ToString("00");

            timer.text = minutes + ":" + seconds;
            score.text = GameManager.Instance.gameData.playerScore.ToString();

        }

        public void Pop(int points, bool isNegative)
        {

            Vector2 screenPos = Camera.main.WorldToScreenPoint(PopPoint.position);
            ScorePopUp pop = Instantiate(scorePopUp);
            pop.SetDetails(canvas, screenPos, points, isNegative);

            int side = isLeft ? -1 : 1;
            isLeft = !isLeft;
            StartCoroutine(AnimatePop(pop.rectTransform, side));

            IEnumerator AnimatePop(RectTransform ui, int x)
            {
                Vector2 start = ui.position;
                Vector2 end = new Vector2(start.x + x * 400, start.y + 100);
                float step = 0;
                while (step < .5f)
                {
                    ui.position = Vector2.Lerp(start, end, step);
                    ui.localScale *= 1.01f;
                    step += Time.deltaTime;
                    yield return null;
                }
                Destroy(ui.gameObject);
            }

        }
    }
}
