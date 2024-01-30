using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ビームの管理プロジェクト
/// </summary>
public class BeamManager : MonoBehaviour
{
    [Header("ビームのゲームオーバー理由文章")]
    [SerializeField, Tooltip("ビームのゲームオーバー理由文章"), Multiline(2)]
    private string enemyExplanationText = "電磁パルスで通信が中断され、\nメールが途切れてしまった。";

    /// <summary>
    /// ビームのゲームオーバー理由文章
    /// </summary>
    public string GetBeamExplanation()
    {
        return enemyExplanationText;
    }
}
