using UnityEngine;

public class ScrollVievExpander : MonoBehaviour
{
    public void UpdateSize(Vector2 size)
    {
        Debug.Log($"new viewport size {size}");
        transform.localScale = size;
    }
}
