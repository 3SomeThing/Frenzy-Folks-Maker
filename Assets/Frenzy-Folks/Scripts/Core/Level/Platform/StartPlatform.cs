using System;
using System.Collections;
using System.Collections.Generic;
using Frenzy_Folks.Scripts.Core;
using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartPlatform : Checkpoint
{
    [SerializeField] private BoxCollider startZone;
    [SerializeField, Min(1f)] private float countdownDuration = 5f;
    [SerializeField] private string countdownCanvasName = "PlayerCanvas";
    [SerializeField] private string countdownTextName = "StartTimerText";

    private readonly List<NetworkPlayer> players = new();
    private bool countdownStarted;
    private bool gameStarted;
}
