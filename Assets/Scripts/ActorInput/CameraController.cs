using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float cam_speed;

	[SerializeField]
	private float cam_border_min;

	[SerializeField]
	private float cam_border_max;

    [SerializeField]
    private Transform cam_target;

    private Transform cachedTransform;
	float standart_cam_speed;

    private void Awake()
    {
        //if (!cam_target)
        //{
        //    cam_target = FindObjectOfType<Player>().transform;
        //}

        cachedTransform = transform;
		standart_cam_speed = cam_speed;
    }

    void FixedUpdate()
    {
        Vector3 cam_target_position = cam_target.position;
		cam_target_position.z = cachedTransform.localPosition.z;
		cam_target_position.y = cachedTransform.localPosition.y;
		cam_target_position.x = Mathf.Clamp (cam_target_position.x, cam_border_min, cam_border_max);

        //Можно метод Lerp (плавнее)
		cachedTransform.localPosition = Vector3.Lerp(cachedTransform.localPosition, cam_target_position, cam_speed);
    }

	public void SetTarget(Transform target, float speed)
	{
		cam_target = target;
		cam_speed = speed;
	}

	public void ResetTarget()
	{
		cam_speed = standart_cam_speed;
		cam_target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
}