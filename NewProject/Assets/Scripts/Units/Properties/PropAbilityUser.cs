﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAbilityUser : PropBase
{
    [SerializeField] protected List<AbilityLogicBase> abilities;
    public List<string> ignoredTags;

    public override void Setup(params MonoBehaviour[] args)
    {
        base.Setup(args);

        propDamager = gameObject.AddComponent<PropDamager>();
        foreach (AbilityLogicBase ability in abilities)
        {
            ability.Setup(propDamager);
        }
    }

    public override void Init(Transform owner)
    {
        base.Init(owner);

        CreateAbilities();
    }

    public override void Terminate()
    {
        foreach (AbilityLogicBase ability in abilities)
        {
            if(ability)
                ability.Terminate();
        }

        base.Terminate();
    }

    protected virtual void StartUse(int idx)
    {
        if (idx < abilities.Count && abilities[idx] != null)
            StartUseInternal(idx);
    }

    protected virtual void StopUse(int idx)
    {
        if (idx < abilities.Count && abilities[idx] != null)
            StopUseInternal(idx);
    }

    #region private

    protected PropDamager propDamager;

    protected virtual void CreateAbilities()
    {
        foreach (AbilityLogicBase ability in abilities)
        {
            ability.Init();
        }
    }

    public void StartUseInternal(int idx)
    {
        abilities[idx].StartUse();
    }

    public void StopUseInternal(int idx)
    {
        abilities[idx].StopUse();
    }

    #endregion
}

public enum AbilitySlot
{
    SLOT_1,
    SLOT_2,
    MAX_COUNT
}

