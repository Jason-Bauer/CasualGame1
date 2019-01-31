using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSManager : MonoBehaviour
{
    public int typeToSpawn;
    public GameObject friendly;
    public GameObject enemy;
    private Vector3[] laneLocs = new Vector3[7];

    // Start is called before the first frame update
    void Start()
    {
        laneLocs[0] = new Vector3(0.0f, 4.3f, 0.0f);
        laneLocs[1] = new Vector3(0.0f, 2.9f, 0.0f);
        laneLocs[2] = new Vector3(0.0f, 1.4f, 0.0f);
        laneLocs[3] = new Vector3(0.0f, 0.0f, 0.0f);
        laneLocs[4] = new Vector3(0.0f, -1.4f, 0.0f);
        laneLocs[5] = new Vector3(0.0f, -2.8f, 0.0f);
        laneLocs[6] = new Vector3(0.0f, -4.3f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        Instantiate(friendly, laneLocs[typeToSpawn], new Quaternion());
    }
}
