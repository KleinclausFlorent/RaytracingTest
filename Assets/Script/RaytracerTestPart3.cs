using System;
using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(Camera))]
    public class RaytracerTestPart3 : MonoBehaviour
    {


        private Camera _camera;
        private Texture2D _renderTexture;

        private bool _realTime = false;

        private float _renderResolution = 1;

        private Light[] _lights;
        


        private void Awake()
        {
            this._renderTexture = new Texture2D(Convert.ToInt32(Screen.width*_renderResolution), Convert.ToInt32(Screen.height*_renderResolution));
            this._camera = GetComponent<Camera>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (!this._realTime)
            {
                RayTrace();
            }
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), _renderTexture );
        }

        // Update is called once per frame
        public void Update()
        {
            if (_realTime)
            {
                RayTrace();
            }
        }


        private void RayTrace()
        {

            _lights = FindObjectsOfType(typeof(Light)) as Light[];
            
            
            for (var x = 0; x < this._renderTexture.width; x++){
                for (var y = 0;y  < this._renderTexture.height; y++)
                {
                    Ray ray = this._camera.ScreenPointToRay(new Vector3(x/_renderResolution,y/_renderResolution,0));
                    
                    this._renderTexture.SetPixel(x,y, TraceRay(ray));
                }
                
            }
            this._renderTexture.Apply();
        }


        private Color TraceRay(Ray ray)
        {
            var returnColor = Color.black;

            if (Physics.Raycast(ray, out var hit))
            {

                Material mat;

                mat = hit.collider.GetComponent<Renderer>().material;

                if (mat.mainTexture)
                {
                    returnColor +=
                        ((Texture2D) mat.mainTexture).GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y);
                }
                else
                {
                    returnColor += mat.color;
                }

                returnColor *= TraceLight(hit.point + hit.normal * 0.0001f);
            }
	
            return returnColor;
        }


        private Color TraceLight(Vector3 pos)
        {
            var returnColor = RenderSettings.ambientLight;

            foreach (var light in _lights)
            {
                if (light.enabled)
                {
                    returnColor += LightTrace(light, pos);
                }
            }

            return returnColor;
        }

        private Color LightTrace(Light light, Vector3 pos)
        {
            if (light.type == LightType.Directional)
            {
                if (Physics.Raycast(pos, -light.transform.forward))
                {
                    return Color.black;
                }

                return light.color * light.intensity;
            }
            return Color.magenta;
        }
    }
}
