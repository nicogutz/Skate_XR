using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(ObjectSpawner))]
public class SpawnedObjectsManager : MonoBehaviour
{

    ObjectSpawner m_Spawner;

    void OnEnable()
    {
        m_Spawner = GetComponent<ObjectSpawner>();
        m_Spawner.spawnAsChildren = true;
    }

    void OnDisable()
    {
    }

    void OnObjectSelectorDropdownValueChanged(int value)
    {
        if (value == 0)
        {
            m_Spawner.RandomizeSpawnOption();
            return;
        }

        m_Spawner.spawnOptionIndex = value - 1;
    }

    void OnDestroyObjectsButtonClicked()
    {
        foreach (Transform child in m_Spawner.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
