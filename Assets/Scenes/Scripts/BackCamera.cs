using System.Collections;
using System.Collections.Generic;
using NatSuite.Devices;
using UnityEngine;
using UnityEngine.UI;

public class BackCamera : MonoBehaviour
{
    [SerializeField]  RawImage previewPanel;
    [SerializeField]  AspectRatioFitter aspectFitter;

    [SerializeField] private Image flipImage;

    private MediaDeviceQuery query;

    private Texture2D previewTexture;
    
    // Start is called before the first frame update
    async void Start () {
        // Request camera permissions
        if (!await MediaDeviceQuery.RequestPermissions<CameraDevice>()) {
            Debug.LogError("User did not grant camera permissions");
            return;
        }
        // Create a device query for device cameras
        query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
        // Start camera preview
        var device = query.current as CameraDevice;
        previewTexture = await device.StartRunning();
        Debug.Log($"Started camera preview with resolution {previewTexture.width}x{previewTexture.height}");
        // Display preview texture
        previewPanel.texture = previewTexture;
        aspectFitter.aspectRatio = (float)previewTexture.width / previewTexture.height;
        flipImage.color = Color.cyan;
    }
    
    public async void SwitchCamera () {
        // Check that there is another camera to switch to
        if (query.count < 2)
            return;
        // Stop current camera
        var device = query.current as CameraDevice;
        device.StopRunning();
        // Advance to next available camera
        query.Advance();
        // Start new camera
        device = query.current as CameraDevice;
        previewTexture = await device.StartRunning();
        // Display preview texture
        previewPanel.texture = previewTexture;
        aspectFitter.aspectRatio = (float)previewTexture.width / previewTexture.height;
        if (flipImage.color == Color.cyan)
        {
            flipImage.color = Color.magenta;
        }
        else
        {
            flipImage.color = Color.cyan;
        }
    }
}
