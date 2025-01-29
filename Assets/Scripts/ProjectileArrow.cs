using UnityEngine;

public class ProjectileArrow : MonoBehaviour
{

    public static void CreateArrow(Vector3 spawnPosition, Vector3 targetPosition)
    {
        Transform arrowTransform = Instantiate(GameAssets.i.pfProjectileArrow, spawnPosition, Quaternion.identity);

        ProjectileArrow projectileArrow = arrowTransform.GetComponent<ProjectileArrow>();
        projectileArrow.Setup(targetPosition);
    }

    private Vector3 targetPosition;

    private void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 20f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float angle = GetAngleFromVectorFloat(moveDir);
        transform.eulerAngles = new Vector3 (0, 0, angle);

        float destroySelfDistance = 1f;
        if(Vector3.Distance(transform.position, targetPosition) < destroySelfDistance )
        {
            Destroy(gameObject);
        }
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
