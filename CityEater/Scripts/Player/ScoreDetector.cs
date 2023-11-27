using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class ScoreDetector : MonoBehaviour
    {
        private int swallowCount = 0;
        private int burpCount = 0;
        [SerializeField] private int burpLimit;
        [SerializeField] private int swallowCheck;
        [SerializeField] private float sizeIncrease;
        [SerializeField] private float sizeDecrease;
        public ParticleSystem burpFX;
        public Transform player;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable") || other.CustomIsTag("Vehicle"))
            {
                swallowCount++;
                burpCount++;

                Consumable consumable = other.GetComponent<Consumable>();
                consumable.Consume();

                if (consumable.isNegative) { DecreasePlayer(); }
                else { IncreasePlayer(); }

                if (swallowCount > swallowCheck)
                {
                    GameManager.Instance.gameData.playerSpeed += .1f;
                    swallowCount = 0;
                }
                if (burpCount > burpLimit)
                {
                    GameManager.Instance.sFXManager.PlayBurp();
                    burpFX.Play();
                    burpCount = 0;
                }

            }

            Destroy(other.gameObject);
        }

        private void SizeSFX() { GameManager.Instance.sFXManager.PlaySizeGain(); }

        private IEnumerator Pop()
        {
            GameManager.Instance.uiManager.sizeUpPop.SetActive(true);
            SizeSFX();
            yield return new WaitForSeconds(.9f);
            GameManager.Instance.uiManager.sizeUpPop.SetActive(false);
        }
        
        private void IncreasePlayer()
        {
            Vector3 endScale = player.localScale;
            endScale.x += sizeIncrease;
            endScale.z += sizeIncrease;
            player.localScale = endScale;
        }

        public void DecreasePlayer()
        {
            Vector3 endScale = player.localScale;
            if (endScale.x > 0.2f && endScale.z > 0.2f)
            {
                endScale.x -= sizeDecrease;
                endScale.z -= sizeDecrease;
                player.localScale = endScale;
            }
            //GameManager.Instance.gameData.playerSpeed -= .1f;
        }
    }
}

