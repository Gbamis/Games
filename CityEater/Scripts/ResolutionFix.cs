using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class ResolutionFix : MonoBehaviour
    {
        private void Update()
        {
            int height = Camera.main.scaledPixelHeight;
            int width = Camera.main.scaledPixelWidth;

            if (width > height)
            {
                GameManager.Instance.mainCamera.orthographicSize = 4;
            }
            else { GameManager.Instance.mainCamera.orthographicSize = 10; }
        }
    }
}
