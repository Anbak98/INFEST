using Fusion;

public class Global : SingletonBehaviour<Global>
{
    public ResourceManager ResourceManager = new ResourceManager();
    public UIManager UIManager = new UIManager();
    public StoreManager storeManager = new StoreManager();
    public NetworkRunner NetworkRunner = new();    

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}
