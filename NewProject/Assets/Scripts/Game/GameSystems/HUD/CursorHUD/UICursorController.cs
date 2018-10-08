﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UICursorController : UIBaseController
{
    [SerializeField] CursorInDetector detector;

    public override void Init()
    {
        base.Init();
        Cursor.visible = false;
        data.mousePoition = Input.mousePosition;
        uiView.Init();

        detector.OnTriggerIn.AddListener(CursorTriggerOn);
        detector.OnTriggerOut.AddListener(CursorTriggerOut);
    }

    #region private

    UICursorViewData data = new UICursorViewData();
    bool isEnemy = false;

    void CursorTriggerOn(Collider other)
    {
        if (other.GetComponents<Enemy>() != null)
        {
            isEnemy = true;
            Debug.Log("true");
        }
    }

    void CursorTriggerOut(Collider other)
    {
        if (other.GetComponents<Enemy>() != null)
        {
            isEnemy = false;
            Debug.Log("false");
        }
    }

    void Update()
    {
        data.mousePoition = Input.mousePosition;
        data.isEnemy = isEnemy;
        uiView.UpdateUI(data);
    }

    #endregion

}
