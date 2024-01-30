using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ��������̓����蔻��
/// </summary>
public class MoveHitDetection : MonoBehaviour
{

    [SerializeField, Header("�v���C���[�̃I�u�W�F�N�g�Q��")]
    private PlayerManager player;

    private bool hitDetection = false;  // �������Ă��邩

    // ���m���ڐG���Ă���i�d�Ȃ��Ă���j�ԁA�����I�Ɏ��s
    private void OnTriggerStay(Collider other)
    {
        // �L�����N�^�[�̏󋵂��~�܂��Ă��鎞�ȊO�͕Ԃ�
        if (!player.PlayerMoveStopCheck()) return;

        // ��Q��
        if (other.CompareTag("Obstacle"))
        {
            hitDetection = true;    // ��Q���ɓ�������
        }
    }

    /// <summary>
    /// �ړ��������̓����蔻������Z�b�g
    /// </summary>
    public void MoveHitDetectionReset()
    {
        hitDetection = false;
    }

    /// <summary>
    /// �ړ��������̓����蔻��
    /// </summary>
    public bool GetMoveHitDetection()
    {
        return hitDetection;
    }

}
