using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;set; }
    [Header("Manager")]
    [SerializeField] internal  SoundManager soundManager;

    
    [Header("Player")]
    [SerializeField] internal PlayerController playerController;
    [SerializeField] internal DigController digController;
    [SerializeField] internal PlayerSound playerSound;
    
    private void Awake()
    {
        Instance = this;
    }

    public void RestartGame()
    {
      var activeScene =  SceneManager.GetActiveScene();
      SceneManager.LoadScene(activeScene.name);
    }
  
}
