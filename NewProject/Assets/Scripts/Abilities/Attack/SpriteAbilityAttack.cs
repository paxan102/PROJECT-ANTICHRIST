﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class SpriteAbilityAttack : MonoBehaviour
{
    [SerializeField] float lifetime;

    public void Init(List<string> ignoredTags)
    {
        gameCamera = GameObject.FindGameObjectWithTag(GAME_CAMERA).GetComponent<Camera>();
        this.ignoredTags = ignoredTags;
        timer = gameObject.AddComponent<Timer>();
        timer.Init(lifetime);

        gameObject.SetActive(false);
    }

    public void Enable(Transform initTransfrom, float damage, float speed, PropDamager damager)
    {
        if (gameObject.activeSelf)
            return;

        var newRot = Quaternion.Euler(initTransfrom.rotation.eulerAngles.x, initTransfrom.rotation.eulerAngles.y + 90, initTransfrom.rotation.eulerAngles.z);

        transform.SetPositionAndRotation(initTransfrom.position, newRot);

        gameObject.SetActive(true);

        this.damage = damage;
        this.speed = speed;
        this.damager = damager;

        timer.OnTimersFinished.AddListener(Disable);
        timer.StartWork();
        StartCoroutine(COROUTINE_MOVE);
    }

    public void Terminate()
    {
        Destroy(gameObject);
    }

    #region private

    protected PropDamager damager;
    protected float damage;
    float speed;
    Timer timer;
    List<string> ignoredTags;
    Vector3 velocity;
    Camera gameCamera;

    const string COROUTINE_MOVE = "Move";
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    const string GAME_CAMERA = "GameCamera";

    public void Disable()
    {
        speed = 0;
        damage = 0;

        timer.OnTimersFinished.RemoveListener(Disable);
        timer.StopWork();

        StopCoroutine(COROUTINE_MOVE);

        gameObject.SetActive(false);
    }

    IEnumerator Move()
    {
        for (; ;)
        {
            transform.position = transform.position + transform.up * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ignoredTags.Contains(other.tag))
        {
            var damagable = other.gameObject.GetComponent<PropDamagable>();
            if (damagable != null)
            {
                var damageInfo = new DamageInfo();
                damageInfo.damagable = damagable;
                damageInfo.damager = damager;
                damageInfo.damage = damage;
                damager.DoDamage(damageInfo);
            }
            
            Disable();
        }
    }

    #endregion

}
