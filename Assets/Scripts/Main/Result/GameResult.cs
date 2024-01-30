using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[�����ʂ̊Ǘ��X�N���v�g
/// </summary>
public class GameResult : MonoBehaviour
{

    [SerializeField, Header("���C���̊Ǘ��̃X�N���v�g�Q��")]
    private MainManager mainManager;

    [SerializeField, Header("���U���g�̔w�i")]
    private GameObject resultBackground;

    [SerializeField, Header("�Q�[���S�[�����̃��U���g")]
    private GameObject gameGoalResult;

    [SerializeField, Header("�Q�[���I�[�o�[���̐����̃I�u�W�F�N�g�Q��")]
    private TextMeshProUGUI gameOverExplanationText;

    [SerializeField, Header("�Q�[���I�[�o�[���̃��U���g")]
    private GameObject gameOverResult;

    [SerializeField, Header("�G���h���X���̃��U���g�\��")]
    private GameObject endlessDisplay;

    [SerializeField, Header("����̐i�s�e�L�X�g�Q��")]
    private TextMeshProUGUI thisTimeAdvancedText;

    [SerializeField, Header("�ő�̐i�s�e�L�X�g�Q��")]
    private TextMeshProUGUI maxAdvancedText;

    private PlayerInput playerInput;    // �v���C���[�̓���

    private Vector2 cursorPressed;
    private bool decisionFlg;           // ����

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
        // �Q�[���v���C�󋵂��u���ʁv�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        Vector2 inputValue = context.ReadValue<Vector2>();  // �A�N�V�����̓��͎擾
        cursorPressed = inputValue.normalized;
    }

    /// <summary>
    /// �㉺���E�̂ǂꂩ�𗣂�
    /// </summary>
    private void OnCursorSeparate(InputAction.CallbackContext context)
    {
        // �Q�[���v���C�󋵂��u���ʁv�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        cursorPressed = Vector2.zero;
    }

    /// <summary>
    /// �����������
    /// </summary>
    private void OnDecisionPressed(InputAction.CallbackContext context)
    {
        // �Q�[���v���C�󋵂��u���ʁv�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        decisionFlg = true;
    }

    /// <summary>
    /// ����𗣂�
    /// </summary>
    private void OnDecisionSeparate(InputAction.CallbackContext context)
    {
        // �Q�[���v���C�󋵂��u���ʁv�̎��ȊO�͕Ԃ�
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

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
    /// �S�[���������̌��ʂ�\��
    /// </summary>
    public void GameGoalResult()
    {
        // �\��
        resultBackground.SetActive(true);
        gameGoalResult.SetActive(true);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���̌��ʂ�\��
    /// </summary>
    public void GameOverResult(string explanation)
    {
        // �Q�[���I�[�o�[�̐���
        gameOverExplanationText.text = explanation;

        // �\��
        resultBackground.SetActive(true);
        gameOverResult.SetActive(true);
    }

    /// <summary>
    /// �������[�h�̃Q�[���I�[�o�[���̌��ʂ�\��
    /// </summary>
    public void EndlessGameOverResult(string explanation,int thisTimeAdvanced)
    {
        // �Q�[���I�[�o�[�̐���
        gameOverExplanationText.text = explanation;

        // ����ƍő�̐i�s�e�L�X�g
        thisTimeAdvancedText.text = thisTimeAdvanced.ToString();
        maxAdvancedText.text = TitleManager.gameMaxAdvanced.ToString();

        // �\��
        resultBackground.SetActive(true);
        gameOverResult.SetActive(true);
        endlessDisplay.SetActive(true);
    }

}
