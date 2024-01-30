using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �E�C���X�G�l�~�[�A�j���[�V����
/// </summary>
public class ViralEnemyAnimation : MonoBehaviour
{

    [Header("�I�u�W�F�N�g")]
    [SerializeField, Tooltip("���̕����̃I�u�W�F�N�g")]
    private Transform body = null;
    [SerializeField, Tooltip("���������̃I�u�W�F�N�g")]
    private Transform leftFoot = null;
    [SerializeField, Tooltip("�E�������̃I�u�W�F�N�g")]
    private Transform rightFoot = null;

    [Header("���̂̃A�j���[�V����(DOLocalMoveZ)")]
    [SerializeField, Tooltip("���̖̂ڕW���W")]
    private float bodyTargetPosZ = 0.15f;
    [SerializeField, Tooltip("���o����")]
    private float bodyProductionTime = 0.75f;
    [SerializeField, Tooltip("�J��Ԃ��^�C�v")]
    private LoopType bodyLoopType = LoopType.Restart;
    [SerializeField, Tooltip("�ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease bodyEase = Ease.Unset;

    [Header("�ړ��̃A�j���[�V����")]
    [Header("�ݒ肵�������̒ʉߒn�_��ʂ��Ĉړ�������(DOLocalPath)")]
    [SerializeField, Tooltip("�E���ړ��̌o�R�n�_")]
    private Vector3[] rightMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("�����ړ��̌o�R�n�_")]
    private Vector3[] leftMoveWayPoints = new Vector3[2];
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("�ړ��̋Ȑ��̎��")]
    private PathType movePathType = PathType.Linear;
    [SerializeField, Tooltip("�ړ��́u�Ō��WayPoint���ŏ���WayPoint�v���q����悤���邩")]
    private bool moveOption = true;
    [SerializeField, Tooltip("�ړ��̌J��Ԃ��^�C�v")]
    private LoopType moveLoopType = LoopType.Restart;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease moveEase = Ease.Unset;

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�V����
        BodyAnimation();
        MoveAnimation();
    }

    /// <summary>
    /// DOTween���g�������̂̃A�j���[�V����
    /// </summary>
    private void BodyAnimation()
    {
        // ���[�J�����W��Z�ړ�
        body.DOLocalMoveZ(
            bodyTargetPosZ,             // �ړ��I���n�_
            bodyProductionTime          // ���o����
        )
        .SetLoops(-1, bodyLoopType)     // �J��Ԃ��i-1 = ���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(bodyEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {

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
            moveProductionTime,         // ���o����
            movePathType                // �Ȑ��̎�� �ACatmull-Rom�Ȑ����g�p(WayPoint���m���Ȑ��Ōq��)
                                        //gizmoColor: Color.red     // Gizmos�ŐԐF��ݒ�
        )
        .SetOptions(moveOption)         // �u�Ō��WayPoint���ŏ���WayPoint�v���q����悤�ɂȂ�܂�
        .SetLoops(-1, moveLoopType)     // �J��Ԃ��i���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����

        //  ���[�J�����W�ňړ�(�E��)
        rightFoot.transform.DOLocalPath(
            rightMoveWayPoints,         // �ʉߒn�_�̃��X�g
            moveProductionTime,             // ���o����
            movePathType                // �Ȑ��̎�� �ACatmull-Rom�Ȑ����g�p(WayPoint���m���Ȑ��Ōq��)
                                        //gizmoColor: Color.red     // Gizmos�ŐԐF��ݒ�
        )
        .SetOptions(moveOption)         // �u�Ō��WayPoint���ŏ���WayPoint�v���q����悤�ɂȂ�܂�
        .SetLoops(-1, moveLoopType)     // �J��Ԃ��i���[�v�j�A�ړ���A�ŏ��̈ʒu����Ăшړ����J�n
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }
}
