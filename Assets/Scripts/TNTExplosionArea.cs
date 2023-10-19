using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TNTExplosionArea : MonoBehaviour
{
    [HideInInspector] public TNTGAI tnt;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            tnt.inArea.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(tnt.inArea.Find(x => x == other.gameObject) != null)
        {
            tnt.inArea.Remove(other.gameObject);
        }
    }
}
