using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム結果の管理スクリプト
/// </summary>
public class GameResult : MonoBehaviour
{

    [SerializeField, Header("メインの管理のスクリプト参照")]
    private MainManager mainManager;

    [SerializeField, Header("リザルトの背景")]
    private GameObject resultBackground;

    [SerializeField, Header("ゲームゴール時のリザルト")]
    private GameObject gameGoalResult;

    [SerializeField, Header("ゲームオーバーじの説明のオブジェクト参照")]
    private TextMeshProUGUI gameOverExplanationText;

    [SerializeField, Header("ゲームオーバー時のリザルト")]
    private GameObject gameOverResult;

    [SerializeField, Header("エンドレス時のリザルト表示")]
    private GameObject endlessDisplay;

    [SerializeField, Header("今回の進行テキスト参照")]
    private TextMeshProUGUI thisTimeAdvancedText;

    [SerializeField, Header("最大の進行テキスト参照")]
    private TextMeshProUGUI maxAdvancedText;

    private PlayerInput playerInput;    // プレイヤーの入力

    private Vector2 cursorPressed;
    private bool decisionFlg;           // 決定

    private void Awake()
    {
        // Actionスクリプトのインスタンス生成
        playerInput = new PlayerInput();

        // Actionイベント登録
        playerInput.System.Cursor.started += OnCursorPressed;   // 入力が0から0以外に変化したとき
        playerInput.System.Cursor.canceled += OnCursorSeparate; // 入力が0以外から0に変化したとき
        playerInput.System.Decision.started += OnDecisionPressed;
        playerInput.System.Decision.canceled += OnDecisionSeparate;

        // Input Actionを機能させるためには、有効化する必要がある
        playerInput.Enable();
    }

    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        playerInput?.Dispose();
    }

    /// <summary>
    /// 上下左右のどれかを押した
    /// </summary>
    private void OnCursorPressed(InputAction.CallbackContext context)
    {
        // ゲームプレイ状況が「結果」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        Vector2 inputValue = context.ReadValue<Vector2>();  // アクションの入力取得
        cursorPressed = inputValue.normalized;
    }

    /// <summary>
    /// 上下左右のどれかを離す
    /// </summary>
    private void OnCursorSeparate(InputAction.CallbackContext context)
    {
        // ゲームプレイ状況が「結果」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        cursorPressed = Vector2.zero;
    }

    /// <summary>
    /// 決定を押した
    /// </summary>
    private void OnDecisionPressed(InputAction.CallbackContext context)
    {
        // ゲームプレイ状況が「結果」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        decisionFlg = true;
    }

    /// <summary>
    /// 決定を離す
    /// </summary>
    private void OnDecisionSeparate(InputAction.CallbackContext context)
    {
        // ゲームプレイ状況が「結果」の時以外は返す
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        decisionFlg = false;
    }

    /// <summary>
    /// 決定をやめる
    /// </summary>
    public void DropDecision()
    {
        decisionFlg = false;
    }

    /// <summary>
    /// 上下左右の入力状況を返す
    /// </summary>
    public Vector2 GetCursorPressed()
    {
        return cursorPressed;
    }

    /// <summary>
    /// 決定の入力状況を返す
    /// </summary>
    public bool GetDecision()
    {
        return decisionFlg;
    }

    /// <summary>
    /// ゴールした時の結果を表示
    /// </summary>
    public void GameGoalResult()
    {
        // 表示
        resultBackground.SetActive(true);
        gameGoalResult.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバー時の結果を表示
    /// </summary>
    public void GameOverResult(string explanation)
    {
        // ゲームオーバーの説明
        gameOverExplanationText.text = explanation;

        // 表示
        resultBackground.SetActive(true);
        gameOverResult.SetActive(true);
    }

    /// <summary>
    /// 無限モードのゲームオーバー時の結果を表示
    /// </summary>
    public void EndlessGameOverResult(string explanation,int thisTimeAdvanced)
    {
        // ゲームオーバーの説明
        gameOverExplanationText.text = explanation;

        // 今回と最大の進行テキスト
        thisTimeAdvancedText.text = thisTimeAdvanced.ToString();
        maxAdvancedText.text = TitleManager.gameMaxAdvanced.ToString();

        // 表示
        resultBackground.SetActive(true);
        gameOverResult.SetActive(true);
        endlessDisplay.SetActive(true);
    }

}
