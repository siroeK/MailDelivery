using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using unityroom.Api;

/// <summary>
/// ���C���̊Ǘ��v���W�F�N�g
/// </summary>
public class MainManager : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�̃I�u�W�F�N�g�Q��")]
    private Transform player;

    [SerializeField, Header("���[�����~�߂�ǂ��o���I�u�W�F�N�g�Q��")]
    private MailStopWall mailStopWall;

    [SerializeField, Header("�Q�[�����ʂ̃I�u�W�F�N�g�Q��")]
    private GameResult gameResult;

    [SerializeField, Header("�ő�̐i�񂾋����e�L�X�g�Q��")]
    private TextMeshProUGUI maxDistanceText;

    [SerializeField, Header("�ő�̐i�񂾋����A�j���[�V�����Q��")]
    private MaxDistanceAnimation maxDistanceAnimation;

    private int maxAdvancedDistance = 0;      // �ő�̐i�񂾋���

    private float maxPlayerAdvancedDistance = 0;    // �v���C���[�̍ő�̐i�񂾋���    

    // SE���Q�Ƃ�����
    private SESound SESound;

    /// <summary>
    /// �Q�[���v���C��
    /// </summary>
    public enum GamePlayStatus
    {
        Start,      // �X�^�[�g
        DuringPlay, // �v���C��
        Result,     // ����
    }
    private GamePlayStatus gamePlayStatus = GamePlayStatus.Start;


    private void Awake()
    {
        maxPlayerAdvancedDistance = player.transform.position.z;

        maxDistanceText.text = maxAdvancedDistance.ToString();  // �ő�̐i�񂾋����e�L�X�g�\��

        gamePlayStatus = GamePlayStatus.DuringPlay;

        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    /// <summary>
    /// �ő�̐i�񂾂�����(�O��ړ��������ƂɊm�F)
    /// </summary>
    public void SetMaxAdvanced(float posZ)
    {
        // �ő�̐i�񂾋���
        if (posZ > maxPlayerAdvancedDistance)
        {
            maxPlayerAdvancedDistance = posZ;   // �v���C���[�̍ő�̐i�񂾋���
            maxAdvancedDistance++;              // �ő�̐i�񂾋���
            mailStopWall.MailStopTimeReset();   // ���Ԃ̃��Z�b�g

            maxDistanceAnimation.ScaleAnimation();
            maxDistanceText.text = maxAdvancedDistance.ToString();  // �ő�̐i�񂾋����e�L�X�g�\�����X�V
        }
    }

    /// <summary>
    /// �Q�[���v���C�󋵂�n��
    /// </summary>
    public GamePlayStatus GetGamePlayStatus()
    {
        return gamePlayStatus;
    }

    /// <summary>
    /// �Q�[���v���C�󋵂�����
    /// </summary>
    public void SetGamePlayStatus(GamePlayStatus Status)
    {
        gamePlayStatus = Status;
    }

    /// <summary>
    /// �S�[���������̌��ʂ�\��
    /// </summary>
    public void GameGoalResultDisplay()
    {
        // ��������SE
        SESound.SuccessSoundSE();

        gameResult.GameGoalResult();
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̌��ʂ�\��
    /// </summary>
    public void GameOverResultDisplay(string explanation)
    {
        // �������[�h�̎�
        if (SceneManager.GetActiveScene().name == "EndlessMain")
        {
            // �Q�[���̍ő�̐i�s�������X�V���Ă��邩�ǂ���
            if (TitleManager.gameMaxAdvanced <= maxAdvancedDistance)
            {
                TitleManager.gameMaxAdvanced = maxAdvancedDistance; // �X�V����
            }

            // unityroom�̃{�[�hNo1�ɍő�̐i�s�����𑗐M����B
            UnityroomApiClient.Instance.SendScore(1, TitleManager.gameMaxAdvanced, ScoreboardWriteMode.HighScoreDesc);

            gameResult.EndlessGameOverResult(explanation, maxAdvancedDistance); // ���U���g�\��
        }
        // ����ȊO
        else
        {
            gameResult.GameOverResult(explanation); // ���U���g�\��
        }
    }

}
