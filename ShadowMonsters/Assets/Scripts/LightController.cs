using UnityEngine;

namespace Assets.Scripts
{
    public class LightController : MonoBehaviour
    {
        public Light light;


        private static LightController lightController;

        public static LightController Instance()
        {
            if (!lightController)
            {
                lightController = FindObjectOfType(typeof(LightController)) as LightController;
                if (!lightController)
                    Debug.LogError("Could not find the light! Darkness reigns.");
            }
            return lightController;
        }

        public void ChangeToNormalLight()
        {
            light.color = new Color32(255,244,214,255);
        }
        public void ChangeToPlanesLight()
        {
            light.color = new Color32(200, 9, 221, 255);
        }
    }
}
