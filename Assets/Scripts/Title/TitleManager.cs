using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �^�C�g���̊Ǘ��X�N���v�g
/// </summary>
public class TitleManager : MonoBehaviour
{

    private PlayerInput playerInput;    // �v���C���[�̓���

    [SerializeField, Header("�ő�̐i�񂾋����e�L�X�g�̃I�u�W�F�N�g�Q��")]
    private TextMeshProUGUI maxAdvancedText;
    
    public static int gameMaxAdvanced;  // �Q�[���̍ő�̐i�s����

    private Vector2 cursorPressed;
    private bool decisionFlg;           // ����

    /// <summary>
    /// �^�C�g���̉^�c��
    /// </summary>
    public enum TitleOperationStatus
    { 
        MainItem,       // �^�C�g����ȍ���
        GameDifficulty, // �Q�[����Փx�̑I������
        OptionSetting,  // �I�v�V�����ݒ�(BGM�ASE)
        RuleExplanation,// ���[������
    }

    private TitleOperationStatus titleOperationStatus = TitleOperationStatus.MainItem;

    private void Awake()
    {
        // Action�X�N���v�g�̃C���X�^���X����
        playerInput = new PlayerInput();

        // Action�C�x���g�o�^
        playerInput.System.Cursor.started += OnCursorPressed;   // ���͂�0����0�ȊO�ɕω������Ƃ�
        playerInput.System.Cursor.canceled += OnCursorSeparate; // ���͂�0�ȊO����0�ɕω������Ƃ�
        playerInput.System.Decision.started += OnDecisionPressed;
        playerInput.System.Decision.canceled += OnDecisionSeparate;

        // Input Action���@�\�����邽�߂ɂ́A�L��������K�v������
        playerInput.Enable();

        // �ō����C���̐i�s�x�̃e�L�X�g
        maxAdvancedText.text = gameMaxAdvanced.ToString();
    }

    private void OnDestroy()
    {
        // ���g�ŃC���X�^���X������Action�N���X��IDisposable���������Ă���̂ŁA
        // �K��Dispose����K�v������
        playerInput?.Dispose();
    }

    /// <summary>
    /// �㉺���E�̂ǂꂩ��������
    /// </summary>
    private void OnCursorPressed(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();  // �A�N�V�����̓��͎擾
        cursorPressed = inputValue.normalized;
    }

    /// <summary>
    /// �㉺���E�̂ǂꂩ�𗣂�
    /// </summary>
    private void OnCursorSeparate(InputAction.CallbackContext context)
    {
        cursorPressed = Vector2.zero;
    }

    /// <summary>
    /// �����������
    /// </summary>
    private void OnDecisionPressed(InputAction.CallbackContext context)
    {
        decisionFlg = true;
    }

    /// <summary>
    /// ����𗣂�
    /// </summary>
    private void OnDecisionSeparate(InputAction.CallbackContext context)
    {
        decisionFlg = false;
    }

    /// <summary>
    /// �������߂�
    /// </summary>
    public void DropDecision()
    {
        decisionFlg = false;
    }

    /// <summary>
    /// �㉺���E�̓��͏󋵂�Ԃ�
    /// </summary>
    public Vector2 GetCursorPressed()
    {
        return cursorPressed;
    }

    /// <summary>
    /// ����̓��͏󋵂�Ԃ�
    /// </summary>
    public bool GetDecision()
    {
        return decisionFlg;
    }

    /// <summary>
    /// �^�C�g���̉^�c�󋵂�n��
    /// </summary>
    public TitleOperationStatus GetTitleOperationStatus()
    {
        return titleOperationStatus;
    }

    /// <summary>
    /// �^�C�g���̉^�c�󋵂�����
    /// </summary>
    public void SetTitleOperationStatus(TitleOperationStatus Status)
    {
        titleOperationStatus = Status;
    }

}
