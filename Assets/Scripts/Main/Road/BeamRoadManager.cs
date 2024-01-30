using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CompositeCollider2D;

/// <summary>
/// �r�[�����o�Ă��铹�̊Ǘ��v���W�F�N�g
/// </summary>
public class BeamRoadManager : MonoBehaviour
{

    [SerializeField, Header("�r�[���G�t�F�N�g")]
    private GameObject beamEffect;

    [SerializeField, Header("��")]
    private MeshRenderer floor;

    [SerializeField, Header("���̃}�e���A��")] 
    private Material[] floorMaterialArray = new Material[2];

    [Header("�r�[���\���֌W")]
    [SerializeField, Tooltip("�r�[���̍ŏ��������Ԃ̊Ԋu")]
    private float minBeamGenerationTime = 4.0f;

    [SerializeField, Tooltip("�r�[���̍ő唭�����Ԃ̊Ԋu")]
    private float maxBeamGenerationTime = 6.0f;

    private float beamGenerationTime = 0.0f;    // ��������

    [SerializeField, Tooltip("�r�[���\���̎���")]
    private float beamDisplayTime = 1.5f;

    private float beamTime = 0f;  // �r�[���̎���


    private void Awake()
    {
        // �������Ԃ�����
        beamGenerationTime = Random.Range(minBeamGenerationTime, maxBeamGenerationTime + 1.0f);

        //�r�[���\���̎��Ԃ�����
        beamTime = beamDisplayTime;
    }

    // Update is called once per frame
    void Update()
    {
        // �r�[���G�t�F�N�g�\��
        if (beamEffect.activeSelf)
        {
            // �r�[���̕\������
            if (beamTime < 0.0f)
            {
                //�r�[���\���̎��Ԃ�����
                beamTime = beamDisplayTime;

                // ����material��߂�
                floor.material = floorMaterialArray[0];

                // �r�[���G�t�F�N�g�̔�\��
                beamEffect.SetActive(false);
            }
            else
            {
                beamTime -= Time.deltaTime;
            }
        }
        // �r�[���G�t�F�N�g��\��
        else
        {
            // �r�[���̔�������
            if (beamGenerationTime <= 0.0f)
            {
                // ���̔������Ԃ�����
                beamGenerationTime = Random.Range(minBeamGenerationTime, maxBeamGenerationTime + 1.0f);

                // ����material��؂�ւ���
                floor.material = floorMaterialArray[3];

                // �r�[���G�t�F�N�g�̕\��
                beamEffect.SetActive(true);
            }
            else
            {
                beamGenerationTime -= Time.deltaTime;

                // ����material��؂�ւ���
                if (beamGenerationTime < 0.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if (beamGenerationTime < 1f)
                {
                    floor.material = floorMaterialArray[3];
                }
                else if (beamGenerationTime < 1.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if(beamGenerationTime < 2f)
                {
                    floor.material = floorMaterialArray[2];
                }
                else if (beamGenerationTime < 2.5f)
                {
                    floor.material = floorMaterialArray[0];
                }
                else if (beamGenerationTime < 3f)
                {
                    floor.material = floorMaterialArray[1];
                }

            }
        }
    }
}
