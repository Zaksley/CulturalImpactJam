using UnityEngine;

namespace Player.BaseControllers
{
    public class CursorState : MonoBehaviour
    {
        public bool IsVisibleCursor = false;

        private void Start()
        {
            Cursor.visible = IsVisibleCursor;

            if (!IsVisibleCursor)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None; 
        }
    }
}