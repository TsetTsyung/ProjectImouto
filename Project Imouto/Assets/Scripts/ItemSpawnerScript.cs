using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerScript : MonoBehaviour {

    [SerializeField]
    private GameObject locationMarkerPrefab;

    private GameObject locationMarker;

    private void Awake()
    {
        GameObjectDirectory.ItemSpawner = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnLocationMarker(Vector3 locationToSpawn)
    {
        locationToSpawn = Utilities.GetSurfacePoint(locationToSpawn);
        if(locationToSpawn != Vector3.zero)
        {
            if(locationMarker != null)
            {
                locationMarker.transform.position = locationToSpawn;
                locationMarker.SetActive(true);
            }
            else
            {
                locationMarker = Instantiate(locationMarkerPrefab, locationToSpawn, Quaternion.identity);
            }
        }
    }

    public void DisableLocationMarker()
    {
        if (locationMarker != null)
            locationMarker.SetActive(false);
    }
}
