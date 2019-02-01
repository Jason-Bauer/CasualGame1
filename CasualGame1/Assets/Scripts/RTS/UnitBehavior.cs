﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    private HealthBar healthBar;
    public float health;
    public float attackPower;
    public Vector3 moveVec;
    public bool bases;
    public bool bump=false;
    public bool switcher = true;
    public bool friendly;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate Health & DMG
        if (!bases)
        {
            healthBar = gameObject.GetComponentsInChildren<HealthBar>()[0];
            health = 100;
            attackPower = 20;
        }
        this.gameObject.transform.parent = GameObject.Find("Panel (3)").transform;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!bases)
        {
            Move();
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        if (bump)
        {
            if (switcher)
            {
                moveVec.x = -moveVec.x;
                switcher = false;
                StartCoroutine(WaitAndSwitch(.1f));
            }
           
                
            
        }
        else
        {
            if (!switcher)
            {
                moveVec.x = -moveVec.x;
                switcher = true;

            }
        }
    }
    private IEnumerator WaitAndSwitch(float waitTime)
    {
        
            yield return new WaitForSeconds(waitTime);
        bump = false;
            //print("WaitAndPrint " + Time.time);
        
    }
    public void Move()
    {
        transform.position += moveVec;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            if (!bases && !other.gameObject.GetComponent<UnitBehavior>().bases)
            {
                if (other.gameObject.GetComponent<UnitBehavior>().friendly != friendly)
                {
                    bump = true;
                    health -= other.gameObject.GetComponent<UnitBehavior>().attackPower;
                    healthBar.SetSize(health / 100.0f);
                }
            }
            else
            {
                health -= other.gameObject.GetComponent<UnitBehavior>().attackPower;
            }
        }
        //bump = false;
    }
}
