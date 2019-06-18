﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool OnAimOnly;

    public Transform Reticule;

    Transform crossTop;
    Transform crossBottom;
    Transform crossLeft;
    Transform crossRight;

    float reticuleStartPoint;

    private void Start()
    {
        crossTop = Reticule.Find("Cross/Top").transform;
        crossBottom = Reticule.Find("Cross/Bottom").transform;
        crossLeft = Reticule.Find("Cross/Left").transform;
        crossRight = Reticule.Find("Cross/Right").transform;

        reticuleStartPoint = crossTop.localPosition.y;
    }

    void SetVisibility(bool value)
    {
        Reticule.gameObject.SetActive(value); 
    }

    private void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Reticule.transform.position = Vector3.Lerp(Reticule.transform.position, screenPosition, speed * Time.deltaTime);

        if (OnAimOnly)
        {
            SetVisibility(false);
            if (GameManager.Instance.InputController.Fire2)
            {
                // Disable reticule when not aiming
                SetVisibility(true);
            }
        }
        else
        {
            SetVisibility(true);
        }
    }

    public void ApplyScale(float scale)
    {
        crossTop.localPosition = new Vector3(0, reticuleStartPoint + scale, 0);
        crossBottom.localPosition = new Vector3(0, -reticuleStartPoint - scale, 0);
        crossLeft.localPosition = new Vector3(-reticuleStartPoint - scale, 0, 0);
        crossRight.localPosition = new Vector3(reticuleStartPoint + scale, 0, 0);
    }
}
