using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルの主な項目選択の管理スクリプト
/// </summary>
public class TitleItemSelection : MonoBehaviour
{

    [SerializeField, Header("項目のフレーム枠")]
    private CursorDOTweenAnimation itemCursorFrame;

    [SerializeField, Header("タイトルの管理スクリプト")]
    private TitleManager titleManager;

    [SerializeField, Header("メニューの背景")]
    private GameObject menuBackground;

    [SerializeField, Header("ゲーム難易度表示")]
    private GameObject gameDifficultyDisplay;

    [SerializeField, Header("オプション設定表示")]
    private GameObject optionSettingDisplay;

    [SerializeField, Header("ゲームルール表示")]
    private GameObject gameRulesDisplay;

    [SerializeField, Header("入力無効時間")]
    private float inputInvalidTime = 0.25f;
    [SerializeField, Header("決定無効時間")]
    private float decisionInvalidTime = 0.5f;

    private float invalidTime = 0f;   // 無効時間

    // SEを参照するやつ
    private SESound SEManager;

    /// <summary>
    /// タイトル主な項目選択
    /// </summary>
    private enum MainItemSelection
    {
        GameStart = 0,      // ゲームスタート
        SoundSetting = 1,   // サウンド設定(BGM、SE)
        GameRules = 2,      // ルール説明
    }

    private MainItemSelection mainItemSelection = MainItemSelection.GameStart;

    void Awake()
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // SEのスクリプト参照
        SEManager = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    // Update is called once per frame
    private void Update()
    {
        // タイトルの運営状況がタイトル主な項目以外の時
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.MainItem) return;

        // カーソルの移動中アニメーションなら処理を返す
        if (itemCursorFrame.IsCursorMoveAnimation()) return;

        // 入力無効時間
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
    /// タイトル主な項目の変更
    /// </summary>
    private void MainItemSelectionChange(MainItemSelection itemSelection)
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // タイトル主な項目選択
        switch (itemSelection) 
        {
            // ゲームスタート
            case MainItemSelection.GameStart:
                break;

            // サウンド設定(BGM、SE)
            case MainItemSelection.SoundSetting:
                break;

            // ルール説明
            case MainItemSelection.GameRules:
                break;

            default:
                Debug.LogError($"項目がないです。:" + itemSelection);
                break;
        }

        // カーソルを移動させる
        itemCursorFrame.CursorMove((int)itemSelection);
    }

    /// <summary>
    /// タイトル主な項目の決定
    /// </summary>
    private void MainItemDecision(MainItemSelection itemSelection)
    {
        // 入力無効時間
        invalidTime = decisionInvalidTime;

        // 決定ボタンのSE
        SEManager.DecisionButtonSE();

        // タイトル主な項目選択
        switch (itemSelection)
        {
            // ゲームスタート
            case MainItemSelection.GameStart:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.GameDifficulty);
                titleManager.DropDecision();    // 決定入力をやめる
                menuBackground.SetActive(true);
                gameDifficultyDisplay.SetActive(true);
                break;

            // サウンド設定(BGM、SE)
            case MainItemSelection.SoundSetting:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.OptionSetting);
                titleManager.DropDecision();    // 決定入力をやめる
                menuBackground.SetActive(true);
                optionSettingDisplay.SetActive(true);
                break;

            // ルール説明
            case MainItemSelection.GameRules:
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.RuleExplanation);
                titleManager.DropDecision();    // 決定入力をやめる
                menuBackground.SetActive(true);
                gameRulesDisplay.SetActive(true);
                break;

            default:
                Debug.LogError($"項目がないです。:" + itemSelection);
                break;
        }
    }
}
