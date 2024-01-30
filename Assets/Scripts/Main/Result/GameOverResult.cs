using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���I�[�o�[���̌��ʂ�\��
/// </summary>
public class GameOverResult : MonoBehaviour
{

    [SerializeField, Header("���C���̊Ǘ��̎Q��")]
    private MainManager mainManager;

    [SerializeField, Header("�Q�[�����ʂ̎Q��")]
    private GameResult gameResult;

    [SerializeField, Header("�Q�[���I�[�o�[���̌��ʂ̃t���[���g")]
    private CursorDOTweenAnimation gameOverFrame;

    [Header("��������")]
    [SerializeField, Tooltip("���͖�������")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("���薳������")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // ��������

    // SE���Q�Ƃ�����
    private SESound SESound;

    /// <summary>
    /// �Q�[���I�[�o�[���̌��ʂ�\���̍��ڑI��
    /// </summary>
    private enum GameOverItem
    {
        Restart = 0,        // �Q�[�����X�^�[�g
        TitleReturn = 1,    // �^�C�g���֖߂�
    }

    private GameOverItem gameOverItem = GameOverItem.Restart;

    // Start is called before the first frame update
    void Start()
    {
        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        gameOverItem = GameOverItem.Restart;

        // ���͖�������
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���v���C�󋵂��u���ʁv�̑I�����ڈȊO�̎�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        // �J�[�\���̈ړ����A�j���[�V�����Ȃ珈����Ԃ�
        if (gameOverFrame.IsCursorMoveAnimation()) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        if (gameResult.GetCursorPressed().y == 1)
        {
            if (gameOverItem == GameOverItem.Restart) return;
            gameOverItem--;
            GameOverItemChange(gameOverItem);
        }
        else if (gameResult.GetCursorPressed().y == -1)
        {
            if (gameOverItem == GameOverItem.TitleReturn) return;
            gameOverItem++;
            GameOverItemChange(gameOverItem);
        }
        // ����
        else if (gameResult.GetDecision())
        {
            GameOverItemDecision(gameOverItem);
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̌��ʂ̍��ڕύX
    /// </summary>
    private void GameOverItemChange(GameOverItem gameOver)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �Q�[���I�[�o�[���̌��ʂ̍��ڑI��
        switch (gameOver)
        {
            // �Q�[�����X�^�[�g
            case GameOverItem.Restart:
                break;

            // �^�C�g���֖߂�
            case GameOverItem.TitleReturn:
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + gameOver);
                break;
        }

        // �J�[�\�����ړ�������
        gameOverFrame.CursorMove((int)gameOver);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̍��ڂ̌���
    /// </summary>
    private void GameOverItemDecision(GameOverItem gameOver)
    {
        // ���͖�������
        invalidTime = decisionInvalidTime;

        // �S�[���������̌��ʂ̍��ڑI��
        switch (gameOver)
        {
            // �Q�[�����X�^�[�g
            case GameOverItem.Restart:
                // ����{�^����SE
                SESound.DecisionButtonSE();
                // ���X�^�[�g
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            // �^�C�g���֖߂�
            case GameOverItem.TitleReturn:
                // ����{�^����SE
                SESound.DecisionButtonSE();
                // �^�C�g���V�[����
                SceneManager.LoadScene("Title");
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + gameOver);
                break;
        }
    }

}
