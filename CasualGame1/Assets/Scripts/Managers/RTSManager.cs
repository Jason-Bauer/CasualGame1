using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSManager : MonoBehaviour
{
    public int typeToSpawn;
    public GameObject friendly;
    public GameObject enemy;
    private Vector3[] laneLocs = new Vector3[7];
    private Vector3[] laneLocs2 = new Vector3[7];
    private float spawnTimer = 3.0f;
    private float time = 0f;
    public bool spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        //friendly locs
        laneLocs[0] = new Vector3(0.0f, 4.45f, 0.0f);
        laneLocs[1] = new Vector3(0.0f, 2.95f, 0.0f);
        laneLocs[2] = new Vector3(0.0f, 1.45f, 0.0f);
        laneLocs[3] = new Vector3(0.0f, -0.05f, 0.0f);
        laneLocs[4] = new Vector3(0.0f, -1.45f, 0.0f);
        laneLocs[5] = new Vector3(0.0f, -2.95f, 0.0f);
        laneLocs[6] = new Vector3(0.0f, -4.5f, 0.0f);
        //enemy locs
        laneLocs2[0] = new Vector3(8.0f, 4.45f, 0.0f);
        laneLocs2[1] = new Vector3(8.0f, 2.95f, 0.0f);
        laneLocs2[2] = new Vector3(8.0f, 1.45f, 0.0f);
        laneLocs2[3] = new Vector3(8.0f, -0.05f, 0.0f);
        laneLocs2[4] = new Vector3(8.0f, -1.45f, 0.0f);
        laneLocs2[5] = new Vector3(8.0f, -2.95f, 0.0f);
        laneLocs2[6] = new Vector3(8.0f, -4.5f, 0.0f);

        

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time>=spawnTimer&&spawning)
        {
            Instantiate(enemy, laneLocs2[Random.Range(0, 7)], new Quaternion());
            time = 0.0f;
        }

        //testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(enemy, laneLocs2[0], new Quaternion());
          
        }

    }

    public void Spawn()
    {
        Debug.Log("SPAWN");
        Instantiate(friendly, laneLocs[typeToSpawn], new Quaternion());
        
    }
}
