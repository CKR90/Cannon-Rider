using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAwardBox : MonoBehaviour
{
    public Canvas MainCanvas;
    public GameObject LearnAwardCanvas;
    public Transform Vehicle;
    public GameObject AwardBox;
    public GameObject TNT;
    public GameObject PearIdiot;
    public CannonGun cannonGun;
    public RectTransform Arrow;
    public TextMeshProUGUI txt;
    public Transform RList;
    public RectTransform skillArrow;


    private int switcher = 0;

    private GameObject award;
    private GameObject tnt;
    private List<GameObject> pears = new List<GameObject>();

    private bool setHelperComplete = false;

    void Start()
    {
        LearnAwardCanvas.gameObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);

        LearnAwardCanvas.SetActive(true);
    }

    void Update()
    {
        switch (switcher)
        {
            case 0: InstantiadeAwadBox(); break;
            case 1: WaitExplodeAwardBox(); break;
            case 2: InstantiatePears(); switcher++; break;
            case 3: WaitExplodePears(); break;

        }


        if (RList.childCount == 0 || (pears.Count > 0 && pears.FindAll(x => x == null).Count > 0))
        {
            skillArrow.gameObject.SetActive(false);
        }

        if (setHelperComplete) return;

        SetSkillHelper();
    }


    private void InstantiadeAwadBox()
    {
        award = Instantiate(AwardBox);
        award.GetComponent<AwardBox>().awardMixer = ObstacleController.instance.awardMixer;
        Vector3 pos = Vehicle.position + Vector3.forward * 12f;
        pos.y = 0f;

        award.transform.position = pos;

        Vector2 arrowPos = FindTopPixelPosition(award.transform.GetComponentInChildren<MeshRenderer>());
        Vector2 textPos = arrowPos;

        arrowPos.x -= 75f;
        arrowPos.y += 50f;
        Arrow.anchoredPosition = arrowPos;

        textPos.x -= 300f;
        textPos.y += 200f;

        txt.gameObject.GetComponent<RectTransform>().anchoredPosition = textPos;
        txt.SetText("Explode The Box!");

        switcher++;
    }
    private void WaitExplodeAwardBox()
    {
        if (award == null)
        {
            txt.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(false);

            switcher++;
        }
    }
    private void InstantiateTNT()
    {
        tnt = Instantiate(TNT);
        tnt.GetComponent<TNTGAI>().Vehicle = Vehicle;
        tnt.GetComponent<TNTGAI>().isTestTNT = true;
        Vector3 pos = Vehicle.position + Vector3.forward * 35f;
        pos.y = 0f;

        tnt.transform.position = pos;

        Vector2 arrowPos = FindTopPixelPosition(tnt.transform.GetComponentInChildren<MeshRenderer>());
        Vector2 textPos = arrowPos;

        arrowPos.x -= 82f;
        Arrow.anchoredPosition = arrowPos;

        textPos.x -= 300f;
        textPos.y += 150f;

        Arrow.gameObject.SetActive(true);
        txt.gameObject.SetActive(true);

        txt.gameObject.GetComponent<RectTransform>().anchoredPosition = textPos;
        txt.SetText("Explode TNT with the Bomb!");
    }
    private void InstantiatePears()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject p = Instantiate(PearIdiot);
            Vector3 pos = Vehicle.transform.position + Vector3.forward * (cannonGun.BulletImpactDistance + 5f);
            pos.x += i * 2f;
            pos.y = 0f;

            p.transform.position = pos;

            Vector3 dir = Vehicle.transform.position - p.transform.position;
            dir.y = 0f;
            dir = dir.normalized;
            p.transform.forward = dir;

            p.GetComponent<PearIdiot>().canvas = MainCanvas;
            pears.Add(p);
        }

        Vector2 arrowPos = FindTopPixelPosition(pears[1].transform.GetComponentInChildren<SkinnedMeshRenderer>());
        Vector2 textPos = arrowPos;
        arrowPos.y -= 50f;
        arrowPos.x -= 82f;
        Arrow.anchoredPosition = arrowPos;

        textPos.x -= 300f;
        textPos.y += 120f;

        Arrow.gameObject.SetActive(true);
        txt.gameObject.SetActive(true);

        txt.gameObject.GetComponent<RectTransform>().anchoredPosition = textPos;
        txt.SetText("Explode the pear with the Bomb!");
    }
    private void WaitExplodeTNT()
    {
        if (tnt == null)
        {
            txt.gameObject.SetActive(false);
            Arrow.gameObject.SetActive(false);

            switcher++;
        }
    }
    private void WaitExplodePears()
    {
        if (pears.FindAll(x => x == null).Count > 0)
        {
            Arrow.gameObject.SetActive(false);

            txt.SetText("x4 coins!");
            txt.gameObject.GetComponent<RectTransform>().localScale = Vector3.one * 2f;

            Vector2 pos = txt.gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.x -= 300f;
            pos.y -= 200f;

            txt.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
            txt.gameObject.GetComponent<RectTransform>().DOShakeAnchorPos(1f);

            Invoke("FinishTutorial", 2f);

            switcher++;
        }
    }

    private void FinishTutorial()
    {
        //PlayerPrefs.SetInt(TutorialType.LearnAwardBox.ToString(), 1);
        GetComponent<TutorialController>().TapToPlayScreen.SetActive(true);
        Destroy(gameObject);
    }

    private void SetSkillHelper()
    {
        if(!setHelperComplete && RList.childCount > 0)
        {
            setHelperComplete = true;
            Invoke("ShowHelper", .7f);
            
        }
    }
    private void ShowHelper()
    {
        RectTransform skill = RList.GetChild(0).GetComponent<RectTransform>();
        Vector2 pos = GetTopLeftScreenPosition(skill);
        skillArrow.anchoredPosition = pos;
        skillArrow.gameObject.SetActive(true);
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
    private Vector2 FindTopPixelPosition(MeshRenderer meshRenderer)
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

    public Vector2 GetTopLeftScreenPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return corners[1];
    }
}
