using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody2D bulletPrefab;

    void Update()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                // แก้ตัวพิมพ์เล็กให้ตรงกับ [SerializeField] ด้านบน
                target.transform.position = new Vector2(hit.point.x, hit.point.y);
                Debug.Log($"Hit {hit.collider.gameObject.name}");

                // เพิ่มฟังก์ชัน CalculateProjectileVelocity ด้านล่างนี้เพื่อให้ Error หายไป
                Vector2 projectileVelocity = CalculateProjectileVelocity(shootPoint.position, hit.point, 1f);

                Rigidbody2D shootBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

                // หากใช้ Unity เวอร์ชั่นใหม่ใช้ linearVelocity (ถ้า Error ให้แก้เป็น velocity เฉยๆ)
                shootBullet.linearVelocity = projectileVelocity;
            }
        }
    }

    // ต้องมีฟังก์ชันนี้ในสคริปต์ด้วย มิฉะนั้นจะเกิด Error สีแดง
    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 targetPos, float time)
    {
        Vector2 distance = targetPos - origin;
        float vx = distance.x / time;
        float vy = (distance.y / time) - (0.5f * Physics2D.gravity.y * time);
        return new Vector2(vx, vy);
    }
}