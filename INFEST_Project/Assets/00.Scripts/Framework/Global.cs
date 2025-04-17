public class Global : SingletonBehaviour<Global>
{
    public ResourceManager ResourceManager = new ResourceManager();
    public UIManager UIManager = new UIManager();
    public StoreManager storeManager = new StoreManager();
}
