using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputActionReference resetGameAction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (resetGameAction != null && resetGameAction.action != null)
            resetGameAction.action.Enable();
    }

    private void OnDisable()
    {
        if (resetGameAction != null && resetGameAction.action != null)
            resetGameAction.action.Disable();
    }

    private void FixedUpdate()
    {
        if (resetGameAction != null && resetGameAction.action != null && resetGameAction.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Game Reset!");
        }
    }
}
