namespace SideCar.Server;

internal class ComponentLoader
{
    private readonly List<IComponentLoader> _loaders = new()
    {
        new PersistenceStoreLoader()
    };

    public IComponentLoader? GetLoader(string type) 
        => _loaders.FirstOrDefault(loader => loader.IsSupported(type));
}



internal interface IComponentLoader
{
    string Type { get; }

    public bool IsSupported(string type) 
        => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);
    
    void Accept(IVisitor visitor);
}

internal class PersistenceStoreLoader : IComponentLoader
{
    public string Type => "Persistence";
    
    public void Accept(IVisitor visitor)
    {
        visitor.VisitPersistenceStoreLoader(this);
    }
}

internal interface IVisitor
{
    void VisitPersistenceStoreLoader(PersistenceStoreLoader element);
}


internal interface IDataStore
{
    
}