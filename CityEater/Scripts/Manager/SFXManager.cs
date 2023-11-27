using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public class SFXManager : MonoBehaviour
    {
        public AudioSource[] audios;
        public AudioClip[] themeFX;

        public void PlayEat() { audios[0].Play(); }
        public void PlaySwallow() { audios[1].Play(); }
        public void PlayBurp() { if (!audios[2].isPlaying) { audios[2].Play(); } }
        public void PlayCarHonk() { if (!audios[3].isPlaying) { audios[3].Play(); } }
        public void PlayerTheme(int sfxSoureindex, int randThemeIndex)
        {
            audios[sfxSoureindex].clip = themeFX[randThemeIndex];
            audios[sfxSoureindex].Play();
        }
        public void PlaySizeGain() { if (!audios[5].isPlaying) { audios[5].Play(); } }
        public void PlayScoreGained() { if (!audios[6].isPlaying) { audios[6].Play(); } }
        public void PlayMissileFired() { audios[7].Play(); }

        public void PlayPoliceSiren(bool value)
        {
            if (value) { audios[8].Play(); }
            else { audios[8].Stop(); }
        }

        public void PlayDroneFlying(bool value)
        {
            if (value) { audios[9].Play(); }
            else { audios[9].Stop(); }
        }
    }
}
