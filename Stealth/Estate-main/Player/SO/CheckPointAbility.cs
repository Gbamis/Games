using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckPointAbility : PlayerAbility
{
    public string file;
    private CheckPoint_Manager checkPoint_Manager;

    public override void OnStart(PlayerEvent p, PlayerContext c)
    {
        base.OnStart(p, c);

        checkPoint_Manager = new CheckPoint_Manager(file);
        Context.checkPointUI.SetActive(false);
        Vector3 last = checkPoint_Manager.ReadLastLocation();
        if(last!=null){
             Context.controller.transform.position = last;
        }
    }
    public void Save(Vector3 loc){
        checkPoint_Manager.SaveLoc(loc);
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
