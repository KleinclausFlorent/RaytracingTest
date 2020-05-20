using System;
using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(Camera))]
    public class RaytracerTestPart2 : MonoBehaviour
    {


        private Camera _camera;
        private Texture2D _renderTexture;

        private bool _realTime = false;

        private float _renderResolution = 1;

        


        private void Awake()
        {
            this._renderTexture = new Texture2D(Convert.ToInt32(Screen.width*_renderResolution), Convert.ToInt32(Screen.height*_renderResolution));
            this._camera = GetComponent<Camera>();
        }

        // Start is called before the first frame update
        void Start()
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
        void Update()
        {
            if (_realTime)
            {
                RayTrace();
            }
        }


        private void RayTrace()
        {
            for (var x = 0; x < this._renderTexture.width; x++){
                for (var y = 0;y  < this._renderTexture.height; y++)
                {
                    Ray ray = this._camera.ScreenPointToRay(new Vector3(x/_renderResolution,y/_renderResolution,0));
                    
                    this._renderTexture.SetPixel(x,y, TraceRay(ray));
                }
                
            }
            this._renderTexture.Apply();
        }


        private static Color TraceRay(Ray ray)
        {
            var returnColor = Color.black;

            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
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
            }
	
            return returnColor;
        }
    }
}
