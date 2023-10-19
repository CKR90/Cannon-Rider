using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public static ObstacleController instance;

    public Transform Vehicle;
    [Min(.2f)]public float maxSpawnTimeFrequency = 1.0f;
    public List<GameObject> Obstacles;
    public GameObject Plane;
    public AwardMixer awardMixer;
    public Canvas GameUICanvas;

    private bool start = false;
    private float timer = 0f;
    private float spawnTime = 0f;

    private float planeTimer = 0f;
    private float planeSpawnTime = 0f;
    private GameObject planeInstance;

    [HideInInspector] public List<GameObject> enemies = new List<GameObject>();
    private float clearNullEnemies_Timer = 0f;

    private bool finishGame = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        spawnTime = Random.Range(0.19f, LevelDataTransfer.gamePlaySettings.EnemiesMaxSpawnTime);
        planeSpawnTime = Random.Range(10f, 20f);
    }


    void Update()
    {
        if (!start || finishGame) return;

        timer += Time.deltaTime;
        if(timer >= spawnTime)
        {
            spawnTime = Random.Range(0.19f, maxSpawnTimeFrequency);
            timer = 0f;
            SpawnEnemy();
        }
        SpawnPlane();



        clearNullEnemies_Timer += Time.deltaTime;
        if(clearNullEnemies_Timer > 5f)
        {
            clearNullEnemies_Timer = 0f;
            enemies.RemoveAll(x => x == null);
        }
    }
    private void SpawnEnemy()
    {
        int index = Random.Range(0, Obstacles.Count);

        GameObject g = Instantiate(Obstacles[index], transform);
        I_Enemy sc = g.GetComponent<I_Enemy>();
        g.name = sc.GetName();
        g.tag = "Enemy";
        

        Vector3 pos = Vehicle.position;
        pos.z += Random.Range(LevelDataTransfer.gamePlaySettings.EnemiesMinSpawnDistance, LevelDataTransfer.gamePlaySettings.EnemiesMaxSpawnDistance);
        pos.x = Random.Range(-2f, 2f);

        g.transform.position = pos;
        sc.SetDefaultScale(g.transform.localScale);
        g.transform.localScale = Vector3.zero;

        Vector3 center = pos;
        center.x = 0f;
        center.z += Random.Range(-20f, 10f);
        sc.SetCenterPosition(center);
        sc.SetPlayer(Vehicle);
        sc.SetRoadLength(100f);
        sc.SetParentUICanvas(GameUICanvas);

        enemies.Add(g);
    }
    private void SpawnPlane()
    {
        if (!LevelDataTransfer.planeEvent.EnablePlane) return;

        planeTimer += Time.deltaTime;
        if(planeTimer >= planeSpawnTime)
        {
            planeSpawnTime = Random.Range(15f, 30f);
            planeTimer = 0f;
            planeInstance = Instantiate(Plane);
            planeInstance.name = "Plane";
            planeInstance.GetComponent<PlaneGAI>().Vehicle = Vehicle;
            planeInstance.GetComponent<PlaneGAI>().awardMixer = awardMixer;
        }
    }


    public void StartGame()
    {
        start = true;
    }
    public void ResetGame()
    {
        start = false;

        List<Transform> transforms = transform.GetComponentsInChildren<Transform>().ToList();
        transforms.RemoveAt(0);

        foreach(var t in transforms)
        {
            Destroy(t.gameObject);
        }

        Destroy(planeInstance);
        planeSpawnTime = Random.Range(10f, 40f);
        planeTimer = 0f;
    }
    public void FinishGame()
    {
        finishGame = true;

        List<Transform> transforms = transform.GetComponentsInChildren<Transform>().ToList();
        transforms.RemoveAt(0);
        transforms.RemoveAll(t => t.gameObject.GetComponent<I_Enemy>() == null);

        foreach (var t in transforms)
        {
            I_Enemy i = t.GetComponent<I_Enemy>();
            i.DestroyMe();
        }
    }
}
