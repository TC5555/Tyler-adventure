using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchScript : MonoBehaviour
{

    protected void Launch(Vector2 direction, Vector2 position, int shootMulti, GameObject ProjectilePrefab)
    {
        for (int i = shootMulti; i > 0; i--)
        {
            float shootAngle = Mathf.Atan2(direction.y, direction.x) * (180f / Mathf.PI);
            GameObject projectileObject = Instantiate(ProjectilePrefab, position, Quaternion.Euler(0f, 0f, shootAngle));
            ProjectileScript projectile = projectileObject.GetComponent<ProjectileScript>();

            projectile.Launch(direction, transform.position);
        }
    }
}
