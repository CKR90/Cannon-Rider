using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGAI : MonoBehaviour
{
    [HideInInspector] public Transform Vehicle;
    [HideInInspector] public AwardMixer awardMixer;

    public GameObject Bomb;
    public GameObject AwardBox;
    public GameObject Body;
    public GameObject Prop;
    

    private float speed = 0f;
    private float destroyBoxTime = 0f;
    private int switcher = 0;
    private GameObject bombInstance;
    private GameObject awardBoxInstance;
    private float breakDistance = 0f;

    private static int[] oldSelectedIndex = new int[2] {-1, -1};

    void Start()
    {

        Vector3 pos = Vehicle.transform.position - Vector3.forward * 350f;
        pos.x = Random.Range(-2f, 2f);
        pos.y = 4f;
        transform.position = pos;
        Body.SetActive(true);
        Prop.SetActive(true);

        speed = Vehicle.GetComponent<AutoPilot>().maxVelocity + 50f;

        breakDistance = Random.Range(20f, 40f);
        transform.forward = Vector3.forward;

    }


    void Update()
    {
        switch (switcher)
        {
            case 0: Part1(); break;
            case 1: Part2(); break;

        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void Part1()
    {

        if (transform.position.z > Vehicle.transform.position.z + breakDistance)
        {
            int i = Random.Range(0, 2);

            if(i == oldSelectedIndex[0] && i == oldSelectedIndex[1])
            {
                i = (i + 1) % 2;
            }

            if(LevelDataTransfer.planeEvent.EnableAwardBox && LevelDataTransfer.planeEvent.EnableTNT)
            {
                oldSelectedIndex[1] = oldSelectedIndex[0];
                oldSelectedIndex[0] = i;

                if (i == 0) BreakBomb();
                else BreakAward();
            }
            else if(LevelDataTransfer.planeEvent.EnableTNT)
            {
                BreakBomb();
            }
            else if(LevelDataTransfer.planeEvent.EnableAwardBox)
            {
                BreakAward();
            }

            switcher++;
        }        
    }
    private void BreakBomb()
    {
        bombInstance = Instantiate(Bomb);
        bombInstance.transform.position = transform.position;
        bombInstance.GetComponent<Rigidbody>().velocity = transform.forward * speed / 3f;
        bombInstance.name = "TNT";
        bombInstance.GetComponent<TNTGAI>().Vehicle = Vehicle;
    }
    private void BreakAward()
    {
        awardBoxInstance = Instantiate(AwardBox);
        awardBoxInstance.transform.position = transform.position;
        awardBoxInstance.GetComponent<Rigidbody>().velocity = transform.forward * speed / 3f;
        awardBoxInstance.name = "AwardBox";
        awardBoxInstance.GetComponent<AwardBox>().awardMixer = awardMixer;
        awardBoxInstance.GetComponent<AwardBox>().Vehicle = Vehicle.GetComponent<AutoPilot>();
    }
    private void Part2()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 10f);

        if(transform.forward.y < .2f) transform.Rotate(Vector3.right, -Time.deltaTime * 20f);

        float roll = transform.localEulerAngles.z;
        if (roll > 180f) roll -= 360f;

        roll = Mathf.Abs(roll);

        if (roll < 50f) transform.Rotate(transform.forward, -Time.deltaTime * 20f);

        if (Vector3.Angle(transform.forward, -Vector3.forward) < 30f)
        {
            destroyBoxTime = 10f;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(bombInstance, destroyBoxTime);
        Destroy(awardBoxInstance, destroyBoxTime);
    }
}