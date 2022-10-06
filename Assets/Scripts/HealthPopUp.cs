using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HealthPopUp : MonoBehaviour
{
    public int damage; 
    public Camera cameraLookAt;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;

    private void Start()
    {
        healthText.text = "-" + damage;
        animator.Play("Health");
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
    }

    private void Update()
    {
        Quaternion rotation = cameraLookAt.transform.rotation;
        canvas.transform.LookAt(canvas.transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
