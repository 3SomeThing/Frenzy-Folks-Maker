using System.Collections;
using System.Collections.Generic;
using Frenzy_Folks.Scripts.Core;
using Frenzy_Folks.Scripts.Network;
using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkObject))]
public class EndPlatform : NetworkBehaviour
{
    [SerializeField] private BoxCollider endZone;
    [SerializeField, Min(1f)] private float endTimerDuration = 20f;
    [SerializeField] private string countdownCanvasName = "PlayerCanvas";
    [SerializeField] private string countdownTextName = "EndTimerText";

    private List<string> playerNameArrivalsOrder = new List<string>();
    private readonly HashSet<ulong> _arrivedPlayers = new();
    
    private bool endCountdownStarted;
}
