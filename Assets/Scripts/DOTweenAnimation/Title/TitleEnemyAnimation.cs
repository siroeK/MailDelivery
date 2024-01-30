using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �^�C�g���̃G�l�~�[�A�j���[�V����
/// </summary>
public class TitleEnemyAnimation : MonoBehaviour
{

    [Header("�I�u�W�F�N�g")]
    [SerializeField, Tooltip("���̕����̃I�u�W�F�N�g")]
    private Transform body = null;
    [SerializeField, Tooltip("���������̃I�u�W�F�N�g")]
    private Transform leftFoot = null;
    [SerializeField, Tooltip("�E�������̃I�u�W�F�N�g")]
    private Transform rightFoot = null;

    [Header("�ړ�������A�j���[�V����(DOLocalMoveZ)")]
    [SerializeField, Tooltip("���̖ڕW�n�_")]
    private float leftTargetPos = 1.0f;
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease moveEase = Ease.Unset;

    private Vector3 originalPos;    // ���̍��W

    [Header("�~�܂��Ă��鎞�̃A�j���[�V����(DOLocalMoveZ)")]
    [SerializeField, Tooltip("�~�܂�Ƃ���̍��W")]
    private float stopPosZ = 0.15f;
    [SerializeField, Tooltip("�~�܂��Ă��鎞�̉��o����")]
    private float stopProductionTime = 0.75f;
    [SerializeField, Tooltip("�~�܂��Ă��鎞�̌J��Ԃ��^�C�v")]
    private LoopType stopLoopType = LoopType.Restart;
    [SerializeField, Tooltip("�~�܂��Ă��鎞�̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease stopEase = Ease.Unset;

    [SerializeField, Tooltip("�~�܂鎞�̍����̍��W")]
    private Vector3 leftFootPos = Vector3.zero;
    [SerializeField, Tooltip("�~�܂鎞�̉E���̍��W")]
    private Vector3 rightFootPos = Vector3.zero;

    private Tween stopAnimationTween;           // �~�܂��Ă��鎞�A�j���[�V�����̕Ԃ�l��p��

    [Header("�ړ��A�j���[�V����")]
    [Header("�ݒ肵�������̒ʉߒn�_��ʂ��Ĉړ�������(DOLocalPath)")]
    [SerializeField, Tooltip("�E���ړ��̌o�R�n�_")]
    private Vector3[] rightMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("�����ړ��̌o�R�n�_")]
    private Vector3[] leftMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float productionTime = 1.0f;
    [SerializeField, Tooltip("�ړ��̋Ȑ��̎��")]
    private PathType movePathType = PathType.Linear;
    [SerializeField, Tooltip("�ړ��́u�Ō��WayPoint���ŏ���WayPoint�v���q����悤���邩")]
    private bool moveOption = true;
    [SerializeField, Tooltip("�ړ��̌J��Ԃ��^�C�v")]
    private LoopType moveLoopType = LoopType.Restart;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease ease = Ease.Unset;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = this.transform.position;

        // �A�j���[�V����
        LeftMoveAnimation();
        StopAnimation();
        MoveAnimation();
    }

    /// <summary>
    /// DOTween���g�������Ɉړ����A�j���[�V����
    /// </summary>
    private void LeftMoveAnimation()
    {
        // ���[�J�����W��Z�ړ�
        this.transform.DOLocalMoveZ(
            leftTargetPos,              // �ړ��I���n�_
            moveProductionTime          // ���o����
        )
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {

        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {
            // ���̍��W��
            this.transform.position = originalPos;

            // �ēx�A�j���[�V����
            LeftMoveAnimation();

        })
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    /// <summary>
    /// DOTween���g�����~�܂��Ă��鎞�A�j���[�V����
    /// </summary>
    private void StopAnimation()
    {
        // ���[�J�����W��Z�ړ�
        this.stopAnimationTween = body.DOLocalMoveZ(
            stopPosZ,                   // �ړ��I���n�_
            stopProductionTime          // ���o����
        )
        .SetLoops(-1, stopLoopType)     // �J��Ԃ��i-1 = ���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(stopEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {
            // �~�܂鎞�̍����A�E���̍��W���C��
            leftFoot.localPosition = leftFootPos;
            rightFoot.localPosition = rightFootPos;
        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {

        })
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    /// <summary>
    /// DOTween���g�����ړ����A�j���[�V����
    /// </summary>
    private void MoveAnimation()
    {
        //  ���[�J�����W�ňړ�(����)
        leftFoot.transform.DOLocalPath(
            leftMoveWayPoints,          // �ʉߒn�_�̃��X�g
            productionTime,             // ���o����
            movePathType                // �Ȑ��̎�� �ACatmull-Rom�Ȑ����g�p(WayPoint���m���Ȑ��Ōq��)
                                        //gizmoColor: Color.red     // Gizmos�ŐԐF��ݒ�
        )
        .SetOptions(moveOption)         // �u�Ō��WayPoint���ŏ���WayPoint�v���q����悤�ɂȂ�܂�
        .SetLoops(-1, moveLoopType)     // �J��Ԃ��i���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(ease)                  // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����

        //  ���[�J�����W�ňړ�(�E��)
        rightFoot.transform.DOLocalPath(
            rightMoveWayPoints,         // �ʉߒn�_�̃��X�g
            productionTime,             // ���o����
            movePathType                // �Ȑ��̎�� �ACatmull-Rom�Ȑ����g�p(WayPoint���m���Ȑ��Ōq��)
                                        //gizmoColor: Color.red     // Gizmos�ŐԐF��ݒ�
        )
        .SetOptions(moveOption)         // �u�Ō��WayPoint���ŏ���WayPoint�v���q����悤�ɂȂ�܂�
        .SetLoops(-1, moveLoopType)     // �J��Ԃ��i���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(ease)                  // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

}
