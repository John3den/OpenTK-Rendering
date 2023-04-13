
using OpenTK.Mathematics;

namespace Engine
{
    public class SceneManager
    {
        private List<Scene> _scenes;
        private List<Material> _materials = new List<Material>();
        private int _sceneNumber;
        public int _material = 1;
        private const int MAXSCENES = 2;
        public bool _isRotating;
        private float _globalTime;
        public void GlobalTimeTick(float delta)
        {
            _globalTime += delta;
        }
        public float GlobalTime()
        {

            return _globalTime;
        }
        public void NextScene()
        {
            _sceneNumber = (_sceneNumber + 1) % MAXSCENES;
        }
        public SceneManager(Renderer rend) 
        {
            _scenes = new List<Scene>();
            string ShaderFolder = "../../../Resources\\Shaders\\";
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(0)), rend._camera, new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\PointLight.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\DirectLight.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\SpotLight.frag")));
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(1)), rend._camera, new Shader(ShaderFolder + "Vertex\\PointLight.vert", ShaderFolder + "Fragment\\Generic.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\DirectLight.vert", ShaderFolder + "Fragment\\Generic.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\SpotLight.vert", ShaderFolder + "Fragment\\Generic.frag")));
            _materials.Add(new Material(0.1f, 2.3f, 32, new Vector3(1, 0.1f, 0.1f))); // red metal
            _materials.Add(new Material(0.2f, 3.0f,100, new Vector3(0.3f, 1f, 0.3f))); // jade
            _materials.Add(new Material(0.2f, 0.2f, 1, new Vector3(1, 0.1f, 0.1f))); // red rubber
            _materials.Add(new Material(0.4f, 4f,120, new Vector3(0.7f, 0.7f, 0.7f))); // ceramic
            _materials.Add(new Material(0.72f, 2.7f,1, new Vector3(0.8f, 0.8f, 0.8f))); // silver
            _materials.Add(new Material(0.1f, 2.8f,30, new Vector3(1, 0.5f, 0.15f))); // bronze
        }
        public Material GetMaterial()
        {
            return _materials[_material];
        }
        public int GetSceneNumber()
        {
            return _sceneNumber;
        }
        public Scene CurrentScene { get { return _scenes[_sceneNumber]; }}
    }
}
