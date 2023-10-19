using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonListController : MonoBehaviour
{
    [HideInInspector] public bool BlockMovement = false;

    private float xRatio = 0f;
    private RectTransform rt;
    void Start()
    {
        xRatio = 250f / 1080f;
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.width * xRatio, rt.sizeDelta.y);
    }

    void Update()
    {
        if (BlockMovement) return;
    }
}
