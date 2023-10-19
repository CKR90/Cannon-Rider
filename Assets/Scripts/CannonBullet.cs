using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    private RaycastHit hit;
    private Vector3 firstPos;
    private Vector3 hitOffset = Vector3.zero;
    private Rigidbody rb;

    private float bulletSpeed = 50f;
    private float bulletImpactDistance = 50f;

    GameObject hitObject;
    private void Start()
    {
        if(hit.collider != null) hitObject = hit.collider.gameObject;
        rb = GetComponent<Rigidbody>();
        firstPos = transform.position;


        if (hit.collider != null)
        {
            hitOffset = hit.point - hit.collider.transform.position;
        }
    }

    private void Update()
    {
        UpdateVelocity();
        CheckImpactDistance();
    }

    private void OnTriggerEnter(Collider other)
    {
        DetectTrigger(other);
    }
    private void DetectTrigger(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<I_Destroy>().KillWithGun(10, GunType.Any);
            Destroy(gameObject);
        }
        else if (other.tag == "TNT")
        {
            SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_120);
            other.gameObject.GetComponent<TNTGAI>().Explode(true);
            Destroy(gameObject);
        }
        else if (other.tag == "AwardBox")
        {
            other.gameObject.GetComponent<AwardBox>().Explode();
            Destroy(gameObject);
        }
    }
    private void CheckImpactDistance()
    {
        if (Vector3.Distance(firstPos, transform.position) > bulletImpactDistance)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            Vector3 dir = transform.forward;
            dir.y = 0f;
            rb.velocity = dir * bulletSpeed;
            GetComponent<Collider>().isTrigger = false;
            Destroy(this);
        }
    }
    private void UpdateVelocity()
    {
        if (hitObject != null)
        {
            Vector3 target = hitObject.transform.position;

            if(hitObject.TryGetComponent<CapsuleCollider>(out CapsuleCollider c))
            {
                target = hitObject.transform.position + c.center * hitObject.transform.localScale.y;
            }
            
            Vector3 dir = (target - transform.position).normalized;
            if (c == null) dir.y = 0f;

            transform.forward = dir;

            Vector3 posAdd = dir * bulletSpeed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, target);
            float newPosDist = posAdd.magnitude;
            posAdd = posAdd.normalized * Mathf.Min(distance, newPosDist);

            transform.position += posAdd;
        }
        else 
        {
            Vector3 dir = transform.forward;
            dir.y = 0f;
            transform.position += dir * bulletSpeed * Time.deltaTime;
        }
    }
    public void SetTarget(RaycastHit h)
    {
        hit = h;
    }
    public void SetBulletData(float speed, float impactDistance)
    {
        bulletSpeed = speed;
        bulletImpactDistance = impactDistance;
    }
}
