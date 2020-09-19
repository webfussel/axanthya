using UnityEngine;
using UnityEngine.U2D;

public class Camera_Controller : MonoBehaviour {
    
    [SerializeField] Camera gameCamera;
    [SerializeField] PixelPerfectCamera pixelPerfectCamera;

    [SerializeField] float keyboardSpeed = 7f, mouseSpeed = 10f;

    [SerializeField] private int zoomStep = 8, minSize = 16, maxSize = 128;
    float timeToWait = 0.5f, currentTimeToWait = 0;

    private void Update() {
        float moveSpeed = keyboardSpeed * Time.deltaTime;

        if(Input.GetKey(KeyCode.D))
        {
            gameCamera.transform.Translate(new Vector3(moveSpeed,0,0));
        }
        if(Input.GetKey(KeyCode.A))
        {
            gameCamera.transform.Translate(new Vector3(-moveSpeed,0,0));
        }
        if(Input.GetKey(KeyCode.S))
        {
            gameCamera.transform.Translate(new Vector3(0,-moveSpeed,0));
        }
        if(Input.GetKey(KeyCode.W))
        {
            gameCamera.transform.Translate(new Vector3(0,moveSpeed,0));
        }

        // Mouse Dragging
        if (Input.GetMouseButton(1))
        {
            float cameraSpeed = mouseSpeed / 3.5f * Time.deltaTime;
            gameCamera.transform.position -= new Vector3(Input.GetAxis("Mouse X") * cameraSpeed, Input.GetAxis("Mouse Y") * cameraSpeed, 0);
        }

        // Mouse Zoom
        if (currentTimeToWait <= 0F) {
            if ((Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.KeypadPlus)) && pixelPerfectCamera.assetsPPU < maxSize)
            {
                pixelPerfectCamera.assetsPPU += zoomStep;
                currentTimeToWait = timeToWait;
            }

            if ((Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey(KeyCode.KeypadMinus)) && pixelPerfectCamera.assetsPPU > minSize)
            {
                pixelPerfectCamera.assetsPPU -= zoomStep;
                currentTimeToWait = timeToWait;
            }

            pixelPerfectCamera.assetsPPU = pixelPerfectCamera.assetsPPU < minSize ? minSize : pixelPerfectCamera.assetsPPU;
            pixelPerfectCamera.assetsPPU = pixelPerfectCamera.assetsPPU > maxSize ? maxSize : pixelPerfectCamera.assetsPPU;
        } else {
            currentTimeToWait -= Time.deltaTime;
        }
    }
}