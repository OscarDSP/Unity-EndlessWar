using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBeh : MonoBehaviour
{
    [SerializeField] private AudioSource loopSound;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent enemy;
	[SerializeField] private Transform player;
    [SerializeField] private Transform attPerc;
    [SerializeField] PlayerShootBeh playerBeh;
    [SerializeField] public GameObject lightCube;
    [SerializeField] private float attRad;
    [SerializeField] LayerMask WhatIsPlayer;
    public bool playerFound;
    private float hp;

    [Header("Atack properties")]
    [SerializeField] private float timeForNextAtck;
    [SerializeField] private float timeForDmg;
    [SerializeField] private int dmg;
    [SerializeField] private bool readyForAttack = true;

	private void Awake()
	{
        player = FindObjectOfType<PlayerShootBeh>().GetComponent<Transform>();
		playerBeh = player.GetComponent<PlayerShootBeh>();
	}

	void Start()
    {
	}

    void Update()
    {
        if (!playerBeh.GetDeathState())
        {
		    hp = this.GetComponent<Target>().health;
		    playerFound = Physics.CheckSphere(attPerc.position, attRad, WhatIsPlayer);

            enemy.SetDestination(player.position);


            if (!playerFound)
                enemy.speed = 2f;
            else
                enemy.speed = 0;
            AttackDetection();

            if (hp <= 0)
            {
			    enemy.speed = 0;
			    this.GetComponent<Target>().health = 0;
                loopSound.Stop();
                animator.SetBool("Attack", false);
                animator.SetBool("Death", true);
            }
        }
    }

    void AttackDetection()
    {
        if(hp > 0)
        {
            if(playerFound && readyForAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        animator.SetBool("Attack", true);
        readyForAttack = false;
        yield return new WaitForSecondsRealtime(timeForDmg);
		playerBeh.TakeDmg(dmg);
        yield return new WaitForSecondsRealtime(timeForNextAtck);
        readyForAttack = true;
		animator.SetBool("Attack", false);
	}

    public void SetLight()
    {
        lightCube.SetActive(true);
    }

    public void TurnOffLight()
    {
		lightCube.SetActive(false);
	}
}
