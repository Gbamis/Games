using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerEvent : MonoBehaviour
{
    public void Restart(){
        StartCoroutine(RestartCorutine());
    }
    
    private IEnumerator RestartCorutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
