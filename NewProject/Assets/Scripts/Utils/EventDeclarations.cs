﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDeclarations : MonoBehaviour
{
    
}

public class EventOnDie : UnityEvent<DamageInfo> { }
public class EventOnGotDamage : UnityEvent<DamageInfo> { }
public class EventOnSeen : UnityEvent<Transform> { }
public class EventOnMove : UnityEvent<Vector3> { }
public class EventOnCursorTrigger : UnityEvent<Collider> { }
public class EventOnDifficultChanged : UnityEvent<DifficultyType> { }
public class EventOnKillEnemy : UnityEvent<DamageInfo> { }
public class EventOnPlayerAbilityUseStart : UnityEvent<int> { }
public class EventOnPlayerAbilityUseStop : UnityEvent<int> { }