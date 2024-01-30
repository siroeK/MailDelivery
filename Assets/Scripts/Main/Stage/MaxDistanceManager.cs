using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MainManager;

/// <summary>
/// �ő�̐i�s�����̃v���W�F�N�g
/// </summary>
public class MaxDistanceManager : MonoBehaviour
{

    [SerializeField, Header("�v���C���[�̃I�u�W�F�N�g�Q��")]
    private Transform player;

    [SerializeField, Header("�ő�̐i�񂾋����Q��")]
    private Transform maxDistanceCanvas;

    [SerializeField, Header("�ő�̐i�񂾋����e�L�X�g�Q��")]
    private TextMeshProUGUI maxDistanceText;

    private bool distanceUpdateFlg = false;     // �����̍X�V

    // SE���Q�Ƃ�����
    private SESound SESound;

    // Start is called before the first frame update
    void Start()
    {
        // ���W���X�V
        maxDistanceCanvas.localPosition = new Vector3(0f, maxDistanceCanvas.localPosition.y, TitleManager.gameMaxAdvanced);

        maxDistanceText.text = "�ō����C���F" + TitleManager.gameMaxAdvanced.ToString();  // �ő�̐i�񂾋����e�L�X�g�\��

        // SE�̃X�N���v�g�Q��
        SESound = GameObject.FindWithTag("SE").GetComponent<SESound>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceUpdateFlg) return;

        if(maxDistanceCanvas.localPosition.z < player.localPosition.z)
        {
            // ��������SE
            SESound.SuccessSoundSE();

            distanceUpdateFlg = true;
        }

    }
}
