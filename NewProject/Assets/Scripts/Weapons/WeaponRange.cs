﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponRange : WeaponBase
{

    [HideInInspector]
    public UnityEvent OnShootEmpty = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnShoot = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnReloadStarted = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnReloadStopped = new UnityEvent();

    [SerializeField] float timeBetweenShots;
    [SerializeField] float timeForReload;
    [SerializeField] float projectileSpeed;
    [SerializeField] int maxBulletsInClip;
    [SerializeField]int maxBullets;

    public override void Setup(params MonoBehaviour[] args)
    {
        base.Setup(args);

        foreach (MonoBehaviour arg in args)
        {
            if (projectilesPool == null && arg is PoolBase)
            {
                projectilesPool = (PoolBase)arg;
            }
        }
    }

    public override void Init()
    {
        base.Init();

        currClipBullets = maxBulletsInClip;
        currBullets = maxBullets;

        shootTimer = gameObject.AddComponent<Timer>();
        shootTimer.Init(timeBetweenShots);
        reloadTimer = gameObject.AddComponent<Timer>();
        reloadTimer.Init(timeForReload);
    }

    public override void Disable()
    {
        reloadTimer?.OnTimersFinished.RemoveListener(StopReload);
        reloadTimer?.StopWork();
        isReloading = false;
        RequestStopAttackInternal();

        base.Disable();
    }

    public override void Enable()
    {
        base.Enable();
    }

    public void TryReload()
    {
        if (isReloading)
        {
            return;
        }

        if (currBullets == 0)
        {
            return;
        }

        wasShooting = isShooting;
        StartReload();
    }

    public int GetCurrClipBullets()
    {
        return currClipBullets;
    }

    public int GetCurrBullets()
    {
        return currBullets;
    }

    #region private

    const int NO_BULLETS = 0;

    PoolBase projectilesPool;
    Timer shootTimer;
    Timer reloadTimer;

    int currClipBullets;
    int currBullets;

    bool isShooting = false;
    bool isReloading = false;
    bool wasShooting = false;

    protected override void RequestStartAttackInternal()
    {
        base.RequestStartAttackInternal();

        shootTimer.OnTimersFinished.AddListener(TryShoot);
        TryShoot();
    }

    void TryShoot()
    {
        if (!CheckConditions())
        {
            return;
        }

        OnAttackStared.Invoke();
        Shoot();
        OnShoot.Invoke();
    }

    bool CheckConditions()
    {
        if (isReloading)
        {
            return false;
        }

        if (shootTimer.IsRunning())
        {
            return false;
        }

        if (currClipBullets <= NO_BULLETS)
        {
            OnShootEmpty.Invoke();
            return false;
        }

        return true;
    }

    protected override void RequestStopAttackInternal()
    {
        if (!isShooting)
        {
            wasShooting = false;
        }

        shootTimer?.OnTimersFinished.RemoveListener(TryShoot);
        shootTimer?.StopWork();
        isShooting = false;
        OnAttackStopped.Invoke();

        base.RequestStopAttackInternal();
    }

    protected virtual void Shoot()
    {
        isShooting = true;
        shootTimer.StartWork();
        currClipBullets -= 1;

        var projectile = projectilesPool.Take(Tags.BULLET);
        var projectileLogic = projectile.GetComponent<ProjectileBase>();
        projectileLogic.Enable(pivot, damage, projectileSpeed, damager);
    }

    void StartReload()
    {
        if (isShooting)
        {
            RequestStopAttackInternal();
        }

        isReloading = true;
        reloadTimer.OnTimersFinished.AddListener(StopReload);
        reloadTimer.StartWork();
        OnReloadStarted.Invoke();
    }

    void StopReload()
    {
        reloadTimer?.OnTimersFinished.RemoveListener(StopReload);
        reloadTimer?.StopWork();
        isReloading = false;
        currClipBullets = RefillClip();
        
        OnReloadStopped.Invoke();

        if (wasShooting)
        {
            RequestStartAttackInternal();
        }
    }

    int RefillClip()
    {
        int requiredBulletsToRefill = maxBulletsInClip - currClipBullets;
        int bulletsToRefill = currClipBullets;

        if (currBullets >= requiredBulletsToRefill)
        {
            currBullets -= requiredBulletsToRefill;
            bulletsToRefill += requiredBulletsToRefill;
        }
        else
        {
            bulletsToRefill += currBullets;
        }

        return bulletsToRefill;
    }

    #endregion

}
