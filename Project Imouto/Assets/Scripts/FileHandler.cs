using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class FileHandler : MonoBehaviour {

    [SerializeField]
    private string playerProfileFilename = "PlayerProfile.dat";
    [SerializeField]
    private string playerSettingsFilename = "Settings.dat";

    private string dataPath;

    private void Awake()
    {
        GameObjectDirectory.FileHandler = this;
    }

    // Use this for initialization
    void Start () {
        dataPath = Application.dataPath + "/";
        Debug.Log(dataPath);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public PlayerSaveProfile GetPlayerProfileFromFile()
    {
        if (File.Exists(dataPath + playerProfileFilename))
        {
            // The file exists, so open it and get the settings
            using (Stream stream = File.OpenRead(dataPath + playerProfileFilename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (PlayerSaveProfile)formatter.Deserialize(stream);
            }
        }
     
        // The File doesn't exist so return null
        return null;
    }

    public void SavePlayerProfileToFile(PlayerSaveProfile profile)
    {
        using (Stream stream = File.OpenWrite(dataPath + playerProfileFilename))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, profile);
        }
    }
}