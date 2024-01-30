using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�C�o�[�U�����o�Ă��铹�̊Ǘ��v���W�F�N�g
/// </summary>
public class CyberAttackRoadManager : MonoBehaviour
{
    [SerializeField, Header("�T�C�o�[�U���G�t�F�N�g")]
    private GameObject[] cyberAttackEffect = new GameObject[4];

    [Header("�T�C�o�[�U���̃|�C���g�G�t�F�N�g")]
    [SerializeField] private GameObject[] cyberAttackPointEffect_1 = new GameObject[4];
    [SerializeField] private GameObject[] cyberAttackPointEffect_2 = new GameObject[4];
    [SerializeField] private GameObject[] cyberAttackPointEffect_3 = new GameObject[4];

    private int minOccurrenceVal = -4;          // �ŏ��������W�̒l
    private int maxOccurrenceVal = 4;           // �ő唭�����W�̒l

    private int[] occurrenceVal = new int[4];   // �������W�̒l

    [Header("�T�C�o�[�U���\���֌W")]
    [SerializeField, Tooltip("�T�C�o�[�U���̍ŏ��������Ԃ̊Ԋu")]
    private float minCyberGenerationTime = 4.0f;

    [SerializeField, Tooltip("�T�C�o�[�U���̍ő唭�����Ԃ̊Ԋu")]
    private float maxCyberGenerationTime = 6.0f;

    private float[] cyberGenerationTime = new float[4];    // ��������

    [SerializeField, Tooltip("�T�C�o�[�U���\���̎���")]
    private float cyberDisplayTime = 1.5f;

    private float[] cyberTime = new float[4];  // �T�C�o�[�U���̕\������

    // Start is called before the first frame update
    void Start()
    {        
        for (int i = 0; i < cyberAttackEffect.Length; i++)
        {
            // �T�C�o�[�U���̔������W�̒l
            occurrenceVal[i] = Random.Range(minOccurrenceVal, maxOccurrenceVal + 1);
            // ���̔������Ԃ�����
            cyberGenerationTime[i] = Random.Range(minCyberGenerationTime, maxCyberGenerationTime + 1.0f);
            //�T�C�o�[�U���\���̎��Ԃ�����
            cyberTime[i] = cyberDisplayTime;
        }


    }

    // Update is called once per frame
    void Update()
    {
        // �T�C�o�[�U���̔���
        for (int i = 0; i < cyberAttackEffect.Length; i++) 
        {
            // �T�C�o�[�U���\��
            if (cyberAttackEffect[i].activeSelf)
            {
                // �T�C�o�[�U���̕\������
                if (cyberTime[i] < 0.0f)
                {
                    // �T�C�o�[�U���\���̎��Ԃ�����
                    cyberTime[i] = cyberDisplayTime;

                    // �T�C�o�[�U���G�t�F�N�g�̔�\��
                    cyberAttackEffect[i].SetActive(false);
                }
                else
                {
                    cyberTime[i] -= Time.deltaTime;
                }
            }
            // �T�C�o�[�U����\��
            else
            {
                // �T�C�o�[�U���̔�������
                if (cyberGenerationTime[i] < 0.0f)
                {
                    // ���̔������Ԃ�����
                    cyberGenerationTime[i] = Random.Range(minCyberGenerationTime, maxCyberGenerationTime + 1.0f);

                    // �T�C�o�[�U���̃|�C���g�̔�\��
                    cyberAttackPointEffect_1[i].SetActive(false);
                    cyberAttackPointEffect_2[i].SetActive(false);
                    cyberAttackPointEffect_3[i].SetActive(false);

                    // �T�C�o�[�U���G�t�F�N�g�̕\��
                    // �������W������
                    cyberAttackEffect[i].transform.localPosition = new Vector3(occurrenceVal[i], 1f, 0f);
                    cyberAttackEffect[i].SetActive(true);

                    // �T�C�o�[�U���̔������W�̒l
                    occurrenceVal[i] = Random.Range(minOccurrenceVal, maxOccurrenceVal + 1);
                }
                else
                {
                    cyberGenerationTime[i] -= Time.deltaTime;

                    // �T�C�o�[�U���̃|�C���g�̕\���A��\��
                    if (cyberGenerationTime[i] < 1.0f)
                    {
                        cyberAttackPointEffect_1[i].SetActive(false);
                        cyberAttackPointEffect_2[i].SetActive(false);
                        cyberAttackPointEffect_3[i].SetActive(true);
                    }
                    else if (cyberGenerationTime[i] < 2f)
                    {
                        cyberAttackPointEffect_1[i].SetActive(false);
                        cyberAttackPointEffect_2[i].SetActive(true);
                        cyberAttackPointEffect_3[i].SetActive(false);
                    }
                    else if (cyberGenerationTime[i] < 3f)
                    {
                        // �������W������
                        cyberAttackPointEffect_1[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);
                        cyberAttackPointEffect_2[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);
                        cyberAttackPointEffect_3[i].transform.localPosition = new Vector3(occurrenceVal[i], 0f, 0f);

                        cyberAttackPointEffect_1[i].SetActive(true);
                        cyberAttackPointEffect_2[i].SetActive(false);
                        cyberAttackPointEffect_3[i].SetActive(false);
                    }
                }
            }
        }


    }
}
