using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void MoveSound()
    {
        GameManager.Instance.soundManager.PlaySound(1);
    }

    public void DigSound()
    {
        GameManager.Instance.soundManager.PlaySound(0);
    }
}
