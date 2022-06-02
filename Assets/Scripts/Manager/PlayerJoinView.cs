using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerJoinView : MonoBehaviour
{
    public GameObject firstSelected = null;
    public GameObject playerRoot = null;
    public Image backgroundImage = null;
    public GameObject characterSelectPanel = null;
    public UnityEvent OnJoinEvent = null;
    public FighterPartSelection fighterPartSelection;
    public Fighter previewGameObject = null;

    [Header("Ready")]
    public bool isReady = false;
    public UnityEvent OnReady = null;
    public UnityEvent OnUnready = null;

    public delegate void OnReadyChange();
    public OnReadyChange onReadyChange;

    public bool isPlayer = false;

    public void Ready()
    {
        if (isReady) return;
        isReady = true;
        OnReady.Invoke();
        onReadyChange();
    }

    public void Unready()
    {
        if (!isReady) return;
        isReady = false;
        OnUnready.Invoke();
        onReadyChange();
    }

    public void BuildPreview()
    {
        foreach (Transform child in previewGameObject.transform)
        {
            Destroy(child.gameObject);
        }

        List<FighterWeapon> fighterWeapons = new List<FighterWeapon>();
        fighterWeapons.Add(FighterCreator.singleton.fighterWeapons[fighterPartSelection.currentWeaponID]);
        previewGameObject.AssembleFighterParts(FighterCreator.singleton.fighterBodies[fighterPartSelection.currentBodyID], fighterWeapons, FighterCreator.singleton.fighterPowerups[fighterPartSelection.currentPowerupID]);
    }
}
