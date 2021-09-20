using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
   public ECSSnake playerSnake;
    Camera cam;
    float startOrtographic;
	// Use this for initialization

	void Start () {
      //  Application.targetFrameRate = 60;
        cam = GetComponent<Camera>();
        startOrtographic = cam.orthographicSize - 1;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (playerSnake != null)
        {
          Follow();
            Zoom();
        }

        if (GameManager.STATE.IN_DUEL == GameManager.instance.state)
        {
            cam.orthographicSize = 80f;
        }

        if (SnakeSpawner.Instance.playerStayDead && GameManager.STATE.IN_DUEL != GameManager.instance.state)
        {
            this.transform.position = new Vector3(SnakeSpawner.Instance.temp_playerTracker.x, transform.position.y, SnakeSpawner.Instance.temp_playerTracker.z);
        }
	}

    void Zoom() {
        float scale = playerSnake.referenceScale;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,startOrtographic + scale,1);


    }

    void Follow() {
        Vector3 playerPosition = SnakeSpawner.Instance.playerTracker.transform.position;
        playerPosition.y = transform.position.y;
        transform.position = playerPosition;// Vector3.Lerp(transform.position, playerPosition,  10*Time.deltaTime);

    }
}
