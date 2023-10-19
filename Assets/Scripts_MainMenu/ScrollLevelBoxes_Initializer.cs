using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScrollLevelBoxes_Initializer : MonoBehaviour
{
    public Transform PackParent;

    private bool initialize = false;

    private bool AutoSlideDownAnim = false;
    private float AutoSlideDownTimer = 0f;

    private RectTransform packRect;

    public bool AwakeInit = false;

    private void Start()
    {
        packRect = PackParent.GetComponent<RectTransform>();
    }

    void Update()
    {
        if ((PackParent.gameObject.activeSelf || AwakeInit) && !initialize)
        {
            initialize = true;
            RectTransform r = PackParent.GetComponent<RectTransform>();
            Vector2 size = r.sizeDelta;
            size.y = 1720f * (PackParent.childCount - 1) + 1200f;
            r.sizeDelta = size;
        }
        else if (!PackParent.gameObject.activeSelf && initialize)
        {
            initialize = false;
            AwakeInit = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            AutoSlideDownAnim = false;
            AutoSlideDownTimer = 0f;
        }

        if (AutoSlideDownAnim)
        {
            AutoSlideDownTimer += Time.deltaTime;

            if (AutoSlideDownTimer >= 10f)
            {
                AutoSlideDownTimer = 0f;
                AutoSlideDownAnim = false;
            }

            packRect.anchoredPosition += Vector2.down * Time.deltaTime * 70f;
        }
    }



    public void FocusOnNextLevel()
    {
        StartCoroutine(FocusOnNextLevelAsync());
    }

    private IEnumerator FocusOnNextLevelAsync()
    {

        yield return new WaitUntil(() => LevelDataGenerator.Instance.levelBoxes.Count >= LevelDataGenerator.Instance.levels.Count);


        LevelBox next = LevelDataGenerator.Instance.levelBoxes.Find(x => x.IamNextLevel);


        if (next != null)
        {
            if (next.levelData.gamePlaySettings.levelIndex > 7)
            {
                RectTransform rt = next.gameObject.GetComponent<RectTransform>();

                int index = next.levelData.gamePlaySettings.levelIndex;
                int packIndex = Mathf.FloorToInt(index / 20f);
                int rowIndex = Mathf.FloorToInt((index % 20) / 4f);
                Vector2 pos = packRect.anchoredPosition;
                pos.y = packIndex * 1720f + rowIndex * 300f - 460f;
                packRect.anchoredPosition = pos;
            }
        }
        else
        {
            packRect.anchoredPosition = Vector2.up * packRect.sizeDelta.y;
            AutoSlideDownAnim = true;
            AutoSlideDownTimer = 0f;
        }
    }

}
