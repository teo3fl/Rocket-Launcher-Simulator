using System.Collections;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject rocketPrefab;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float f_fireCooldown = 1f;

    private bool canFire = true;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
            StartCoroutine(FireCooldown());
        }
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(f_fireCooldown);
        canFire = true;
    }

    private void Fire()
    {
        canFire = false;

        var rocket = Instantiate(rocketPrefab, spawnPoint.position, transform.rotation).GetComponent<Rocket>();
        // rocket.target = PlaneManager.Instance.Plane;
    }
}
