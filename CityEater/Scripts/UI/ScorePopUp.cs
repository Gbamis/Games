using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Duelit.Hole
{
    public class ScorePopUp : MonoBehaviour
    {
        [SerializeField] private Color positiveScoreColor;
        [SerializeField] private Color negativeScoreColor;
        [SerializeField] private Text popText;
        [SerializeField] private Image popImage;
        public RectTransform rectTransform;

        public void SetDetails(Transform parent,Vector3 pos, int points, bool isNegative)
        {
            transform.SetParent(parent);
            rectTransform.position = pos;
            if (isNegative)
            {
                popText.text = "-" + points.ToString();
                popImage.color = negativeScoreColor;
            }
            else
            {
                popText.text = "+" + points.ToString();
                popImage.color = positiveScoreColor;
            }
            gameObject.SetActive(true);
        }
    }
}
