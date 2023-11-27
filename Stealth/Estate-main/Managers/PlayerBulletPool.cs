using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class PlayerBulletPool : MonoBehaviour
{
    public IObjectPool<GameObject> bulletPool;
    public GameObject bulletPrefab;
    public int defaultCapacity = 10;
    public int MaxCapacity;
    public int createdSize;
    public float bulletSpeed;

    private static PlayerBulletPool m_instance;
    public static PlayerBulletPool Instance
    {
        get { return m_instance; }
        set { m_instance = value; }
    }

    private void Awake()
    {
        Instance = this;
        bulletPool = Pool;
    }

    private IObjectPool<GameObject> Pool
    {
        get
        {
            bulletPool = new ObjectPool<GameObject>(OnCreaateBullet, OnGetBullet, OnReturnBullet, OnDestroyBullet, true, 10, MaxCapacity);
            return bulletPool;
        }
    }

    private GameObject OnCreaateBullet()
    {
        GameObject clone = Instantiate(bulletPrefab);
        return clone;
    }
    private void OnGetBullet(GameObject bullet)
    {
        bullet.SetActive(true);
    }
    private void OnReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
    private void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    public void FireBullet(Transform nuzzle,Transform target)
    {
        GameObject bullet = PlayerBulletPool.Instance.bulletPool.Get();
        bullet.transform.position = nuzzle.position;
        StartCoroutine(MoveBullet(bullet, target.position +new Vector3(0, 1, 0)));
    }
    IEnumerator MoveBullet(GameObject bullet, Vector3 pos)
    {
        Vector3 curr = bullet.transform.position;
        float distanceToTarget = (curr - pos).sqrMagnitude; ;
        float step = 0;
        bullet.transform.LookAt(pos);

        while (distanceToTarget > 1f)
        {
            curr = bullet.transform.position;
            distanceToTarget = (curr - pos).sqrMagnitude;
            bullet.transform.position = Vector3.Lerp(curr, pos, bulletSpeed * step * Time.deltaTime);
            step++;
            yield return new WaitForFixedUpdate();
        }
        PlayerBulletPool.Instance.bulletPool.Release(bullet);
        yield break;

    }

}

