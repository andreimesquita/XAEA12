using System;
using Sources.Photon;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    public bool Initialized { get; private set; }
    
    public static SimulationController Instance
    {
        get => _instance;
        set => _instance = value;
    }
    private static SimulationController _instance = null;

    private void Awake()
    {
        _instance = this;
        Initialized = false;

        PhotonFacade photonFacade = PhotonFacade.Instance;
        photonFacade.OnStartGameSimulation += OnStartGameSimulation;
    }

    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.Return))
            Initialized = true;
    }

    private void OnStartGameSimulation()
    {
        Initialized = true;
    }
}
