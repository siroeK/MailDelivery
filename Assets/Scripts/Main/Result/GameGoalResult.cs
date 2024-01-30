using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �S�[���������̌��ʂ�\��
/// </summary>
public class GameGoalResult : MonoBehaviour
{

    [SerializeField, Header("���C���̊Ǘ��̎Q��")]
    private MainManager mainManager;

    [SerializeField, Header("�Q�[�����ʂ̎Q��")]
    private GameResult gameResult;

    [SerializeField, Header("�S�[���������̌��ʂ̃t���[���g")]
    private CursorDOTweenAnimation goalResultFrame;

    [Header("��������")]
    [SerializeField, Tooltip("���͖�������")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("���薳������")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // ��������

    // SE���Q�Ƃ�����
    private SESound SESound;

    /// <summary>
    /// �S�[���������̌��ʂ�\���̍��ڑI��
    /// </summary>
    private enum GoalResultItem
    {
        Restart = 0,        // �Q�[�����X�^�[�g
        TitleReturn = 1,    // �^�C�g���֖߂�
    }

    private GoalResultItem goalResultItem = GoalResultItem.Restart;

    // Start is called before the first frame update
    void Start()
    {
        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        goalResultItem = GoalResultItem.Restart;

        // ���͖�������
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���v���C�󋵂��u���ʁv�̑I�����ڈȊO�̎�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        // �J�[�\���̈ړ����A�j���[�V�����Ȃ珈����Ԃ�
        if (goalResultFrame.IsCursorMoveAnimation()) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        if (gameResult.GetCursorPressed().y == 1)
        {
            if (goalResultItem == GoalResultItem.Restart) return;
            goalResultItem--;
            GoalResultItemChange(goalResultItem);
        }
        else if (gameResult.GetCursorPressed().y == -1)
        {
            if (goalResultItem == GoalResultItem.TitleReturn) return;
            goalResultItem++;
            GoalResultItemChange(goalResultItem);
        }
        // ����
        else if (gameResult.GetDecision())
        {
            GoalResultItemDecision(goalResultItem);
        }
    }

    /// <summary>
    /// �S�[���������̌��ʂ̍��ڕύX
    /// </summary>
    private void GoalResultItemChange(GoalResultItem goalItem)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �S�[���������̌��ʂ̍��ڑI��
        switch (goalItem)
        {
            // �Q�[�����X�^�[�g
            case GoalResultItem.Restart:
                break;

            // �^�C�g���֖߂�
            case GoalResultItem.TitleReturn:
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + goalItem);
                break;
        }

        // �J�[�\�����ړ�������
        goalResultFrame.CursorMove((int)goalItem);
    }

    /// <summary>
    /// �S�[���������̎�ȍ��ڂ̌���
    /// </summary>
    private void GoalResultItemDecision(GoalResultItem goalItem)
    {
        // ���͖�������
        invalidTime = decisionInvalidTime;

        // �S�[���������̌��ʂ̍��ڑI��
        switch (goalItem)
        {
            // �Q�[�����X�^�[�g
            case GoalResultItem.Restart:
                // ����{�^����SE
                SESound.DecisionButtonSE();
                // ���X�^�[�g
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            // �^�C�g���֖߂�
            case GoalResultItem.TitleReturn:
                // ����{�^����SE
                SESound.DecisionButtonSE();
                // �^�C�g���V�[����
                SceneManager.LoadScene("Title");    
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + goalItem);
                break;
        }
    }

}
