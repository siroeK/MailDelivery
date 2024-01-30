using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �E�C���X�G�l�~�[�̊Ǘ��v���W�F�N�g
/// </summary>
public class ViralEnemyManager : MonoBehaviour
{

    [Header("�G�̃Q�[���I�[�o�[���R����")]
    [SerializeField, Tooltip("�G�̃Q�[���I�[�o�[���R����"), Multiline(2)]
    private string enemyExplanationText = "�E�C���X�ɂ���ă��[����\n���g���R�k���Ă��܂����B";

    [SerializeField, Header("�����G�t�F�N�g")]
    private GameObject explosionEffect;

    [Header("�ړ�������A�j���[�V����(DOLocalMoveX)")]
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float moveProductionTime = 1.0f;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease moveEase = Ease.Unset;

    /// <summary>
    /// DOTween���g�������������̈ړ��A�j���[�V����
    /// </summary>
    public void HorizontalAxisMove(float targetX)
    {
        // ���[�J�����W�ňړ�
        transform.DOLocalMoveX(
            targetX,                    // �ړ��I���n�_
            moveProductionTime          // ���o����
        )
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {

        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {
            Destroy(this.gameObject);
        })
        .SetLink(gameObject);           // ����GameObject���폜���ꂽ�����Ŏ~�߂�悤�ɕR�t����
    }

    /// <summary>
    /// �G�̃Q�[���I�[�o�[���R����
    /// </summary>
    public string GetEnemyExplanation()
    {
        return enemyExplanationText;
    }

    /// <summary>
    /// �����G�t�F�N�g����
    /// </summary>
    public void ExplosionEffectGeneration()
    {
        Instantiate(explosionEffect, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
    }

}
