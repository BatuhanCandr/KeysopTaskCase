using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;set; }
    [Header("Player")]
    [SerializeField] internal PlayerController playerController;
    [SerializeField] internal DigController digController;
    
    private void Awake()
    {
        Instance = this;
    }

  
}
