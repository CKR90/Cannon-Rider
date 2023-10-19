using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AutoPilot : MonoBehaviour
{
    #region Variables
    private bool start = false;
    public bool isBreaking = false;
    public int velocityLevel = 0;
    public float force = 1f;
    public float brakeForce = 1f;
    public float maxVelocity = 1f;
    public float DamperLengthScaler = 100f;
    public float VirtualAccelerationForce = 50f;
    public GameObject Booster;
    public ParticleSystem CircularStar;
    public AudioSource rollSound;

    public VehicleObjects vehicleObjects;

    [HideInInspector] public bool BoosterHelperEnabled = false;
    [HideInInspector] public bool BoosterGas = true;
    [HideInInspector] public float Gas = 1f;
    [HideInInspector] public float BoostBlocker = 0f;
    [HideInInspector] public bool BoostBlockerEnable = false;

    private Rigidbody rb;
    private bool waitStop = false;
    private bool turn = false;
    private float angle = 0f;

    private float velocityAvarage = 0f;
    private float velocityAvarageCounter = 0;
    private int velocityAvarageTimer = 0;
    private float acceleration = 0f;
    private float lastVelocity = 0f;

    private int rollSwitch = 0;
    private float rollSwitchTimer = 0f;
    private bool halfRoad = false;
    private int rollCount = 0;
    private float rollTimer = 0f;
    private bool enableJumpSpringSound = true;

    private bool disableJumpSpringSound = false;
    private float Block_JumpSpringSound_Timer = 0f;
    private float Block_JumpSpringSound_Time = 0f;

    private float hitSoundTimer = 0f;
    private GameObject lastHit;

    private bool enableTireHitSound = false;
    private bool BlockRollUntilLandingState = true;
    private float RollStateTimer = 0f;

    [HideInInspector] public bool finishGame = false;
    private float finishForceLeft = 0f;
    private float finishForceRight = 0f;
    #endregion
    #region Unity Events
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        SetVehicleProperty();
    }
    private void Update()
    {
        LimitXAxis();
    }
    void FixedUpdate()
    {
        


        CalculateVelocityAvarage();
        CalculateAcceleration();
        SetWheelAxe();
        ApplyDamperLength();

        UpdateSingleWheel(vehicleObjects.Collider_Left, vehicleObjects.WheelMesh_Left);
        UpdateSingleWheel(vehicleObjects.Collider_Right, vehicleObjects.WheelMesh_Right);

        if (finishGame)
        {
            FinishEvent();
            return;
        }

        if (!start)
        {
            ApplyBreaking(brakeForce);
            return;
        }
        if (disableJumpSpringSound) Block_JumpSpringSound();
        if (hitSoundTimer != 0f) PlayHitSound();

        AutoMove();
        ApplyPitch();
        ApplySlip();
        CheckRollState();
        CheckBlockRollState();
        JumpSpringSound();
        TireHitSound();
        RollingSound();
        BoosterEvent();
    }
    private void OnTriggerEnter(Collider other)
    {
        ColliderEvent(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ColliderEvent(collision.collider);
    }
    private void ColliderEvent(Collider other)
    {
        if (AwardMixer.Instance.ProtectorEnabled) return;
        if (lastHit == other.gameObject) return;

        if (other.gameObject.tag == "Enemy")
        {
            BoostBlockerEnable = false;
            lastHit = other.gameObject;
            rb.AddForce(Vector3.up * 300f * (velocityAvarage / maxVelocity), ForceMode.Impulse);
            PlayHitSound();
        }
    }
    #endregion
    #region Movement
    private void LimitXAxis()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -10f, 10f);
        transform.position = pos;
    }
    private void ApplyBreaking(float force)
    {
        vehicleObjects.Collider_Left.brakeTorque = force;
        vehicleObjects.Collider_Right.brakeTorque = force;
    }
    private void AutoMove()
    {
        angle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);

        ApplyBreaking(0f);

        if (turn && Mathf.Abs(angle) < 30f) turn = false;

        if ((Mathf.Abs(angle) > 50f || waitStop) && !turn) StopVehicle();
        else if (turn) Turn();
        else
        {
            AddTorque();
            SetMovementRotation();
        }
    }
    private void UpdateSingleWheel(WheelCollider collider, Transform wheel)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheel.position = pos;
        wheel.rotation = rot;
    }
    private void StopVehicle()
    {
        ApplyBreaking(brakeForce);
        waitStop = true;
        if (rb.velocity.magnitude < 0.2f)
        {
            waitStop = false;
            turn = true;
        }
    }
    private void Turn()
    {
        if (angle < 0f)
        {
            vehicleObjects.Collider_Left.brakeTorque = brakeForce;

            vehicleObjects.Collider_Right.brakeTorque = 0f;
            vehicleObjects.Collider_Right.motorTorque = force / 8f;
        }
        else
        {
            vehicleObjects.Collider_Left.brakeTorque = 0f;
            vehicleObjects.Collider_Left.motorTorque = force / 8f;

            vehicleObjects.Collider_Right.brakeTorque = brakeForce;
        }
    }
    private void AddTorque()
    {
        float v = rb.velocity.magnitude;

        float currentForce = force;

        if (v < maxVelocity - .3f)
        {
            float rpm_L = vehicleObjects.Collider_Left.rpm;
            float rpm_R = vehicleObjects.Collider_Right.rpm;
            vehicleObjects.Collider_Left.brakeTorque = 0f;
            vehicleObjects.Collider_Right.brakeTorque = 0f;

            if (v > maxVelocity / 2f) currentForce /= 3f;

            if (rpm_L + 15f < rpm_R)
            {
                currentForce /= 10f;
                vehicleObjects.Collider_Left.motorTorque = currentForce;
                vehicleObjects.Collider_Right.motorTorque = 0f;
            }
            else if (rpm_R + 15f < rpm_L)
            {
                currentForce /= 10f;
                vehicleObjects.Collider_Left.motorTorque = 0f;
                vehicleObjects.Collider_Right.motorTorque = currentForce;
            }
            else
            {

                vehicleObjects.Collider_Left.GetGroundHit(out WheelHit wheelDataL);
                vehicleObjects.Collider_Right.GetGroundHit(out WheelHit wheelDataR);

                string slip = "Slip: L:" + wheelDataL.forwardSlip.ToString("0.0") + " / R: ";
                slip += wheelDataR.forwardSlip.ToString("0.0");


                vehicleObjects.Collider_Left.motorTorque = currentForce;
                vehicleObjects.Collider_Right.motorTorque = currentForce;
            }
        }
        else
        {
            currentForce = 0f;
            vehicleObjects.Collider_Left.motorTorque = 0f;
            vehicleObjects.Collider_Right.motorTorque = 0f;

            if (v > maxVelocity + .3f)
            {
                vehicleObjects.Collider_Left.brakeTorque = brakeForce / 5f;
                vehicleObjects.Collider_Right.brakeTorque = brakeForce / 5f;
            }
            else
            {
                vehicleObjects.Collider_Left.brakeTorque = 0f;
                vehicleObjects.Collider_Right.brakeTorque = 0f;
            }
        }
    }
    private void SetMovementRotation()
    {
        if (!vehicleObjects.Collider_Left.isGrounded && !vehicleObjects.Collider_Right.isGrounded) return;

        Vector3 posTarget = transform.position;
        posTarget.x = 0f;
        posTarget.z += 10f;

        Vector3 dir = (posTarget - transform.transform.position).normalized;

        transform.transform.forward = Vector3.Lerp(transform.transform.forward, dir, Time.deltaTime * 2f);
    }
    private void CalculateVelocityAvarage()
    {
        velocityAvarageTimer++;
        velocityAvarageCounter += rb.velocity.magnitude;

        if (velocityAvarageTimer >= 10)
        {
            velocityAvarage = velocityAvarageCounter / 10f;
            velocityAvarageCounter = 0f;
            velocityAvarageTimer = 0;
        }
    }
    private void CalculateAcceleration()
    {
        float acc = (rb.velocity.magnitude - lastVelocity) / Time.deltaTime;
        acceleration = Mathf.Lerp(acceleration, (rb.velocity.magnitude - lastVelocity) / Time.deltaTime, Time.deltaTime * 20f);
        lastVelocity = rb.velocity.magnitude;
    }
    private void SetWheelAxe()
    {
        vehicleObjects.WheelAxe.position = (vehicleObjects.WheelMesh_Left.transform.position + vehicleObjects.WheelMesh_Right.transform.position) / 2f;

        float distance = (vehicleObjects.WheelAxe.position - vehicleObjects._Collider.position).magnitude;
        vehicleObjects.VehicleBody.transform.position = vehicleObjects.WheelAxe.position + vehicleObjects.WheelAxe.up * distance;
    }
    private void ApplyPitch()
    {
        float angle = -Mathf.Clamp(acceleration * 10f, 0f, 60f);
        Vector3 a = vehicleObjects.WheelAxe.transform.localEulerAngles;
        if (a.x > 180f) a.x -= 360f;
        a.x = Mathf.Lerp(a.x, angle, Time.deltaTime);
        vehicleObjects.WheelAxe.transform.localEulerAngles = a;
    }
    private void ApplySlip()
    {
        vehicleObjects.Collider_Left.GetGroundHit(out WheelHit hit_L);
        vehicleObjects.Collider_Right.GetGroundHit(out WheelHit hit_R);

        float slip_L = Mathf.Max(Mathf.Abs(hit_L.forwardSlip), Mathf.Abs(hit_L.sidewaysSlip));
        float slip_R = Mathf.Max(Mathf.Abs(hit_R.forwardSlip), Mathf.Abs(hit_R.sidewaysSlip));

        if (vehicleObjects.Collider_Left.isGrounded && (slip_L > .2f || vehicleObjects.Collider_Left.brakeTorque > brakeForce - 5f))
        {
            vehicleObjects.Trail_L.emitting = true;
            if (!vehicleObjects.Particle_L.isPlaying) vehicleObjects.Particle_L.Play();
        }
        else
        {
            vehicleObjects.Trail_L.emitting = false;
            if (vehicleObjects.Particle_L.isPlaying) vehicleObjects.Particle_L.Stop();
        }

        if (vehicleObjects.Collider_Right.isGrounded && (slip_R > .2f || vehicleObjects.Collider_Right.brakeTorque > brakeForce - 5f))
        {
            vehicleObjects.Trail_R.emitting = true;
            if (!vehicleObjects.Particle_R.isPlaying) vehicleObjects.Particle_R.Play();
        }
        else
        {
            vehicleObjects.Trail_R.emitting = false;
            if (vehicleObjects.Particle_R.isPlaying) vehicleObjects.Particle_R.Stop();
        }


    }
    private void ApplyDamperLength()
    {
        float distance = Vector3.Distance(vehicleObjects.WheelAxe.position, vehicleObjects.VehicleBody.transform.position);

        Vector3 Scale = vehicleObjects.Damper_L.localScale;
        Scale.z = distance * DamperLengthScaler;

        vehicleObjects.Damper_L.localScale = Scale;
        vehicleObjects.Damper_R.localScale = Scale;
    }
    public void BlockRollUntilLanding(Transform from, bool blockRoll = true)
    {
        if (blockRoll && !vehicleObjects.Collider_Right.isGrounded) return;

        if ((blockRoll && Vector3.Distance(from.position, transform.position) > 15f) || rb.angularVelocity.y > 3f) return;

        if (!blockRoll)
        {
            BlockRollUntilLandingState = false;
            RollStateTimer = 0f;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }
        else
        {
            BlockRollUntilLandingState = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    private void CheckBlockRollState()
    {
        if (BlockRollUntilLandingState && vehicleObjects.Collider_Left.isGrounded)
        {
            RollStateTimer += Time.deltaTime;
            if (RollStateTimer >= .2f)
            {
                BlockRollUntilLanding(transform, false);
                RollStateTimer = 0f;
            }
        }
    }
    public void SetVirtualAcceleration()
    {
        acceleration = VirtualAccelerationForce;
    }
    public void BoosterEvent()
    {
        if (!BoosterHelperEnabled)
        {
            if (Booster.activeSelf) Booster.SetActive(false);
        }
        else
        {
            if (!Booster.activeSelf) Booster.SetActive(true);
            waitStop = false;
            turn = false;

            Vector3 force = Vector3.zero;

            if (Mathf.Abs(transform.position.x) > 2f) force.x = -transform.position.x * 50f;
            else force.x = -rb.velocity.x * 50f;

            if (5f - transform.position.y > 0f) force.y = Mathf.Max(5f - transform.position.y, 2f) * (rb.mass + 500f);
            else if (rb.velocity.y < 0f) force.y = (rb.mass + 1000f) * Mathf.Abs(rb.velocity.y * 7f);
            else force.y = (rb.mass + 1000f);

            force.z = (maxVelocity * 1.5f - rb.velocity.z) * 200f;

            if(BoosterGas) rb.AddForce(force * Gas);

            if(rb.constraints != RigidbodyConstraints.FreezeRotation) rb.constraints = RigidbodyConstraints.FreezeRotation;
            transform.forward = Vector3.Lerp(transform.forward, Vector3.forward, Time.deltaTime * 3f);
            if(transform.position.y > 15f)
            {
                Vector3 pos = transform.position;
                pos.y = 15f;
                transform.position = pos;
            }
        }
    }
    #endregion
    #region Visual Effects and Sounds
    public void BombEffect(Transform t, float bombForce = 300f, float rollForce = 0f, bool checkIsGrounded = true)
    {
        if (AwardMixer.Instance.ProtectorEnabled) return;

        if (checkIsGrounded && !vehicleObjects.Collider_Left.isGrounded) return;

        float distance = Vector3.Distance(transform.position, t.position);
        if (distance < 20f)
        {
            Vector3 force = Vector3.up * bombForce * ((20f - distance) / 6f);
            Vector3 roll_force = Vector3.up * rollForce * ((20f - distance) / 6f);
            rb.AddForce(force, ForceMode.Impulse);

            if (distance < 15f) rb.AddTorque(roll_force, ForceMode.Impulse);

            if (rollForce == 0f)
            {
                if (force.y > 50f && force.y < 300f)
                {
                    SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_394);
                }
                else if (force.y >= 300f && force.y < 600f)
                {
                    SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_10);
                }
                else if (force.y >= 600f)
                {
                    SoundController.instance.Play(SFXList.Mouthpiece_Fart_2);
                }
            }
            else
            {
                SoundController.instance.PlayTalk(TalkList.Voice_Clip_Male_28);
            }


            if (force.y > 100f && force.y < 300f)
            {
                CharacterAnimController.Instance.HandsShake(CharacterMovementAcuity.Soft);
            }
            else if (force.y >= 300f && force.y < 600f)
            {
                CharacterAnimController.Instance.HandsShake(CharacterMovementAcuity.Normal);
            }
            else if (force.y >= 600f)
            {
                CharacterAnimController.Instance.HandsShake(CharacterMovementAcuity.Hard);
            }


            Block_JumpSpringSound(2f);
        }
    }
    public void Block_JumpSpringSound(float time = -1.0f)
    {
        if (time != -1.0f) Block_JumpSpringSound_Time = time;

        disableJumpSpringSound = true;
        Block_JumpSpringSound_Timer += Time.deltaTime;

        if (Block_JumpSpringSound_Timer >= Block_JumpSpringSound_Time)
        {
            Block_JumpSpringSound_Timer = 0f;
            disableJumpSpringSound = false;
        }
    }
    public void JumpSpringSound()
    {
        if (disableJumpSpringSound)
        {
            enableJumpSpringSound = false;
            return;
        }

        if (enableJumpSpringSound)
        {
            if (rb.velocity.y > 4f && !BoosterHelperEnabled)
            {
                SoundController.instance.Play(SFXList.Rubber_Band_Stretch_3);
                enableJumpSpringSound = false;
            }
        }
        else
        {
            if (rb.velocity.y < 1f)
            {
                enableJumpSpringSound = true;
            }
        }
    }
    public void PlayHitSound()
    {
        if (hitSoundTimer == 0f)
        {
            SoundController.instance.PlayTalk((TalkList)Random.Range((int)TalkList.Male_Hurt_01, (int)TalkList.Male_Hurt_04 + 1));
        }

        hitSoundTimer += Time.deltaTime;
        if (hitSoundTimer >= .5f)
        {
            hitSoundTimer = 0f;
        }
    }
    private void TireHitSound()
    {
        #region Booster Blocker
        if (BoosterHelperEnabled)
        {
            BoostBlockerEnable = true;
            BoostBlocker = 0f;
            return;
        }
        else if(!BoosterHelperEnabled && BoostBlockerEnable)
        {
            #region Apply Sound
            if (!vehicleObjects.Collider_Left.isGrounded) enableTireHitSound = true;
            else if (enableTireHitSound && vehicleObjects.Collider_Left.isGrounded)
            {
                float volume = Mathf.Clamp(Mathf.Abs(rb.velocity.y) / 5f, 0f, 1f);
                SoundController.instance.Play(SFXList.Tire_Hit, volume);
                enableTireHitSound = false;
            }
            #endregion


            if (BoostBlockerEnable)
            {

                BoostBlocker += Time.deltaTime;
                if (BoostBlocker < 2f)
                {
                    return;
                }
                else
                {
                    BoostBlocker = 0f;
                    BoostBlockerEnable = false;
                }
            }
        }
        #endregion

        if (!vehicleObjects.Collider_Left.isGrounded) enableTireHitSound = true;
        else if (enableTireHitSound && vehicleObjects.Collider_Left.isGrounded)
        {
            #region Apply Sound
            float volume = Mathf.Clamp(Mathf.Abs(rb.velocity.y) / 5f, 0f, 1f);
            SoundController.instance.Play(SFXList.Tire_Hit, volume);
            enableTireHitSound = false;
            #endregion
            #region Animate Character And Throw Apples
            float hitVelocity = Mathf.Abs(rb.velocity.y);
            if (hitVelocity > 6f) CharacterAnimController.Instance.HandsDown(CharacterMovementAcuity.Hard);
            else if (hitVelocity > 3f) CharacterAnimController.Instance.HandsDown(CharacterMovementAcuity.Normal);
            else CharacterAnimController.Instance.HandsDown(CharacterMovementAcuity.Soft);
            #endregion
        }
    }
    private void RollingSound()
    {
        float roll = Mathf.Abs(rb.angularVelocity.y);

        if (roll > 10f)
        {
            if (!rollSound.isPlaying) rollSound.Play();
            float value = Mathf.Clamp((roll - 10f) / 10f, 0f, 1f);
            rollSound.volume = value;
            rollSound.pitch = 0.5f + value / 3f;
        }
        else
        {
            if (rollSound.isPlaying)
            {
                rollSound.volume = 0f;
                rollSound.Stop();
            }
        }
    }
    #endregion
    #region StarAnimation
    private void CheckRollState()
    {
        switch (rollSwitch)
        {
            case 0: WaitTrueSituation(); break;
            case 1: Enable_CircularStarAnim(); break;
            case 2: SwitchWait(2.5f); break;
            case 3: Disable_CircularStarAnim(); break;
            case 4: rollSwitch = 0; break;
        }
    }
    private void WaitTrueSituation()
    {
        if (!halfRoad && transform.localEulerAngles.y > 150f && transform.localEulerAngles.y < 210f)
        {
            rollTimer = 0f;
            halfRoad = true;
        }

        if (halfRoad)
        {
            rollTimer += Time.deltaTime;

            if (transform.localEulerAngles.y > 330f || transform.localEulerAngles.y < 30f)
            {
                rollCount++;
                rollTimer = 0f;
                halfRoad = false;
            }
            else if (rollTimer >= 1f)
            {
                rollCount = 0;
                halfRoad = false;
            }
        }

        if (rollCount > 3)
        {
            rollCount = 0;
            halfRoad = false;
            rollSwitch++;
        }
    }
    private void Enable_CircularStarAnim()
    {
        Vector3 scale = CircularStar.transform.localScale;
        CircularStar.transform.localScale = Vector3.zero;
        CircularStar.gameObject.SetActive(true);

        CircularStar.transform.DOScale(scale, .3f).OnComplete(() => SoundController.instance.Play(SFXList.Comic_Head_Impact, 1f));

        rollSwitch++;
    }
    private void Disable_CircularStarAnim()
    {
        Vector3 scale = CircularStar.transform.localScale;
        CircularStar.transform.DOScale(0f, .3f).OnComplete(() =>
        {
            CircularStar.transform.localScale = scale;
            CircularStar.gameObject.SetActive(false);
            rollSwitch = 0;
        });

        if (rollSwitch != 0) rollSwitch = -1;
    }
    private void SwitchWait(float time)
    {
        rollSwitchTimer += Time.deltaTime;

        if (rollSwitchTimer >= time)
        {
            rollSwitch++;
            rollSwitchTimer = 0f;
        }
    }
    #endregion


    private void SetVehicleProperty()
    {
        velocityLevel = Mathf.Clamp(User.info.velocitylevel, 1, 20);

        switch (velocityLevel)
        {
            case 1:  maxVelocity = 12f;     break;
            case 2:  maxVelocity = 12.5f;   break;
            case 3:  maxVelocity = 13f;     break;
            case 4:  maxVelocity = 13.5f;   break;
            case 5:  maxVelocity = 14f;     break;
            case 6:  maxVelocity = 14.5f;   break;
            case 7:  maxVelocity = 15f;     break;
            case 8:  maxVelocity = 15.5f;   break;
            case 9:  maxVelocity = 16f;     break;
            case 10: maxVelocity = 16.5f;   break;
            case 11: maxVelocity = 17f;     break;
            case 12: maxVelocity = 17.5f;   break;
            case 13: maxVelocity = 18f;     break;
            case 14: maxVelocity = 18.5f;   break;
            case 15: maxVelocity = 19f;     break;
            case 16: maxVelocity = 19.5f;   break;
            case 17: maxVelocity = 20f;     break;
            case 18: maxVelocity = 21f;     break;
            case 19: maxVelocity = 22f;     break;
            case 20: maxVelocity = 23f;     break;
        }
    }
    public void ResetGame()
    {
        start = false;
        transform.position = Vector3.up;
        transform.forward = Vector3.forward;

        rb.velocity = Vector3.zero;

        ApplyBreaking(brakeForce);
        vehicleObjects.Collider_Left.motorTorque = 0f;
        vehicleObjects.Collider_Right.motorTorque = 0f;
        vehicleObjects.WheelAxe.transform.localEulerAngles = Vector3.zero;

        halfRoad = false;
        rollCount = 0;
        rollSwitch = 0;
        rollSwitchTimer = 0f;
        rollTimer = 0f;
        CircularStar.gameObject.SetActive(false);

        rollSound.volume = 0f;
        rollSound.Stop();

    }
    public void StartGame()
    {
        start = true;

    }
    public void FinishGame()
    {
        finishGame = true;

        int i = Random.Range(0, 2);

        if(i == 0)
        {
            finishForceLeft = force * 2f;
            finishForceRight = force / 2f;
        }
        else
        {
            finishForceLeft = force / 2f;
            finishForceRight = force * 2f;
        }
        
    }
    private void FinishEvent()
    {
        vehicleObjects.Collider_Left.brakeTorque = finishForceLeft;
        vehicleObjects.Collider_Right.brakeTorque = finishForceRight;

        ApplyPitch();
        ApplySlip();
        CheckRollState();
        CheckBlockRollState();
        JumpSpringSound();
        TireHitSound();
        RollingSound();
        BoosterHelperEnabled = false;
    }
}
[System.Serializable]
public class VehicleObjects
{
    public GameObject VehicleBody;
    public Transform WheelAxe;
    public Transform _Collider;
    public Transform Damper_L;
    public Transform Damper_R;
    public TrailRenderer Trail_L;
    public TrailRenderer Trail_R;
    public ParticleSystem Particle_L;
    public ParticleSystem Particle_R;
    public WheelCollider Collider_Left;
    public WheelCollider Collider_Right;
    public Transform WheelMesh_Left;
    public Transform WheelMesh_Right;
}