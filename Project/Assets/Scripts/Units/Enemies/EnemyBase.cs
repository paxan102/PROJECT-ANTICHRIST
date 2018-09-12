﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AiPlayerDetector))]
[RequireComponent(typeof(AiMovement))]
public class EnemyBase : UnitBase 
{
    [HideInInspector]
    public UnityEvent OnEnemyDeath = new UnityEvent();
    public int cost;

    public void CachePlayer(Player player)
    {

    }

    #region private

    AiMovement propMovement;
    AiPlayerDetector propPlayerDetector;
    
    protected override void InitComponents()
    {
        base.InitComponents();

        propMovement = GetComponent<AiMovement>();
        propMovement.Init();

        propDamagable.OnDie.AddListener(Die);

        propPlayerDetector = GetComponent<AiPlayerDetector>();
        propPlayerDetector.OnMiss.AddListener(HandleOnPlayerMiss);
        propPlayerDetector.OnSeen.AddListener(HandleOnPlayerSeen);
        propPlayerDetector.Init();

    }

    public void Die(DamageInfo info)
    {
        OnEnemyDeath.Invoke();
        //gameObject.SetActive(false);
    }

    public int ReturnCost()
    {
        return cost;
    }

    protected override void TerminateComponents()
    {
        propMovement.Terminate();
        propPlayerDetector.Terminate();

        base.TerminateComponents();
    }

    void HandleOnPlayerSeen(Transform playerTransform)
    {
        propMovement.StartChase(playerTransform);
    }

    void HandleOnPlayerMiss()
    {
        propMovement.StopChase();
    }

    #endregion

}
