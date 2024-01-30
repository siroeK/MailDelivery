using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���[�����~�߂�ǂ��o���v���W�F�N�g
/// </summary>
public class MailStopWall : MonoBehaviour
{

    [SerializeField, Header("���C���̊Ǘ��̃X�N���v�g�Q��")]
    private MainManager mainManager;

    [SerializeField, Header("�v���C���[�̃I�u�W�F�N�g�Q��")]
    private PlayerManager player;

    [Header("���[�����~�߂�ǂ̉��o���o������")]
    [SerializeField, Tooltip("���[�����~�߂�ǂ̉��o����")]
    private float mailStopTime = 10f;

    private float stopTime = 0f;

    [Header("�~�߂鉉�o���R����")]
    [SerializeField, Tooltip("�~�߂鉉�o���R����"), Multiline(2)]
    private string stopExplanationText = "���Ԃ̌o�߂ɂ��G���[�ŁA\n���[�������s���܂����B";

    [Header("�ړ�������A�j���[�V����(DOLocalMove)")]
    [SerializeField, Tooltip("�ړ��̉��o����")]
    private float moveProductionTime = 0.5f;
    [SerializeField, Tooltip("�ړ��̕ω��̊ɋ}�i���C�[�W���O�j���w��")]
    private Ease moveEase = Ease.Unset;

    // SE���Q�Ƃ�����
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        MailStopTimeReset();
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���v���C�󋵂��u�v���C���v�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.DuringPlay) return;

        if (stopTime < 0f) 
        {
            // �v���C���[���~�܂��Ă��邩�m�F
            if(player.PlayerMoveStopCheck())
            {
                // ���W���v���C���[��1�O��
                this.transform.position = new Vector3(0f, -0.1f, player.gameObject.transform.position.z + 1);

                // �Q�[���v���C�󋵂��u���ʁv�ɂ���
                mainManager.SetGamePlayStatus(MainManager.GamePlayStatus.Result);

                // �ǂ��o��
                VerticalAxisMove();
            }
        }
        else
        {
            stopTime -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ���[�����~�߂�ǂ̉��o���o�����Ԃ̃��Z�b�g
    /// </summary>
    public void MailStopTimeReset()
    {
        stopTime = mailStopTime;
    }

    /// <summary>
    /// DOTween���g�����c�������̈ړ��A�j���[�V����
    /// </summary>
    private void VerticalAxisMove()
    {
        // ���[�J�����W�ňړ�
        transform.DOLocalMoveY(
            2.0f,                       // �ړ��I���n�_
            moveProductionTime          // ���o����
        )
        .SetEase(moveEase)              // �ω��̊ɋ}�i���C�[�W���O�j���w��A���̑��x�Œl���ω�
        .OnStart(() =>                  // ���s�J�n���̃R�[���o�b�N
        {
            // ���[���~�߂�ǂ�SE
            SESound.MailStopWallSE();
        })
        .OnComplete(() =>               // ���s�������̃R�[���o�b�N
        {
            mainManager.GameOverResultDisplay(stopExplanationText);    // �Q�[���I�[�o�[���ʂ�\��
        });
    }

}
