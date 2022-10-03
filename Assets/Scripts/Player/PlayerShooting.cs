using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Projectile _rocketPrefab;
    [SerializeField] private Projectile _grenadePrefab;
    [SerializeField] private PlayerManager _playerManager;

    private float _charge = 1f;

    private void OnEnable()
    {
        _charge = 1f;
    }

    private void Update()
    {
        // 0 = Left Click, 1 = Right Click
        if (Input.GetMouseButtonDown(0))
        {
            InstantiateProjectile(ProjectileWeapon.Rocket, 0);

            return;
        }

        if (Input.GetMouseButton(1))
        {
            _charge += Time.deltaTime;

            // Todo: Create UI showing the charge
        }

        if (Input.GetMouseButtonUp(1) && _charge > 1f)
        {
            InstantiateProjectile(ProjectileWeapon.Granade, _charge);
        }
    }

    private void InstantiateProjectile(ProjectileWeapon projectileType, float charge)
    {
        Projectile prefab = projectileType == ProjectileWeapon.Rocket ? _rocketPrefab : _grenadePrefab;

        Transform camera = _playerManager.GetActivePlayerCameraTransform();
        Projectile projectile = Instantiate(prefab, camera.transform.position + camera.transform.forward * 2, camera.transform.rotation, null);

        projectile.projectile = projectileType;
        projectile.charge = charge;

        GameManager.Instance.UpdateGameState(GameState.Wait);
    }
}
