using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム難易度の項目選択の管理スクリプト
/// </summary>
public class GameDifficultySelection : MonoBehaviour
{

    [SerializeField, Header("タイトルの管理スクリプト")]
    private TitleManager titleManager;

    [SerializeField, Header("メニューの背景")]
    private GameObject menuBackground;

    [SerializeField, Header("ゲーム難易度表示")]
    private GameObject gameDifficultyDisplay;

    [SerializeField, Header("難易度設定のオブジェクト参照")]
    private Transform difficultySetting;

    [SerializeField, Header("難易度設定のオブジェクト参照")]
    private TextMeshProUGUI difficultyText;

    [SerializeField, Header("難易度説明のオブジェクト参照")]
    private TextMeshProUGUI explanationDifficultyText;
    
    [SerializeField, Header("ゲーム難易度項目のフレーム枠")]
    private CursorDOTweenAnimation difficultyCursorFrame;

    [Header("難易度テキスト")]
    [SerializeField, Tooltip("簡単な難易度"), Multiline(1)]
    private string easyDifficultyText = "簡単";
    [SerializeField, Tooltip("普通の難易度"), Multiline(1)]
    private string normalDifficultyText = "普通";
    [SerializeField, Tooltip("無限な難易度"), Multiline(1)]
    private string endlessDifficultyText = "無限";

    [Header("説明難易度テキスト")]
    [SerializeField, Tooltip("簡単な難易度の説明"), Multiline(3)]
    private string easyExplanationText = "簡単なモード\n15ラインでクリアです。";
    [SerializeField, Tooltip("普通の難易度の説明"), Multiline(3)]
    private string normalExplanationText = "普通モード\n30ラインでクリアです。";
    [SerializeField, Tooltip("無限な難易度の説明"), Multiline(3)]
    private string endlessExplanationText = "無限モード\nゲームオーバーに\nなるまで続けれます。";

    [Header("無効時間")]
    [SerializeField, Tooltip("入力無効時間")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("決定無効時間")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // 無効時間

    // SEを参照するやつ
    private SESound SESound;

    /// <summary>
    /// ゲーム難易度の選択
    /// </summary>
    private enum DifficultySelection
    {
        Easy = 0,       // 簡単
        Normal = 1,     // 普通
        Endless = 2,    // 無限
    }

    private DifficultySelection difficultySelection = DifficultySelection.Easy;

    /// <summary>
    /// 難易度表示の項目選択
    /// </summary>
    private enum DifficultyItem
    {
        Difficulty = 0, // ゲーム難易度
        GameStart = 1,  // ゲームスタート
        Return = 2,     // 戻る
    }

    private DifficultyItem difficultyItem = DifficultyItem.Difficulty;

    // Start is called before the first frame update
    void Start()
    {
        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        // 難易度の説明文を入れる
        difficultyText.text = easyDifficultyText;
        explanationDifficultyText.text = easyExplanationText;

        // 入力無効時間
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // タイトルの運営状況がゲーム難易度の選択項目以外の時
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.GameDifficulty) return;

        // カーソルの移動中アニメーションなら処理を返す
        if (difficultyCursorFrame.IsCursorMoveAnimation()) return;

        // 入力無効時間
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
        // 決定
        else if (titleManager.GetDecision())
        {
            DifficultyItemDecision(difficultyItem);
        }
    }

    /// <summary>
    /// ゲーム難易度の変更
    /// </summary>
    private void DifficultyChange(DifficultySelection difficulty)
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // ゲーム難易度選択
        switch (difficulty)
        {
            // 簡単
            case DifficultySelection.Easy:
                // 簡単の難易度の説明文を入れる
                difficultyText.text = easyDifficultyText;
                explanationDifficultyText.text = easyExplanationText;
                break;

            // 普通
            case DifficultySelection.Normal:
                // 普通の難易度の説明文を入れる
                difficultyText.text = normalDifficultyText;
                explanationDifficultyText.text = normalExplanationText;
                break;

            // 無限
            case DifficultySelection.Endless:
                // 無限の難易度の説明文を入れる
                difficultyText.text = endlessDifficultyText;
                explanationDifficultyText.text = endlessExplanationText;
                break;

            default:
                Debug.LogError($"項目がないです。:" + difficulty);
                break;
        }

        // 矢印ボタンのSE
        SESound.ArrowButtonSE();

    }

    /// <summary>
    /// ゲーム難易度の変更
    /// </summary>
    private void DifficultyItemChange(DifficultyItem difficultyItem)
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // ゲーム難易度の主な項目選択
        switch (difficultyItem)
        {
            // ゲーム難易度
            case DifficultyItem.Difficulty:
                break;

            // ゲームスタート
            case DifficultyItem.GameStart:
                break;

            // 戻る
            case DifficultyItem.Return:
                break;

            default:
                Debug.LogError($"項目がないです。:" + difficultyItem);
                break;
        }

        // カーソルを移動させる
        difficultyCursorFrame.CursorMove((int)difficultyItem);
    }

    /// <summary>
    /// ゲーム難易度の主な項目の決定
    /// </summary>
    private void DifficultyItemDecision(DifficultyItem decisionItem)
    {
        // 入力無効時間
        invalidTime = decisionInvalidTime;

        // ゲーム難易度の主な項目選択
        switch (decisionItem)
        {
            // ゲーム難易度
            case DifficultyItem.Difficulty:
                break;

            // ゲームスタート
            case DifficultyItem.GameStart:

                // 入力無効時間
                invalidTime = decisionInvalidTime;

                // 決定ボタンのSE
                SESound.DecisionButtonSE();

                // ゲーム難易度によってシーンを変える
                switch (difficultySelection)
                {
                    // 簡単
                    case DifficultySelection.Easy:
                        // 簡単なゲームシーンへ
                        SceneManager.LoadScene("EasyMain");
                        break;

                    // 普通
                    case DifficultySelection.Normal:
                        // 普通なゲームシーンへ
                        SceneManager.LoadScene("NormalMain");
                        break;

                    // 無限
                    case DifficultySelection.Endless:
                        // 無限なゲームシーンへ
                        SceneManager.LoadScene("EndlessMain");
                        break;

                    default:
                        Debug.LogError($"項目がないです。:" + difficultySelection);
                        break;
                }

                break;

            // 戻る
            case DifficultyItem.Return:

                // 入力無効時間
                invalidTime = decisionInvalidTime;

                // 決定ボタンのSE
                SESound.DecisionButtonSE();

                // 設定の項目を戻す
                difficultySelection = DifficultySelection.Easy;
                difficultyItem = DifficultyItem.Difficulty;

                // 難易度の説明文を入れる
                difficultyText.text = easyDifficultyText;
                explanationDifficultyText.text = easyExplanationText;

                // タイトルの運営状況を戻す
                titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

                titleManager.DropDecision();    // 決定入力をやめる

                // 非表示に
                menuBackground.SetActive(false);
                gameDifficultyDisplay.SetActive(false);

                // フレーム枠の位置を戻す
                difficultyCursorFrame.gameObject.transform.position = difficultySetting.position;

                break;

            default:
                Debug.LogError($"項目がないです。:" + decisionItem);
                break;
        }
    }

}
