using UnityEngine;

public class Parallax : MonoBehaviour
{
    public bool moveOnYAxis;
    public bool repeating;
    public float parallaxMultiplier = .5f;

    private Transform cam;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    void Start()
    {
        cam = Camera.main.transform;
        lastCameraPosition = cam.position;
        if (repeating)
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Texture2D texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCameraPosition;
        if (moveOnYAxis)
        {
            transform.position += deltaMovement * parallaxMultiplier;
        }
        else
        {
            Vector3 newPosition = new Vector3(transform.position.x + deltaMovement.x * parallaxMultiplier, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
        lastCameraPosition = cam.position;

        if (repeating)
        {
            if (Mathf.Abs(cam.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cam.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cam.position.x + offsetPositionX, transform.position.y);
            }
        }
    }
}
