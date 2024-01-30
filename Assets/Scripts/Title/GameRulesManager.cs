using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームルールの管理スクリプト
/// </summary>
public class GameRulesManager : MonoBehaviour
{

    [SerializeField, Header("タイトルの管理スクリプト")]
    private TitleManager titleManager;

    [SerializeField, Header("メニューの背景")]
    private GameObject menuBackground;

    [SerializeField, Header("ゲームルール表示")]
    private GameObject gameRulesDisplay;

    [Header("無効時間")]
    [SerializeField, Tooltip("入力無効時間")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("決定無効時間")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // 無効時間

    // SEを参照するやつ
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // 入力無効時間
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // タイトルの運営状況がルール説明の選択項目以外の時
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.RuleExplanation) return;

        // 入力無効時間
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        // 決定
        if (titleManager.GetDecision())
        {
            GameRuleReturn();
        }
    }

    /// <summary>
    /// ゲームルールから戻る
    /// </summary>
    private void GameRuleReturn()
    {
        // 入力無効時間
        invalidTime = decisionInvalidTime;

        // 決定ボタンのSE
        SESound.DecisionButtonSE();

        // タイトルの運営状況を戻す
        titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

        titleManager.DropDecision();    // 決定入力をやめる

        // 非表示に
        menuBackground.SetActive(false);
        gameRulesDisplay.SetActive(false);
    }

}
