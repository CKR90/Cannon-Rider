using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTNT : MonoBehaviour
{
    public Transform Vehicle;
    public CannonGun cannonGun;
    public Canvas MainCanvas;
    public Canvas TNTCanvas;
    public GameObject TNT;
    public GameObject PearIdiot;
    public RectTransform Arrow;
    public RectTransform ArrowText;
    public RectTransform DangerousText;
    private TutorialController controller;

    private GameObject tnt;

    private bool waitExplode = true;

    void Start()
    {
        controller = GetComponent<TutorialController>();
        TNTCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
        TNTCanvas.gameObject.SetActive(true);

        tnt = Instantiate(TNT);
        tnt.GetComponent<TNTGAI>().isTestTNT = true;
        tnt.GetComponent<TNTGAI>().Vehicle = Vehicle;

        Vector3 pos = Vector3.zero;
        pos.z = Vehicle.position.z + cannonGun.BulletImpactDistance - 3f;

        tnt.transform.position = pos;

        for(int i = -1; i < 2; i++)
        {
            if(i != 0)
            {
                GameObject p = Instantiate(PearIdiot);
                
                p.GetComponent<PearIdiot>().canvas = MainCanvas;

                Vector3 ps = Vector3.zero;
                ps.x = i * 2f;
                ps.z = Vehicle.position.z + cannonGun.BulletImpactDistance + 3f;
                p.transform.position = ps;

                Vector3 dir = Vehicle.position - p.transform.position;
                dir.y = 0f;
                p.transform.forward = dir;
            }
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(tnt.transform.position + Vector3.up);

        Arrow.anchoredPosition = screenPos - Vector3.right * Arrow.sizeDelta.x / 2f;
        ArrowText.anchoredPosition = Arrow.anchoredPosition + Vector2.up * 200f - Vector2.right * ArrowText.sizeDelta.x / 2f;
    }

    void Update()
    {
        if(waitExplode)
        {
            if(tnt == null)
            {
                waitExplode = false;
                Destroy(Arrow.gameObject);
                Destroy(ArrowText.gameObject);
                Invoke("OpenTapToPlay", 2f);

                DangerousText.DOScale(1f, 1f).SetEase(Ease.OutBounce);
            }
        }
    }

    private void OpenTapToPlay()
    {
        PlayerPrefs.SetInt(TutorialType.LearnTNT.ToString(), 1);
        controller.TapToPlayScreen.SetActive(true);
        Destroy(gameObject);
    }
}
