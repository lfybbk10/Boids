using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineTargetController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private float movementSpeed = 1f;
    
    [Header("Cinemachine Data")]
    [SerializeField] private CinemachineVirtualCameraBase cinemachineCamera = null; 
    [SerializeField] private CinemachineInputProvider cinemachineInputProvider = null;
    
    [Header("Input Data")]
    [SerializeField] private InputActionReference moveAction = null;
    [SerializeField] private InputActionReference enableLookAction = null;
    
    private void Update()
    {
        if (moveAction.action.IsPressed())
        {
            transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward);
        }

        cinemachineInputProvider.enabled = enableLookAction.action.IsPressed();
        
        transform.rotation = Quaternion.LookRotation(transform.position - cinemachineCamera.transform.position);
    }
}
