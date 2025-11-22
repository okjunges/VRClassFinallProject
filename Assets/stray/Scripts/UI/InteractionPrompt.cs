using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Trigger";
    [SerializeField] private string promptText = "[E]로 상호작용";
    [SerializeField] private Vector2 position = new Vector2(0.5f, 0.8f); // Screen relative position (0.5, 0.8 is bottom center)
    [SerializeField] private int fontSize = 30;
    [SerializeField] private Color textColor = Color.white;
    
    private bool showPrompt = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetInteractionPrompt(true, promptText);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.SetInteractionPrompt(false);
            }
        }
    }
}
