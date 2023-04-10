
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
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(0)), camera, new Shader("../../../Resources\\Generic.vert", "../../../Resources\\PointLight.frag"),
                                                                             new Shader("../../../Resources\\Generic.vert", "../../../Resources\\DirectLight.frag"),
                                                                             new Shader("../../../Resources\\Generic.vert", "../../../Resources\\SpotLight.frag")));
            _scenes.Add(new Scene(new Actor(new Engine.Geometry(1)), camera, new Shader("../../../Resources\\PointLight.vert", "../../../Resources\\Generic.frag"),
                                                                             new Shader("../../../Resources\\DirectLight.vert", "../../../Resources\\Generic.frag"),
                                                                             new Shader("../../../Resources\\SpotLight.vert", "../../../Resources\\Generic.frag")));
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
