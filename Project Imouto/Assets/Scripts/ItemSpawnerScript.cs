using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerScript : MonoBehaviour {

    [SerializeField]
    private GameObject locationMarkerPrefab;

    private GameObject locationMarker;
    private RaycastHit hitInfo;
    private Ray ray;

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
        ray = new Ray(new Vector3(locationToSpawn.x, locationToSpawn.y + 100f, locationToSpawn.z), -Vector3.up);
        if (Physics.Raycast(ray, out hitInfo, 200f))
        {
            if(locationMarker != null)
            {
                locationMarker.transform.position = hitInfo.point;
                locationMarker.SetActive(true);
            }
            else
            {
                locationMarker = Instantiate(locationMarkerPrefab, hitInfo.point, Quaternion.identity);
            }
        }
    }

    public void DisableLocationMarker()
    {
        if (locationMarker != null)
            locationMarker.SetActive(false);
    }
}
