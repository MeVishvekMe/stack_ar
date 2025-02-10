using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectFallDetection : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("SpawnedObject")) {
            ObjectPlacer.isStackStarted = false;
            SceneManager.LoadScene(1);
        }
    }
}
