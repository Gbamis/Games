using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSFX_Manager : MonoBehaviour
{
    public AudioSource dogs;
    public AudioSource checkpoint;
    public AudioSource footStep;

    private static DogSFX_Manager m_instance;
    public static DogSFX_Manager Instance
    {
        get{
            return m_instance;
        }
        set
        {
            m_instance = value;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }

    public void DogBark(){
        StartCoroutine(Bark(0.2f));
    }

    public void CheckPoint(){
        checkpoint.Play();
    }
    public void StepSFX(){
        footStep.Play();
    }

    IEnumerator Bark(float sec)
    {
        yield return new WaitForSeconds(sec);
        if(!dogs.isPlaying){
            dogs.Play();
        }
         
    }
}
