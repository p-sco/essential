using UnityEngine;
/**
 * A camera to help with Orthagonal mode when you need it to lock to pixels.  Desiged to be used on android and retina devices.
 */
public class CenterOnPlayer : MonoBehaviour
{

    public float pixelsPerUnit = 16.0f;
    /**
     * A game object that the camera will follow the x and y position of.
     */
    public GameObject followTarget;

    private Camera _camera;
    private int _currentScreenWidth = 0;
    private int _currentScreenHeight = 0;

    private float _pixelLockedPPU = 16.0f;

    protected void Start()
    {
        followTarget = GameObject.Find("Player");
        _camera = this.GetComponent<Camera>();
    }


    public void Update()
    {
        if (_camera && followTarget)
        {
            Vector2 newPosition = new Vector2(followTarget.transform.position.x, followTarget.transform.position.y);
            //float nextX = Mathf.Round(_pixelLockedPPU * newPosition.x);
            //float nextY = Mathf.Round(_pixelLockedPPU * newPosition.y);
            //_camera.transform.position = new Vector3(nextX / _pixelLockedPPU, nextY / _pixelLockedPPU, _camera.transform.position.z);
            _camera.transform.position = new Vector3(newPosition.x, newPosition.y, _camera.transform.position.z);

        }
    }
}