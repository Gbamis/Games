using System.Collections;
using UnityEngine;
using System.Text;
using TMPro;

using UnityEngine.SceneManagement;

public class Narrator : MonoBehaviour
{
    public string story;
    private char[] chars;
    public float time;
    public TextMeshProUGUI textUi;
    private AudioSource key;
    public GameObject startUI;

    PlayerInput input;
    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }


    private void Awake()
    {
        input = new PlayerInput();
        key = GetComponent<AudioSource>();
        chars = new char[story.Length];
        startUI.SetActive(false);
        for (int i = 0; i < story.Length; i++)
        {
            chars[i] = story[i];
        }

        input.Player.Start.performed +=ctx =>{
            SceneManager.LoadScene("prototype");
        };
        StartCoroutine(Read());
    }

    private IEnumerator Read(){
        StringBuilder sb = new StringBuilder();
        int t = 0;
        while(t < chars.Length){
            if(chars[t] =='.' || chars[t] =='!'){
                yield return new WaitForSeconds(time*2);
            }
            yield return new WaitForSeconds(time);
            sb.Append(chars[t]);
            textUi.text = sb.ToString();
            key.Play();
            t+=1;
        }
        yield return new WaitForSeconds(time*10);
        startUI.SetActive(true);
    }
}
