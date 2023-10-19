using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class CharacterAnimController : MonoBehaviour
{
    public static CharacterAnimController Instance;

    public Animator animator;
    public AnimationCurve HandsShakeGraph;
    public AnimationCurve HandsDownGraph;
    public AnimationCurve Head;


    private bool playHandsShake = false;
    private bool playHandsDown = false;

    private float Timer = 0f;
    private CharacterMovementAcuity Acuity;

    private bool ThrowItem = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(!LevelDataTransfer.gamePlaySettings.basketEnable)
        {
            animator.SetLayerWeight(1, 0f);
        }
        else
        {
            animator.SetLayerWeight(1, 0.75f);
        }
    }

    private void Update()
    {

        HandsShakeLocomation();
        HandsDownLocomation();

    }

    public void HandsShake(CharacterMovementAcuity acuity)
    {
        if (playHandsShake) return;

        SetState(acuity, out playHandsShake);
    }
    private void HandsShakeLocomation()
    {
        if (!playHandsShake) return;

        Timer += Time.deltaTime;

        if (Timer >= 1f)
        {
            ResetState();
            if (LevelDataTransfer.gamePlaySettings.basketEnable) animator.SetLayerWeight(1, 0.75f);
            animator.SetLayerWeight(2, 0f);
            ThrowItem = false;
        }
        else
        {
            float scale = ((int)Acuity + 1) / 3f;
            float value = HandsShakeGraph.Evaluate(Timer) - 0.75f;
            value *= scale;
            value += 0.75f;
            if (LevelDataTransfer.gamePlaySettings.basketEnable) animator.SetLayerWeight(1, value);
            animator.SetLayerWeight(2, Head.Evaluate(Timer) * scale);

            if(!ThrowItem && Timer >= .4f)
            {
                ThrowItem = true;
                BasketController.Instance.ThrowItem(Acuity);
            }
        }
    }

    public void HandsDown(CharacterMovementAcuity acuity)
    {
        if (playHandsDown) return;

        SetState(acuity, out playHandsDown);

    }
    private void HandsDownLocomation()
    {
        if (!playHandsDown) return;

        Timer += Time.deltaTime;

        if (Timer >= .5f)
        {
            ResetState();
            if (LevelDataTransfer.gamePlaySettings.basketEnable) animator.SetLayerWeight(1, 0.75f);
            animator.SetLayerWeight(3, 0f);
            ThrowItem = false;
        }
        else
        {
            float scale = ((int)Acuity + 1) / 3f;
            float value = HandsDownGraph.Evaluate(Timer) - 0.75f;
            value *= scale;
            value += 0.75f;
            if (LevelDataTransfer.gamePlaySettings.basketEnable) animator.SetLayerWeight(1, value);
            animator.SetLayerWeight(3, Head.Evaluate(Timer) * scale);

            if (!ThrowItem && Timer >= .1f)
            {
                ThrowItem = true;
                BasketController.Instance.ThrowItem(Acuity);
            }
        }
    }
    private void SetState(CharacterMovementAcuity acuity, out bool Sign)
    {
        animator.SetLayerWeight(2, 0f);
        animator.SetLayerWeight(3, 0f);

        playHandsShake = false;
        playHandsDown = false;
        Sign = true;

        Acuity = acuity;
        Timer = 0f;
    }
    private void ResetState()
    {
        playHandsShake = false;
        playHandsDown = false;

        Timer = 0f;
    }
}

public enum CharacterMovementAcuity
{
    Soft,
    Normal,
    Hard
}
