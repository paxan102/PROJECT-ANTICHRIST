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