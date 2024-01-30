using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// �I�v�V�����̐ݒ�X�N���v�g(BGM�ESE)
/// </summary>
public class OptionSetting : MonoBehaviour
{

    [SerializeField, Header("BGM�̉��ʃe�L�X�g")]
    private TextMeshProUGUI BGMVolumeText;

    [SerializeField, Header("SE�̉��ʃe�L�X�g")]
    private TextMeshProUGUI SEVolumeText;

    [SerializeField, Header("�^�C�g���̊Ǘ��X�N���v�g")]
    private TitleManager titleManager;

    [SerializeField, Header("���j���[�̔w�i")]
    private GameObject menuBackground;

    [SerializeField, Header("�I�v�V�����ݒ�\��")]
    private GameObject optionSettingDisplay;

    [SerializeField, Header("BGM�̐ݒ�I�u�W�F�N�g�Q��")]
    private Transform BGMSetting;

    [SerializeField, Header("���ڂ̃t���[���g")]
    private CursorDOTweenAnimation optionCursorFrame;

    // BGM��SE���Q�Ƃ�����
    private BGMSound BGMSound;
    private SESound SESound;

    [SerializeField, Header("����")]
    public int upDownValue = 10;        // �㉺������l

    public int maxBGMVolume = 100;      // �ő��BGM�̑傫��
    public static int BGMVolume = 50;   // BGM�̑傫��
    public int maxSEVolume = 100;       // �ő��SE�̑傫��
    public static int SEVolume = 50;    // SE�̑傫��

    [SerializeField, Header("���͖�������")]
    private float inputInvalidTime = 0.25f;
    [SerializeField, Header("���薳������")]
    private float decisionInvalidTime = 0.5f;

    private float invalidTime = 0f;   // ��������


    /// <summary>
    /// �I�v�V�����ݒ�̍��ڑI��
    /// </summary>
    private enum OptionSelection
    {
        BGM = 0,    // BGM
        SE = 1,     // SE
        Return = 2, // �߂�
    }

    private OptionSelection optionSelection = OptionSelection.BGM;

    // Start is called before the first frame update
    void Start()
    {
        // BGM�ESE�̃X�N���v�g�Q��
        BGMSound = GameObject.FindWithTag("BGM").GetComponent<BGMSound>();
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        BGMVolumeText.text = BGMVolume.ToString();
        SEVolumeText.text = SEVolume.ToString();

        // ���͖�������
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    private void Update()
    {
        // �^�C�g���̉^�c�󋵂��I�v�V�����ݒ�(BGM�ASE)�ȊO�̎�
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.OptionSetting) return;
        
        // �J�[�\���̈ړ����A�j���[�V�����Ȃ珈����Ԃ�
        if (optionCursorFrame.IsCursorMoveAnimation()) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }
        
        if (titleManager.GetCursorPressed().x == 1)
        {
            if (optionSelection == OptionSelection.BGM)
            {
                BGMVolumeUp();
            }
            else if (optionSelection == OptionSelection.SE)
            {
                SEVolumeUp();
            }
        }
        else if (titleManager.GetCursorPressed().x == -1)
        {
            if (optionSelection == OptionSelection.BGM)
            {
                BGMVolumeDown();
            }
            else if (optionSelection == OptionSelection.SE)
            {
                SEVolumeDown();
            }
        }
        else if(titleManager.GetCursorPressed().y == 1)
        {
            if (optionSelection == OptionSelection.BGM) return;
            optionSelection--;
            OptionSelectionChange(optionSelection);
        }
        else if (titleManager.GetCursorPressed().y == -1)
        {
            if (optionSelection == OptionSelection.Return) return;
            optionSelection++;
            OptionSelectionChange(optionSelection);
        }
        else if (titleManager.GetDecision())
        {
            OptionSettingReturn();
        }
    }

    // BGM�̉��ʂ��グ��
    public void BGMVolumeUp()
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // ���{�^����SE
        SESound.ArrowButtonSE();

        // �ő��BGM�̑傫�����ǂ���
        if (BGMVolume >= maxBGMVolume) return;
        ///+= value * 0.01f
        // ���ʂ��グ��
        BGMVolume += upDownValue;
        BGMSound.GetSetBGMVolume += upDownValue * 0.01f;
        BGMVolumeText.text = BGMVolume.ToString();

    }

    // BGM�̉��ʂ�������
    public void BGMVolumeDown()
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // ���{�^����SE
        SESound.ArrowButtonSE();

        // �Œ��BGM�̑傫�����ǂ���
        if (BGMVolume <= 0) return;

        // ���ʂ�������
        BGMVolume -= upDownValue;
        BGMSound.GetSetBGMVolume -= upDownValue * 0.01f;
        BGMVolumeText.text = BGMVolume.ToString();
    }

    // SE�̉��ʂ��グ��
    public void SEVolumeUp()
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // ���{�^����SE
        SESound.ArrowButtonSE();

        // �ő��SE�̑傫�����ǂ���
        if (SEVolume >= maxSEVolume) return;

        // ���ʂ��グ��
        SEVolume += upDownValue;
        SESound.GetSetSEVolume += upDownValue * 0.01f;
        SEVolumeText.text = SEVolume.ToString();
    }

    // SE�̉��ʂ�������
    public void SEVolumeDown()
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // ���{�^����SE
        SESound.ArrowButtonSE();

        // �Œ��SE�̑傫�����ǂ���
        if (SEVolume <= 0) return;

        // ���ʂ�������
        SEVolume -= upDownValue;
        SESound.GetSetSEVolume -= upDownValue * 0.01f;
        SEVolumeText.text = SEVolume.ToString();
    }

    /// <summary>
    /// �I�v�V�����̐ݒ荀�ڂ���߂�
    /// </summary>
    public void OptionSettingReturn()
    {
        if (optionSelection != OptionSelection.Return) return;

        // ���͖�������
        invalidTime = decisionInvalidTime;

        // ����{�^����SE
        SESound.DecisionButtonSE();

        optionSelection = OptionSelection.BGM;  // �I�v�V�����ݒ�̍��ڂ�߂�

        // �^�C�g���̉^�c�󋵂�߂�
        titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

        titleManager.DropDecision();    // ������͂���߂�

        // ��\����
        menuBackground.SetActive(false);
        optionSettingDisplay.SetActive(false);

        // �t���[���g�̈ʒu��߂�
        optionCursorFrame.gameObject.transform.position = BGMSetting.position;
    }

    /// <summary>
    /// �I�v�V�����̐ݒ荀�ڂ̕ύX
    /// </summary>
    private void OptionSelectionChange(OptionSelection optionSelection)
    {
        // ���͖�������
        invalidTime = inputInvalidTime;

        // �I�v�V�����̐ݒ荀�ڑI��
        switch (optionSelection)
        {
            // BGM
            case OptionSelection.BGM:
                break;

            // SE
            case OptionSelection.SE:
                break;

            // �߂�
            case OptionSelection.Return:
                break;

            default:
                Debug.LogError($"���ڂ��Ȃ��ł��B:" + optionSelection);
                break;
        }

        // �J�[�\�����ړ�������
        optionCursorFrame.CursorMove((int)optionSelection);
    }


}
