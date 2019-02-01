using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    private HealthBar healthBar;
    public float health;
    public float attackPower;
    public Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate Health & DMG
        healthBar = gameObject.GetComponentsInChildren<HealthBar>()[0];
        health = 100;
        attackPower = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position += moveVec;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        health -= other.gameObject.GetComponent<UnitBehavior>().attackPower;
        healthBar.SetSize(health/100.0f);
    }
}
