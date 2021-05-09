
using UnityEngine;
using TMPro;

public class ProjectileGunTutorial : MonoBehaviour
{

    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadtime, timeBetweenShots;
    public int magazinesize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Bug fixing:
    public bool allowinvoke = true;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazinesize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft>0)
        {
            //set bullets shot to 0
            bulletShot = 0;

            Shoot();

        }
        
    }
    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
            targetpoint = hit.point;
        else
            targetpoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetpoint - attackPoint.position;
        float X = Random.Range(-spread, spread);
        float Y= Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread = new Vector3(X, Y, 0);

        GameObject currenBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        currenBullet.transform.forward = directionWithSpread.normalized;

        currenBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currenBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * shootForce, ForceMode.Impulse);


        bulletsLeft--;
        bulletShot++;

        if(allowinvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowinvoke = false;

        }
        if (bulletShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShooting);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowinvoke = true;

    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadtime);
    }
}
