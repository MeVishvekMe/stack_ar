using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenUIControl : MonoBehaviour {
    public void GoToMenuScreen() {
        SceneManager.LoadScene(0);
        ObjectPlacer.isStackStarted = false;
    }

    public void ResetScene() {
        SceneManager.LoadScene(1);
        ObjectPlacer.isStackStarted = false;
    }
}
