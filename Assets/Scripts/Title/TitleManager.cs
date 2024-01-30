using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// タイトルの管理スクリプト
/// </summary>
public class TitleManager : MonoBehaviour
{

    private PlayerInput playerInput;    // プレイヤーの入力

    [SerializeField, Header("最大の進んだ距離テキストのオブジェクト参照")]
    private TextMeshProUGUI maxAdvancedText;
    
    public static int gameMaxAdvanced;  // ゲームの最大の進行距離

    private Vector2 cursorPressed;
    private bool decisionFlg;           // 決定

    /// <summary>
    /// タイトルの運営状況
    /// </summary>
    public enum TitleOperationStatus
    { 
        MainItem,       // タイトル主な項目
        GameDifficulty, // ゲーム難易度の選択項目
        OptionSetting,  // オプション設定(BGM、SE)
        RuleExplanation,// ルール説明
    }

    private TitleOperationStatus titleOperationStatus = TitleOperationStatus.MainItem;

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

        // 最高ラインの進行度のテキスト
        maxAdvancedText.text = gameMaxAdvanced.ToString();
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
        Vector2 inputValue = context.ReadValue<Vector2>();  // アクションの入力取得
        cursorPressed = inputValue.normalized;
    }

    /// <summary>
    /// 上下左右のどれかを離す
    /// </summary>
    private void OnCursorSeparate(InputAction.CallbackContext context)
    {
        cursorPressed = Vector2.zero;
    }

    /// <summary>
    /// 決定を押した
    /// </summary>
    private void OnDecisionPressed(InputAction.CallbackContext context)
    {
        decisionFlg = true;
    }

    /// <summary>
    /// 決定を離す
    /// </summary>
    private void OnDecisionSeparate(InputAction.CallbackContext context)
    {
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
    /// タイトルの運営状況を渡す
    /// </summary>
    public TitleOperationStatus GetTitleOperationStatus()
    {
        return titleOperationStatus;
    }

    /// <summary>
    /// タイトルの運営状況を入れる
    /// </summary>
    public void SetTitleOperationStatus(TitleOperationStatus Status)
    {
        titleOperationStatus = Status;
    }

}
