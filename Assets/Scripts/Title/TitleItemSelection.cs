using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C�g���̎�ȍ��ڑI���̊Ǘ��X�N���v�g
/// </summary>
public class TitleItemSelection : MonoBehaviour
{

    [SerializeField, Header("���ڂ̃t���[���g")]
    private CursorDOTweenAnimation itemCursorFrame;

    [SerializeField, Header("�^�C�g���̊Ǘ��X�N���v�g")]
    private TitleManager titleManager;

    [SerializeField, Header("���j���[�̔w�i")]
    private GameObject menuBackground;

    [SerializeField, Header("�Q�[����Փx�\��")]
    private GameObject gameDifficultyDisplay;

    [SerializeField, Header("�I�v�V�����ݒ�\��")]
    private GameObject optionSettingDisplay;

    [SerializeField, Header("�Q�[�����[���\��")]
    private GameObject gameRulesDisplay;

    [SerializeField, Header("���͖�������")]
    private float inputInvalidTime = 0.25f;
    [SerializeField, Header("���薳������")]
    private float decisionInvalidTime = 0.5f;

    private float invalidTime = 0f;   // ��������

    // SE���Q�Ƃ�����
    private SESound SEManager;

    /// <summary>
    /// �^�C�g����ȍ��ڑI��
    /// </summary>
    private enum MainItemSelection
    {
        GameStart = 0,      // �Q�[���X�^�[�g
        SoundSetting = 1,   // �T�E���h�ݒ�(BGM�ASE)
        GameRules = 2,      // ���[������
    }

    private MainItemSelection mainItemSelection = MainItemSelection.GameStart;

    void Awake()
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // SE�̃X�N���v�g�Q��
        SEManager = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    // Update is called once per frame
    private void Update()
    {
        // �^�C�g���̉^�c�󋵂��^�C�g����ȍ��ڈȊO�̎�
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.MainItem) return;

        // �J�[�\���̈ړ����A�j���[�V�����Ȃ珈����Ԃ�
        if (itemCursorFrame.IsCursorMoveAnimation()) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        if (titleManager.GetCursorPressed().y == 1)
        {
            if (mainItemSelection == MainItemSelection.GameStart) return;
            mainItemSelection--;
            MainItemSelectionChange(mainItemSelection);
        }
        else if(titleManager.GetCursorPressed().y == -1) 
        {
            if (mainItemSelection == MainItemSelection.GameRules) return;
            mainItemSelection++;
            MainItemSelectionChange(mainItemSelection);
        }
        else if(titleManager.GetDecision())
        {
            MainItemDecision(mainItemSelection);
        }
    }

    /// <summary>
    /// �^�C�g����ȍ��ڂ̕ύX
    /// </summary>
    private void MainItemSelectionChange(MainItemSelection itemSelection)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �^�C�g����ȍ��ڑI��
        switch (itemSelection) 
        {
            // �Q�[���X�^�[�g
            case MainItemSelection.GameStart:
                break;

            // �T�E���h�ݒ�(BGM�ASE)
            case MainItemSelection.SoundSetting:
                break;

            // ���[������
            case MainItemSelection.GameRules:
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + itemSelection);
                break;
        }

        // �J�[�\�����ړ�������
        itemCursorFrame.CursorMove((int)itemSelection);
    }

    /// <summary>
    /// �^�C�g����ȍ��ڂ̌���
    /// </summary>
    private void MainItemDecision(MainItemSelection itemSelection)
    {
        // ���͖�������
        invalidTime = decisionInvalidTime;

        // ����{�^����SE
        SEManager.DecisionButtonSE();

        // �^�C�g����ȍ��ڑI��
        switch (itemSelection)
        {
            // �Q�[���X�^�[�g
            case MainItemSelection.GameStart:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.GameDifficulty);
                titleManager.DropDecision();    // ������͂���߂�
                menuBackground.SetActive(true);
                gameDifficultyDisplay.SetActive(true);
                break;

            // �T�E���h�ݒ�(BGM�ASE)
            case MainItemSelection.SoundSetting:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.OptionSetting);
                titleManager.DropDecision();    // ������͂���߂�
                menuBackground.SetActive(true);
                optionSettingDisplay.SetActive(true);
                break;

            // ���[������
            case MainItemSelection.GameRules:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.RuleExplanation);
                titleManager.DropDecision();    // ������͂���߂�
                menuBackground.SetActive(true);
                gameRulesDisplay.SetActive(true);
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + itemSelection);
                break;
        }
    }
}
