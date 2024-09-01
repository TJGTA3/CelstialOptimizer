using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Celstial.Functions;
using JustPlay.Gameplay.Abilities.UI;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Celstial.Main
{
    public class Entity : MonoBehaviour
    {
        private int maxTextureSize = 512;
        TextureFormat textureFormat = TextureFormat.ETC2_RGBA8;
        public int poolSize = 10;
        private Queue<GameObject> pool = new Queue<GameObject>();
        public GameObject prefab;
        private float timer;
        public static List<ActionDisplay> ads;
        public static List<WebHudLayoutManager> hud;
        public static List<HotbarSlot> hbs;
        public static List<Image> uii;
        public static List<AbilitySlotUI> asui;
        public static List<Animator> animators;
        public static List<Camera> Cameras;
        public static List<TextMeshProUGUI> tmpu;
        public static List<ParticleSystem> ParticleSystems;
        public static PartyRoomConnector prc;
        
        public void Start()
        {
            Resources.UnloadUnusedAssets();
            
            foreach (Texture2D texture2D in Resources.FindObjectsOfTypeAll(typeof(Texture2D)))
            {
                int newWidth = Mathf.Max(1, texture2D.width / 10000);
                int newHeight = Mathf.Max(1, texture2D.height / 10000);
                Texture2D resizedTexture =
                    new Texture2D(newWidth, newHeight, texture2D.format, texture2D.mipmapCount > 1);

                Color[] pixels = texture2D.GetPixels(0);
                Color[] resizedPixels = new Color[newWidth * newHeight];
                float xRatio = (float)texture2D.width / newWidth;
                float yRatio = (float)texture2D.height / newHeight;

                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        int px = Mathf.FloorToInt(x * xRatio);
                        int py = Mathf.FloorToInt(y * yRatio);
                        resizedPixels[y * newWidth + x] = pixels[py * texture2D.width + px];
                    }
                }

                resizedTexture.SetPixels(resizedPixels);
                resizedTexture.Apply();

                ReplaceTextureInMaterials(texture2D, resizedTexture);
            }
            
            MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                OptimizeMesh(meshFilter);
                MeshRenderer renderer = meshFilter.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.receiveShadows = false;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
            
            for (int i = 0; i < poolSize; i++)
            {
                var obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            
            foreach (SpriteRenderer spriteRenderer in FindObjectsOfType<SpriteRenderer>(true))
            {
                OptimizeSprite(spriteRenderer.sprite);
            }

            CanvasRenderer[] allCanvasRenderers = FindObjectsOfType<CanvasRenderer>(true);

            foreach (CanvasRenderer canvasRenderer in allCanvasRenderers)
            {
                OptimizeUIElement(canvasRenderer);
            }
            
            

        }
        
        
        
        void OptimizeUIElement(CanvasRenderer canvasRenderer)
        {
            Image image = canvasRenderer.GetComponent<Image>();
            if (image != null && image.sprite != null)
            {
                OptimizeSprite(image.sprite);
            }
        }

        void OptimizeSprite(Sprite sprite)
        {
            if (sprite == null) return;

            Texture2D texture = sprite.texture;

            // Create a new texture with the desired settings
            Texture2D optimizedTexture = new Texture2D(
                Mathf.Min(texture.width, maxTextureSize),
                Mathf.Min(texture.height, maxTextureSize),
                textureFormat,
                texture.mipmapCount > 1
            );

            // Copy texture data
            RenderTexture renderTexture = RenderTexture.GetTemporary(
                optimizedTexture.width,
                optimizedTexture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );

            RenderTexture.active = renderTexture;
            Graphics.Blit(texture, renderTexture);

            optimizedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            optimizedTexture.Apply();

            RenderTexture.ReleaseTemporary(renderTexture);

            // Create a new sprite with the optimized texture
            Rect rect = sprite.rect;
            Sprite newSprite = Sprite.Create(
                optimizedTexture,
                new Rect(0, 0, optimizedTexture.width, optimizedTexture.height),
                new Vector2(0.5f, 0.5f),
                sprite.pixelsPerUnit,
                0,
                SpriteMeshType.FullRect
            );

            // Replace the sprite's texture with the optimized one
            sprite = newSprite;
        }

        
        void OptimizeMesh(MeshFilter meshFilter)
        {
            Mesh mesh = meshFilter.mesh;
            MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                foreach (Material material in meshRenderer.sharedMaterials)
                {
                    Texture mainTexture = material.mainTexture;
                    if (mainTexture != null)
                    {
                        OptimizeTexture((Texture2D)mainTexture);
                    }
                }
            }
        }

        void OptimizeTexture(Texture2D texture)
        {
            if (texture == null) return;

            Texture2D optimizedTexture = new Texture2D(
                Mathf.Min(texture.width, maxTextureSize),
                Mathf.Min(texture.height, maxTextureSize),
                textureFormat,
                texture.mipmapCount > 1
            );

            RenderTexture renderTexture = RenderTexture.GetTemporary(
                optimizedTexture.width,
                optimizedTexture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );

            RenderTexture.active = renderTexture;
            Graphics.Blit(texture, renderTexture);

            optimizedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            optimizedTexture.Apply();

            RenderTexture.ReleaseTemporary(renderTexture);
        }
        
        static void ReplaceTextureInMaterials(Texture2D oldTexture, Texture2D newTexture)
        {
            // Find all materials using the old texture and replace it with the new one
            Material[] allMaterials = FindObjectsOfType<Material>();
            foreach (Material mat in allMaterials)
            {
                if (mat.mainTexture == oldTexture)
                {
                    mat.mainTexture = newTexture;
                }
            }
        }

        void Update()
        {
            
            timer += Time.deltaTime;
            if (timer >= 1.5f)
            {
                if (PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "MainMenu")
                {
                    tmpu = FindObjectsOfType<TextMeshProUGUI>().ToList();
                }
                Calls();
                
                timer = 0;
            }
        }

        void Calls()
        {
            prc = FindObjectOfType<PartyRoomConnector>();
            ParticleSystems = FindObjectsOfType<ParticleSystem>().ToList();
            if ((bool)Config.Get("SeeUsers"))
            {
                if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("COUser")) PhotonNetwork.LocalPlayer.CustomProperties.Add("COUser", "true");
            }
            else
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("COUser")) PhotonNetwork.LocalPlayer.CustomProperties.Remove("COUser");
                
            }
            

            if ((bool)Config.Get("FPSCapper"))
            {
                if (Application.targetFrameRate != (int)(float)Config.Get("FPSCap")) Application.targetFrameRate = (int)(float)Config.Get("FPSCap");
            }
            else
            {
                if (Application.targetFrameRate != int.MaxValue) Application.targetFrameRate = int.MaxValue;
            }
            
            
            Config.SaveConfig(Config.filepath);
            if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "MoveScene")
            {

                Time.fixedDeltaTime = 0.02f; // Set fixed timestep for physics updates

                Cameras = FindObjectsOfType<Camera>().ToList();
                Camera.main.orthographicSize = 1; // Set orthographic size to minimum
                Camera.main.farClipPlane = 10000; // Increase far clip plane
                Camera.main.nearClipPlane = 0.01f; // Decrease near clip plane
                
                uii = FindObjectsOfType<Image>().ToList();
                
               
                
                UnityEngine.QualitySettings.SetQualityLevel(0, true);
                UnityEngine.QualitySettings.antiAliasing = 0;
                UnityEngine.QualitySettings.renderPipeline = null;
                Type qualitySettingsType = typeof(QualitySettings);

                PropertyInfo shadowmaskModeProp = qualitySettingsType.GetProperty("shadowmaskMode", BindingFlags.Static | BindingFlags.Public);
                if (shadowmaskModeProp != null)
                {
                    shadowmaskModeProp.SetValue(null, ShadowmaskMode.DistanceShadowmask);
                }

                PropertyInfo activeColorSpaceProp = qualitySettingsType.GetProperty("activeColorSpace", BindingFlags.Static | BindingFlags.Public);
                if (activeColorSpaceProp != null)
                {
                    activeColorSpaceProp.SetValue(null, ColorSpace.Gamma);
                }

                PropertyInfo maxQueuedFramesProp = qualitySettingsType.GetProperty("maxQueuedFrames", BindingFlags.Static | BindingFlags.Public);
                if (maxQueuedFramesProp != null)
                {
                    maxQueuedFramesProp.SetValue(null, 1);
                }


            }
        }
        
        
        
    }
}