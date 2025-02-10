using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public void LoadPlayScreen() {
        SceneManager.LoadScene(1);
        ObjectPlacer.isStackStarted = false;
    }
}
