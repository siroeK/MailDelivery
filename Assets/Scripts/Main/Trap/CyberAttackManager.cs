using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サイバー攻撃の管理プロジェクト
/// </summary>
public class CyberAttackManager : MonoBehaviour
{
    [Header("サイバー攻撃のゲームオーバー理由文章")]
    [SerializeField, Tooltip("サイバー攻撃のゲームオーバー理由文章"), Multiline(2)]
    private string enemyExplanationText = "サイバー攻撃が発生し、\n不正アクセスを受けました。";

    /// <summary>
    /// サイバー攻撃のゲームオーバー理由文章
    /// </summary>
    public string GetBeamExplanation()
    {
        return enemyExplanationText;
    }
}
