using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;
using LazyFollow = UnityEngine.XR.Interaction.Toolkit.UI.LazyFollow;


public class GoalManager : MonoBehaviour
{
    public enum OnboardingGoals
    {
        Empty,
        FindSurfaces,
        TapSurface,
    }


    [SerializeField]
    GameObject m_CoachingUIParent;

    [SerializeField]
    FadeMaterial m_FadeMaterial;

    [SerializeField]
    LazyFollow m_GoalPanelLazyFollow;

    [SerializeField]
    GameObject m_TapTooltip;

    [SerializeField]
    ARPlaneManager m_ARPlaneManager;

    [SerializeField]
    ObjectSpawner m_ObjectSpawner;


    void Start()
    {
        if (m_TapTooltip != null)
            m_TapTooltip.SetActive(false);

        if (m_FadeMaterial != null)
            m_FadeMaterial.FadeSkybox(false);

        if (m_ObjectSpawner == null)
            m_ObjectSpawner = FindAnyObjectByType<ObjectSpawner>();
        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.Follow;

    }



    void Update()
    {

    }

    public void StartGame()
    {
        m_GoalPanelLazyFollow.positionFollowMode = LazyFollow.PositionFollowMode.None;
        m_CoachingUIParent.SetActive(false);

        m_TapTooltip.SetActive(true);

        m_FadeMaterial.FadeSkybox(true);

        m_ObjectSpawner.objectSpawned += OnObjectSpawned;
        StartCoroutine(TurnOnPlanes());
    }

    public IEnumerator TurnOnPlanes()
    {
        yield return new WaitForSeconds(1f);
        m_ARPlaneManager.enabled = true;
    }

    void OnObjectSpawned(GameObject spawnedObject)
    {
        m_TapTooltip.SetActive(false);
    }

}
