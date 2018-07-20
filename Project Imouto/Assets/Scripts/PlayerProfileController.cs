using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileController : MonoBehaviour {

    PlayerSaveProfile ourSaveProfile;
    FileHandler fileHandler;

    private void Awake(){
        GameObjectDirectory.PlayerProfileHandler = this;
    }

    // Use this for initialization
    void Start () {
        fileHandler = GameObjectDirectory.FileHandler;

        ourSaveProfile = fileHandler.GetPlayerProfileFromFile();

        if (ourSaveProfile == null)
            ourSaveProfile = new PlayerSaveProfile();
	}

	// Update is called once per frame
	void Update () {
		
	}

    //public PlayerSaveProfile GetProfile()
    //{
    //}
}

// serializable class for saving the player's data
[Serializable]
public class PlayerSaveProfile {

    public int PlayerLevel { get; protected set; }
    public int PlayerXP { get; protected set; }
    public Vector3 RespawnLocation { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int MaxStamina { get; protected set; }
    public int SwordLevel { get; protected set; }
    public int ShieldLevel { get; protected set; }
    public int SkillPointsToAssign { get; protected set; }

    public PlayerSaveProfile()
    {
        PlayerLevel = 0;
        PlayerXP = 0;
        //RespawnLocation = default start location
        MaxHealth = 50;
        MaxStamina = 50;
        SwordLevel = 1;
        ShieldLevel = 1;
        SkillPointsToAssign = 0;
    }
}