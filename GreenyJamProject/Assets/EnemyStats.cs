using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [SerializeField] private int health;
    private int visualCount;
    private bool isDead = false;
    [SerializeField] private GameObject[] healthVisuals;



    private void Start()
    {
        health = healthVisuals.Length -1;
        visualCount = healthVisuals.Length;
        foreach (var healthVisual in healthVisuals)
        {
            healthVisual.SetActive(false);
        }
        healthVisuals[healthVisuals.Length - 1].SetActive(true);
    }

    private void Update()
    {
        if (isDead)
            Destroy(this.gameObject);
    }


    public void DamageEnemy(int damage)
    {

    }

    public void DamageEnemy()
    {
        visualCount--;
        if (isDead) 
            return;
        healthVisuals[visualCount].SetActive(false);
        health--;
        if (health > 0)
            healthVisuals[visualCount].SetActive(true);
        if (health == 0)
            isDead = true;
        Debug.Log(isDead);
    }


}
