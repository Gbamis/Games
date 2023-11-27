using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Json;

public class CheckPoint_Manager
{

    private string fileName;
    public CheckPoint_Manager(string f)
    {
        fileName = f;
    }

    public Vector3 ReadLastLocation()
    {
        Vector3 loc = Vector3.zero;
        string path = Application.persistentDataPath + '/' + fileName + ".json";
        
        StreamReader sr = new StreamReader(path);
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            Data d = JsonUtility.FromJson<Data>(line);
            loc = d.lastLocation;
        }
        Debug.Log("location read");
        return loc;

    }
    public void SaveLoc(Vector3 loc)
    {
        string path = Application.persistentDataPath + '/' + fileName + ".json";
        Data d = new Data(loc);
        string json = JsonUtility.ToJson(d);
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(json);
        }
        Debug.Log("location saved");
    }
}

[System.Serializable]
public class Data
{
    public Vector3 lastLocation;
    public Data(Vector3 loc)
    {
        lastLocation = loc;
    }
}
