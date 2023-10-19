using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class BasketController : MonoBehaviour
{
    public static BasketController Instance;
    public GPU_Spawner GPUItemSpawner;
    public GameObject basketL;
    public GameObject basketR;
    public Image BasketItemIcon;
    public TextMeshProUGUI BasketItemSize;

    [SerializeField] public List<BasketData> basketDatas;
    private BasketData data;

    private int indexChanger = 0;

    private int[] indices = new int[2];

    private Rigidbody rb;

    private int UpdateThrower = 0;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if(UpdateThrower > 0)
        {
            int limit = Mathf.Min(3, UpdateThrower);
            UpdateThrower -= limit;

            for (int i = 0; i < limit; i++)
            {
                GameObject g = Instantiate(data.RBPrefab, null);

                g.transform.DOScale(data.MeshScale * 3f, .5f);

                GPUInstanceObjectData gp = GPUItemSpawner.Batches[0].Find(x => x.Referance == data.PositionTransforms[indices[indexChanger]]);
                if (gp == null) gp = GPUItemSpawner.Batches[1].Find(x => x.Referance == data.PositionTransforms[indices[indexChanger]]);

                gp.Scale = Vector3.zero;
                g.transform.position = gp.Position;

                indices[indexChanger]--;
                indexChanger = (indexChanger + 1) % 2;

                g.GetComponent<MeshRenderer>().sharedMaterial = data.Materials[Random.Range(0, data.Materials.Count)];

                Vector3 velocity = Vector3.up * rb.velocity.y * Random.Range(1f, 2f);
                velocity.z = rb.velocity.z;
                velocity.x = Random.Range(-0.5f, 0.5f);
                g.GetComponent<Rigidbody>().velocity = velocity;

                Destroy(g, 20f);
            }
        }
    }
    public void Initialize()
    {
        rb = GameController.Instance.Vehicle.GetComponent<Rigidbody>();

        if (LevelDataTransfer.gamePlaySettings.basketEnable)
        {
            data = basketDatas.Find(x => x.Item == LevelDataTransfer.gamePlaySettings.basketItem);

            basketL.SetActive(true);
            basketR.SetActive(true);
            BasketItemIcon.gameObject.SetActive(true);
            BasketItemIcon.gameObject.GetComponent<Image>().sprite = data.Icon;
            BasketItemSize.SetText(GameDatabase.Instance.SetBasketItem(data.PositionTransforms.Count).ToString());

            indices[0] = data.PositionTransforms.Count / 2 - 1;
            indices[1] = data.PositionTransforms.Count - 1;
        }
        else
        {
            basketL.SetActive(false);
            basketR.SetActive(false);
            BasketItemIcon.gameObject.SetActive(false);
        }
    }
    public void ThrowItem(CharacterMovementAcuity acuity)
    {
        if (!GameController.Instance.GameStarted) return;
        if (GameController.Instance.GameFinished) return;
        if (GameDatabase.Instance.GetBasketItem() <= 0) return;

        float Strength = ((int)acuity + 1) / 3f;
        int value = Random.Range(4, 16);

        int throwSize = Mathf.FloorToInt(value * Strength);
        if (throwSize > GameDatabase.Instance.GetBasketItem()) throwSize = GameDatabase.Instance.GetBasketItem();
        BasketItemSize.SetText(GameDatabase.Instance.DecreaseBasketItem(throwSize).ToString());

        UpdateThrower += throwSize;
    }   
    public void ResetBaskets()
    {
        if(GPUItemSpawner.Batches != null && GPUItemSpawner.Batches.Count > 0)
        {
            foreach (var v in GPUItemSpawner.Batches[0])
            {
                v.Scale = data.MeshScale;
            }
            foreach (var v in GPUItemSpawner.Batches[1])
            {
                v.Scale = data.MeshScale;
            }
        }
    }
}

