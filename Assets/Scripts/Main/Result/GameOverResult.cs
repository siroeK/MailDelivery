using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー時の結果を表示
/// </summary>
public class GameOverResult : MonoBehaviour
{

    [SerializeField, Header("メインの管理の参照")]
    private MainManager mainManager;

    [SerializeField, Header("ゲーム結果の参照")]
    private GameResult gameResult;

    [SerializeField, Header("ゲームオーバー時の結果のフレーム枠")]
    private CursorDOTweenAnimation gameOverFrame;

    [Header("無効時間")]
    [SerializeField, Tooltip("入力無効時間")]
    private float inputInvalidTime = 0.2f;
    [SerializeField, Tooltip("決定無効時間")]
    private float decisionInvalidTime = 0.2f;

    private float invalidTime = 0f;   // 無効時間

    // SEを参照するやつ
    private SESound SESound;

    /// <summary>
    /// ゲームオーバー時の結果を表示の項目選択
    /// </summary>
    private enum GameOverItem
    {
        Restart = 0,        // ゲームリスタート
        TitleReturn = 1,    // タイトルへ戻る
    }

    private GameOverItem gameOverItem = GameOverItem.Restart;

    // Start is called before the first frame update
    void Start()
    {
        // SEのスクリプト参照
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        gameOverItem = GameOverItem.Restart;

        // 入力無効時間
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームプレイ状況が「結果」の選択項目以外の時
        if (mainManager.GetGamePlayStatus() != MainManager.GamePlayStatus.Result) return;

        // カーソルの移動中アニメーションなら処理を返す
        if (gameOverFrame.IsCursorMoveAnimation()) return;

        // 入力無効時間
        if (invalidTime > 0f)
        {
            invalidTime -= Time.deltaTime;
            return;
        }

        if (gameResult.GetCursorPressed().y == 1)
        {
            if (gameOverItem == GameOverItem.Restart) return;
            gameOverItem--;
            GameOverItemChange(gameOverItem);
        }
        else if (gameResult.GetCursorPressed().y == -1)
        {
            if (gameOverItem == GameOverItem.TitleReturn) return;
            gameOverItem++;
            GameOverItemChange(gameOverItem);
        }
        // 決定
        else if (gameResult.GetDecision())
        {
            GameOverItemDecision(gameOverItem);
        }
    }

    /// <summary>
    /// ゲームオーバー時の結果の項目変更
    /// </summary>
    private void GameOverItemChange(GameOverItem gameOver)
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // ゲームオーバー時の結果の項目選択
        switch (gameOver)
        {
            // ゲームリスタート
            case GameOverItem.Restart:
                break;

            // タイトルへ戻る
            case GameOverItem.TitleReturn:
                break;

            default:
                Debug.LogError($"項目がないです。:" + gameOver);
                break;
        }

        // カーソルを移動させる
        gameOverFrame.CursorMove((int)gameOver);
    }

    /// <summary>
    /// ゲームオーバー時の項目の決定
    /// </summary>
    private void GameOverItemDecision(GameOverItem gameOver)
    {
        // 入力無効時間
        invalidTime = decisionInvalidTime;

        // ゴールした時の結果の項目選択
        switch (gameOver)
        {
            // ゲームリスタート
            case GameOverItem.Restart:
                // 決定ボタンのSE
                SESound.DecisionButtonSE();
                // リスタート
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            // タイトルへ戻る
            case GameOverItem.TitleReturn:
                // 決定ボタンのSE
                SESound.DecisionButtonSE();
                // タイトルシーンへ
                SceneManager.LoadScene("Title");
                break;

            default:
                Debug.LogError($"項目がないです。:" + gameOver);
                break;
        }
    }

}
