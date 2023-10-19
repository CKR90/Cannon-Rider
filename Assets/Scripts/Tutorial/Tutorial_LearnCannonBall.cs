using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_LearnCannonBall : MonoBehaviour
{
    public Canvas MainCanvas;
    public Canvas LearnCannonCanvas;
    public GameObject Vehicle;
    public Transform Cannon;
    public RectTransform Darker;
    public TextMeshProUGUI drag;
    public TextMeshProUGUI drop;
    public GameObject AutoPilotText;
    public RectTransform ExplodeThem;
    public RectTransform Awesome;
    public GameObject hand;
    public GameObject PearIdiot;
    public List<RectTransform> Arrows;
    private TutorialController controller;


    private List<GameObject> pears = new List<GameObject>();

    private bool end = false;
    private float h;
    private bool onMouseDown = false;
    private float mouseDownH = 0f;

    void Start()
    {
        LearnCannonCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        controller = GetComponent<TutorialController>();
        LearnCannonCanvas.gameObject.SetActive(true);

        InstantiatePears();
        SetArrowPositions();
        SetDarkerLimit();
    }

    private void Update()
    {
        if(end)
        {
            if (hand.activeSelf) hand.SetActive(false);
            if (drag.gameObject.activeSelf) drag.gameObject.SetActive(false);
            if (drop.gameObject.activeSelf) drop.gameObject.SetActive(false);
            if (Darker.gameObject.activeSelf) Darker.gameObject.SetActive(false);
            return;
        }

        if(Input.GetMouseButton(0) && !onMouseDown)
        {
            onMouseDown = true;
            mouseDownH = Input.mousePosition.y;
        }
        else if(!Input.GetMouseButton(0) && onMouseDown)
        {
            onMouseDown = false;
        }


        if(Input.GetMouseButton(0) && mouseDownH < h)
        {
            if (hand.activeSelf) hand.SetActive(false);
            if (drag.gameObject.activeSelf) drag.gameObject.SetActive(false);
            if (Darker.gameObject.activeSelf) Darker.gameObject.SetActive(false);
            if (!drop.gameObject.activeSelf) drop.gameObject.SetActive(true);
        }
        else
        {
            if (!hand.activeSelf) hand.SetActive(true);
            if (!drag.gameObject.activeSelf) drag.gameObject.SetActive(true);
            if (drop.gameObject.activeSelf) drop.gameObject.SetActive(false);

            if(pears.FindAll(x => x == null).Count == 0)
            {
                if (!Darker.gameObject.activeSelf) Darker.gameObject.SetActive(true);
            }
        }

        if(pears.FindAll(x => x == null).Count == 3)
        {
            end = true;
            PlayerPrefs.SetInt(TutorialType.LearnCannonBall.ToString(), 1);
            ExplodeThem.gameObject.SetActive(false);
            Awesome.gameObject.SetActive(true);
            SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_60);
            Invoke("OpenTapToPlay", 1f);
        }
    }


    private void InstantiatePears()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject p = Instantiate(PearIdiot);
            p.transform.position = Vector3.forward * 12f + Vector3.right * ((i - 1) * 2f);

            Vector3 dir = Vehicle.transform.position - p.transform.position;
            dir.y = 0f;
            dir = dir.normalized;
            p.transform.forward = dir;

            p.GetComponent<PearIdiot>().canvas = MainCanvas;
            p.GetComponent<PearIdiot>().arrow = Arrows[i].gameObject;

            pears.Add(p);
        }
    }
    private void SetArrowPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            Arrows[i].anchoredPosition = Camera.main.WorldToScreenPoint(pears[i].transform.GetChild(0).position) - Vector3.right * Arrows[i].sizeDelta.x / 2f;
        }

        float h = Arrows.Max(x => x.anchoredPosition.y);
        ExplodeThem.anchoredPosition = Vector2.up * (h + 70f);
        Awesome.anchoredPosition = Vector2.up * (h + 70f);
    }
    private Vector2 FindTopPixelPosition(SkinnedMeshRenderer meshRenderer)
    {
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer is not set!");
            return Vector2.zero;
        }

        // Get the bounds of the MeshRenderer's mesh
        Bounds bounds = meshRenderer.bounds;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(bounds.center);
        float topPixelPosition = screenPos.y + (bounds.extents.y * 100);
        Vector2 topPixelPos = new Vector2(screenPos.x, topPixelPosition);

        return topPixelPos;
    }
    private void SetDarkerLimit()
    {
        h = Camera.main.WorldToScreenPoint(Cannon.position).y;
        Vector2 min = Darker.offsetMin;
        min.y = h;
        Darker.offsetMin = min;

        drag.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.up * (h - 50f);
        drop.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.up * (h - 50f);
    }
    private void OpenTapToPlay()
    {
        AutoPilotText.SetActive(true);
        controller.TapToPlayScreen.SetActive(true);
        Destroy(gameObject);
    }
}