using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Projectile _rocketPrefab;
    [SerializeField] private Projectile _grenadePrefab;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private UIManger _uiManager;

    private float _charge = 1f;

    private void OnEnable()
    {
        _charge = 1f;
    }

    private void Update()
    {
        if (GameManager.IsGamePaused) 
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Time.frameCount: " + Time.frameCount);
            InstantiateProjectile(ProjectileWeapon.Rocket, 0);

            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _uiManager.EnablePlayerChargeUI();
        }

        if (Input.GetMouseButton(1))
        {
            if (_charge < 10f)
            {
                _uiManager.UpdatePlayerCharge(_charge += Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonUp(1))
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
