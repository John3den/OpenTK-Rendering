
namespace Engine
{
    public class SceneManager
    {
        private List<Scene> _scenes;
        private int _sceneNumber;
        private const int MAXSCENES = 2;
        public void NextScene()
        {
            _sceneNumber = (_sceneNumber + 1) % MAXSCENES;
        }
        public SceneManager(Camera camera) 
        {
            _scenes = new List<Scene>();
            string ShaderFolder = "../../../Resources\\Shaders\\";
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(0)), camera, new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\PointLight.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\DirectLight.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\Generic.vert", ShaderFolder + "Fragment\\SpotLight.frag")));
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(1)), camera, new Shader(ShaderFolder + "Vertex\\PointLight.vert", ShaderFolder + "Fragment\\Generic.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\DirectLight.vert", ShaderFolder + "Fragment\\Generic.frag"),
                                                                             new Shader(ShaderFolder + "Vertex\\SpotLight.vert", ShaderFolder + "Fragment\\Generic.frag")));
        }
        public int GetSceneNumber()
        {
            return _sceneNumber;
        }
        public void ChangeScene()
        {
            _sceneNumber = (_sceneNumber + 1) % MAXSCENES;
        }
        public Scene CurrentScene { get { return _scenes[_sceneNumber]; }}
    }
}
