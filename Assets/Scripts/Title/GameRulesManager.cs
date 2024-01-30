using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[�����[���̊Ǘ��X�N���v�g
/// </summary>
public class GameRulesManager : MonoBehaviour
{

    [SerializeField, Header("�^�C�g���̊Ǘ��X�N���v�g")]
    private TitleManager titleManager;

    [SerializeField, Header("���j���[�̔w�i")]
    private GameObject menuBackground;

    [SerializeField, Header("�Q�[�����[���\��")]
    private GameObject gameRulesDisplay;

    [Header("��������")]
    [SerializeField, Tooltip("���͖�������")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("���薳������")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // ��������

    // SE���Q�Ƃ�����
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // ���͖�������
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�g���̉^�c�󋵂����[�������̑I�����ڈȊO�̎�
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.RuleExplanation) return;

        // ���͖�������
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        // ����
        if (titleManager.GetDecision())
        {
            GameRuleReturn();
        }
    }

    /// <summary>
    /// �Q�[�����[������߂�
    /// </summary>
    private void GameRuleReturn()
    {
        // ���͖�������
        invalidTime = decisionInvalidTime;

        // ����{�^����SE
        SESound.DecisionButtonSE();

        // �^�C�g���̉^�c�󋵂�߂�
        titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

        titleManager.DropDecision();    // ������͂���߂�

        // ��\����
        menuBackground.SetActive(false);
        gameRulesDisplay.SetActive(false);
    }

}
