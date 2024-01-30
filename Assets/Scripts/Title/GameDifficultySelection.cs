using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[����Փx�̍��ڑI���̊Ǘ��X�N���v�g
/// </summary>
public class GameDifficultySelection : MonoBehaviour
{

    [SerializeField, Header("�^�C�g���̊Ǘ��X�N���v�g")]
    private TitleManager titleManager;

    [SerializeField, Header("���j���[�̔w�i")]
    private GameObject menuBackground;

    [SerializeField, Header("�Q�[����Փx�\��")]
    private GameObject gameDifficultyDisplay;

    [SerializeField, Header("��Փx�ݒ�̃I�u�W�F�N�g�Q��")]
    private Transform difficultySetting;

    [SerializeField, Header("��Փx�ݒ�̃I�u�W�F�N�g�Q��")]
    private TextMeshProUGUI difficultyText;

    [SerializeField, Header("��Փx�����̃I�u�W�F�N�g�Q��")]
    private TextMeshProUGUI explanationDifficultyText;
    
    [SerializeField, Header("�Q�[����Փx���ڂ̃t���[���g")]
    private CursorDOTweenAnimation difficultyCursorFrame;

    [Header("��Փx�e�L�X�g")]
    [SerializeField, Tooltip("�ȒP�ȓ�Փx"), Multiline(1)]
    private string easyDifficultyText = "�ȒP";
    [SerializeField, Tooltip("���ʂ̓�Փx"), Multiline(1)]
    private string normalDifficultyText = "����";
    [SerializeField, Tooltip("�����ȓ�Փx"), Multiline(1)]
    private string endlessDifficultyText = "����";

    [Header("������Փx�e�L�X�g")]
    [SerializeField, Tooltip("�ȒP�ȓ�Փx�̐���"), Multiline(3)]
    private string easyExplanationText = "�ȒP�ȃ��[�h\n15���C���ŃN���A�ł��B";
    [SerializeField, Tooltip("���ʂ̓�Փx�̐���"), Multiline(3)]
    private string normalExplanationText = "���ʃ��[�h\n30���C���ŃN���A�ł��B";
    [SerializeField, Tooltip("�����ȓ�Փx�̐���"), Multiline(3)]
    private string endlessExplanationText = "�������[�h\n�Q�[���I�[�o�[��\n�Ȃ�܂ő�����܂��B";

    [Header("��������")]
    [SerializeField, Tooltip("���͖�������")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("���薳������")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // ��������

    // SE���Q�Ƃ�����
    private SESound SESound;

    /// <summary>
    /// �Q�[����Փx�̑I��
    /// </summary>
    private enum DifficultySelection
    {
        Easy = 0,       // �ȒP
        Normal = 1,     // ����
        Endless = 2,    // ����
    }

    private DifficultySelection difficultySelection = DifficultySelection.Easy;

    /// <summary>
    /// ��Փx�\���̍��ڑI��
    /// </summary>
    private enum DifficultyItem
    {
        Difficulty = 0, // �Q�[����Փx
        GameStart = 1,  // �Q�[���X�^�[�g
        Return = 2,     // �߂�
    }

    private DifficultyItem difficultyItem = DifficultyItem.Difficulty;

    // Start is called before the first frame update
    void Start()
    {
        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // ��Փx�̐�����������
        difficultyText.text = easyDifficultyText;
        explanationDifficultyText.text = easyExplanationText;

        // ���͖�������
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�g���̉^�c�󋵂��Q�[����Փx�̑I�����ڈȊO�̎�
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.GameDifficulty) return;

        // �J�[�\���̈ړ����A�j���[�V�����Ȃ珈����Ԃ�
        if (difficultyCursorFrame.IsCursorMoveAnimation()) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        if (titleManager.GetCursorPressed().x == 1)
        {
            if (difficultyItem != DifficultyItem.Difficulty) return;

            if(difficultySelection == DifficultySelection.Endless)
            {
                difficultySelection = DifficultySelection.Easy;
            }
            else
            {
                difficultySelection++;
            }
            
            DifficultyChange(difficultySelection);

        }
        else if (titleManager.GetCursorPressed().x == -1)
        {
            if (difficultyItem != DifficultyItem.Difficulty) return;

            if (difficultySelection == DifficultySelection.Easy)
            {
                difficultySelection = DifficultySelection.Endless;
            }
            else
            {
                difficultySelection--;
            }

            DifficultyChange(difficultySelection);
        }
        else if (titleManager.GetCursorPressed().y == 1)
        {
            if (difficultyItem == DifficultyItem.Difficulty) return;
            difficultyItem--;
            DifficultyItemChange(difficultyItem);
        }
        else if (titleManager.GetCursorPressed().y == -1)
        {
            if (difficultyItem == DifficultyItem.Return) return;
            difficultyItem++;
            DifficultyItemChange(difficultyItem);
        }
        // ����
        else if (titleManager.GetDecision())
        {
            DifficultyItemDecision(difficultyItem);
        }
    }

    /// <summary>
    /// �Q�[����Փx�̕ύX
    /// </summary>
    private void DifficultyChange(DifficultySelection difficulty)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �Q�[����Փx�I��
        switch (difficulty)
        {
            // �ȒP
            case DifficultySelection.Easy:
                // �ȒP�̓�Փx�̐�����������
                difficultyText.text = easyDifficultyText;
                explanationDifficultyText.text = easyExplanationText;
                break;

            // ����
            case DifficultySelection.Normal:
                // ���ʂ̓�Փx�̐�����������
                difficultyText.text = normalDifficultyText;
                explanationDifficultyText.text = normalExplanationText;
                break;

            // ����
            case DifficultySelection.Endless:
                // �����̓�Փx�̐�����������
                difficultyText.text = endlessDifficultyText;
                explanationDifficultyText.text = endlessExplanationText;
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + difficulty);
                break;
        }

        // ���{�^����SE
        SESound.ArrowButtonSE();

    }

    /// <summary>
    /// �Q�[����Փx�̕ύX
    /// </summary>
    private void DifficultyItemChange(DifficultyItem difficultyItem)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �Q�[����Փx�̎�ȍ��ڑI��
        switch (difficultyItem)
        {
            // �Q�[����Փx
            case DifficultyItem.Difficulty:
                break;

            // �Q�[���X�^�[�g
            case DifficultyItem.GameStart:
                break;

            // �߂�
            case DifficultyItem.Return:
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + difficultyItem);
                break;
        }

        // �J�[�\�����ړ�������
        difficultyCursorFrame.CursorMove((int)difficultyItem);
    }

    /// <summary>
    /// �Q�[����Փx�̎�ȍ��ڂ̌���
    /// </summary>
    private void DifficultyItemDecision(DifficultyItem decisionItem)
    {
        // ���͖�������
        invalidTime = decisionInvalidTime;

        // �Q�[����Փx�̎�ȍ��ڑI��
        switch (decisionItem)
        {
            // �Q�[����Փx
            case DifficultyItem.Difficulty:
                break;

            // �Q�[���X�^�[�g
            case DifficultyItem.GameStart:

                // ���͖�������
                invalidTime = decisionInvalidTime;

                // ����{�^����SE
                SESound.DecisionButtonSE();

                // �Q�[����Փx�ɂ���ăV�[����ς���
                switch (difficultySelection)
                {
                    // �ȒP
                    case DifficultySelection.Easy:
                        // �ȒP�ȃQ�[���V�[����
                        SceneManager.LoadScene("EasyMain");
                        break;

                    // ����
                    case DifficultySelection.Normal:
                        // ���ʂȃQ�[���V�[����
                        SceneManager.LoadScene("NormalMain");
                        break;

                    // ����
                    case DifficultySelection.Endless:
                        // �����ȃQ�[���V�[����
                        SceneManager.LoadScene("EndlessMain");
                        break;

                    default:
                        Debug.LogError($"���ڂ��Ȃ��ł��B:" + difficultySelection);
                        break;
                }

                break;

            // �߂�
            case DifficultyItem.Return:

                // ���͖�������
                invalidTime = decisionInvalidTime;

                // ����{�^����SE
                SESound.DecisionButtonSE();

                // �ݒ�̍��ڂ�߂�
                difficultySelection = DifficultySelection.Easy;
                difficultyItem = DifficultyItem.Difficulty;

                // ��Փx�̐�����������
                difficultyText.text = easyDifficultyText;
                explanationDifficultyText.text = easyExplanationText;

                // �^�C�g���̉^�c�󋵂�߂�
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

                titleManager.DropDecision();    // ������͂���߂�

                // ��\����
                menuBackground.SetActive(false);
                gameDifficultyDisplay.SetActive(false);

                // �t���[���g�̈ʒu��߂�
                difficultyCursorFrame.gameObject.transform.position = difficultySetting.position;

                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + decisionItem);
                break;
        }
    }

}
