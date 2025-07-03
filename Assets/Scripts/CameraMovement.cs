using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0, -100f);
    private float smoothTime = 0.05f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private bool freeCam = false;
    GenerateLevel generateLevel;

    private void Start()
    {
        generateLevel = FindAnyObjectByType<GenerateLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!generateLevel.playMode &&  Input.GetKeyDown(KeyCode.F))
        {
            if (freeCam)
            {
                freeCam = false;
                FindAnyObjectByType<Schmoovement>().enabled = true;
            }
            else { 
                freeCam = true;
                FindAnyObjectByType<Schmoovement>().enabled = false;
            }
        }
        if (freeCam)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            transform.position += new Vector3(horizontal, vertical, 0);
        } else
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
