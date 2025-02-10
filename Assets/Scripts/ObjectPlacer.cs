using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour {

    public static bool isStackStarted = false;
    
    public GameObject[] objectPrefabs;
    private GameObject selectedObject;
    public ARSession arSession;
    
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start() {
        isStackStarted = false;
        arSession.Reset();
        raycastManager = GetComponent<ARRaycastManager>();

        if (raycastManager == null) {
            Debug.LogWarning("ARRaycastManager is missing! Running in Editor Simulation Mode.");
        }

        if (objectPrefabs.Length > 0) {
            selectedObject = objectPrefabs[0]; // Default to Cube
        }
        else {
            Debug.LogError("No object prefabs assigned!");
        }
    }

    void Update() {
        if (Application.isEditor) { // Simulator mode for Unity Editor
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) { // Simulate AR Raycast using Physics.Raycast
                    if (hit.collider.CompareTag("SimulatedPlane") || hit.collider.CompareTag("SpawnedObject")) { // Ensure we hit our test plane or existing objects
                        Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                        Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], spawnPosition, Quaternion.identity);
                    }
                }
            }
        }
        else { // AR Mode for real devices
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                // Step 1: First try to hit existing objects using Physics.Raycast
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.CompareTag("SpawnedObject")) { // Spawn on top of other objects
                        Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 0.7f, hit.point.z);
                        Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], spawnPosition, Quaternion.identity);
                        Debug.Log("Placed object on existing object at " + hit.point);
                        return; // Stop further execution
                    }
                }

                // Step 2: If no object was hit, fallback to AR Raycast for detecting planes
                if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon)) {
                    Pose pose = hits[0].pose;
                    Vector3 spawnPosition = new Vector3(pose.position.x, pose.position.y + 0.7f, pose.position.z);
                    GameObject go = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], spawnPosition, Quaternion.identity);
                    if (!isStackStarted) {
                        isStackStarted = true;
                        go.tag = "RootObject";
                    }
                    Debug.Log("Placed object on AR Plane at " + pose.position);
                }
            }
        }
    }
}
