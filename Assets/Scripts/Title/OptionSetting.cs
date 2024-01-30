using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// オプションの設定スクリプト(BGM・SE)
/// </summary>
public class OptionSetting : MonoBehaviour
{

    [SerializeField, Header("BGMの音量テキスト")]
    private TextMeshProUGUI BGMVolumeText;

    [SerializeField, Header("SEの音量テキスト")]
    private TextMeshProUGUI SEVolumeText;

    [SerializeField, Header("タイトルの管理スクリプト")]
    private TitleManager titleManager;

    [SerializeField, Header("メニューの背景")]
    private GameObject menuBackground;

    [SerializeField, Header("オプション設定表示")]
    private GameObject optionSettingDisplay;

    [SerializeField, Header("BGMの設定オブジェクト参照")]
    private Transform BGMSetting;

    [SerializeField, Header("項目のフレーム枠")]
    private CursorDOTweenAnimation optionCursorFrame;

    // BGMとSEを参照するやつ
    private BGMSound BGMSound;
    private SESound SESound;

    [SerializeField, Header("音量")]
    public int upDownValue = 10;        // 上下させる値

    public int maxBGMVolume = 100;      // 最大のBGMの大きさ
    public static int BGMVolume = 50;   // BGMの大きさ
    public int maxSEVolume = 100;       // 最大のSEの大きさ
    public static int SEVolume = 50;    // SEの大きさ

    [SerializeField, Header("入力無効時間")]
    private float inputInvalidTime = 0.25f;
    [SerializeField, Header("決定無効時間")]
    private float decisionInvalidTime = 0.5f;

    private float invalidTime = 0f;   // 無効時間


    /// <summary>
    /// オプション設定の項目選択
    /// </summary>
    private enum OptionSelection
    {
        BGM = 0,    // BGM
        SE = 1,     // SE
        Return = 2, // 戻る
    }

    private OptionSelection optionSelection = OptionSelection.BGM;

    // Start is called before the first frame update
    void Start()
    {
        // BGM・SEのスクリプト参照
        BGMSound = GameObject.FindWithTag("BGM").GetComponent<BGMSound>();
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();

        BGMVolumeText.text = BGMVolume.ToString();
        SEVolumeText.text = SEVolume.ToString();

        // 入力無効時間
        invalidTime = inputInvalidTime;
    }

    // Update is called once per frame
    private void Update()
    {
        // タイトルの運営状況がオプション設定(BGM、SE)以外の時
        if (titleManager.GetTitleOperationStatus() != TitleManager.TitleOperationStatus.OptionSetting) return;
        
        // カーソルの移動中アニメーションなら処理を返す
        if (optionCursorFrame.IsCursorMoveAnimation()) return;

        // 入力無効時間
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

    // BGMの音量を上げる
    public void BGMVolumeUp()
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // 矢印ボタンのSE
        SESound.ArrowButtonSE();

        // 最大のBGMの大きさかどうか
        if (BGMVolume >= maxBGMVolume) return;
        ///+= value * 0.01f
        // 音量を上げる
        BGMVolume += upDownValue;
        BGMSound.GetSetBGMVolume += upDownValue * 0.01f;
        BGMVolumeText.text = BGMVolume.ToString();

    }

    // BGMの音量を下げる
    public void BGMVolumeDown()
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // 矢印ボタンのSE
        SESound.ArrowButtonSE();

        // 最低のBGMの大きさかどうか
        if (BGMVolume <= 0) return;

        // 音量を下げる
        BGMVolume -= upDownValue;
        BGMSound.GetSetBGMVolume -= upDownValue * 0.01f;
        BGMVolumeText.text = BGMVolume.ToString();
    }

    // SEの音量を上げる
    public void SEVolumeUp()
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // 矢印ボタンのSE
        SESound.ArrowButtonSE();

        // 最大のSEの大きさかどうか
        if (SEVolume >= maxSEVolume) return;

        // 音量を上げる
        SEVolume += upDownValue;
        SESound.GetSetSEVolume += upDownValue * 0.01f;
        SEVolumeText.text = SEVolume.ToString();
    }

    // SEの音量を下げる
    public void SEVolumeDown()
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // 矢印ボタンのSE
        SESound.ArrowButtonSE();

        // 最低のSEの大きさかどうか
        if (SEVolume <= 0) return;

        // 音量を下げる
        SEVolume -= upDownValue;
        SESound.GetSetSEVolume -= upDownValue * 0.01f;
        SEVolumeText.text = SEVolume.ToString();
    }

    /// <summary>
    /// オプションの設定項目から戻る
    /// </summary>
    public void OptionSettingReturn()
    {
        if (optionSelection != OptionSelection.Return) return;

        // 入力無効時間
        invalidTime = decisionInvalidTime;

        // 決定ボタンのSE
        SESound.DecisionButtonSE();

        optionSelection = OptionSelection.BGM;  // オプション設定の項目を戻す

        // タイトルの運営状況を戻す
        titleManager.SetTitleOperationStatus(TitleManager.TitleOperationStatus.MainItem);

        titleManager.DropDecision();    // 決定入力をやめる

        // 非表示に
        menuBackground.SetActive(false);
        optionSettingDisplay.SetActive(false);

        // フレーム枠の位置を戻す
        optionCursorFrame.gameObject.transform.position = BGMSetting.position;
    }

    /// <summary>
    /// オプションの設定項目の変更
    /// </summary>
    private void OptionSelectionChange(OptionSelection optionSelection)
    {
        // 入力無効時間
        invalidTime = inputInvalidTime;

        // オプションの設定項目選択
        switch (optionSelection)
        {
            // BGM
            case OptionSelection.BGM:
                break;

            // SE
            case OptionSelection.SE:
                break;

            // 戻る
            case OptionSelection.Return:
                break;

            default:
                Debug.LogError($"項目がないです。:" + optionSelection);
                break;
        }

        // カーソルを移動させる
        optionCursorFrame.CursorMove((int)optionSelection);
    }


}
