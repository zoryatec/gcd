namespace Gcd.CommandBuilder.Menu;

public interface IGcdMenu
{
    public INipkgMenu WithNipkgMenu();
    public IVipmMenu WithVipmMenu();
    
    public IToolsMenu WithToolsMenu();
    
    public ILabViewMenu WithLabViewMenu();
    
    public IConfigMenu WithConfigMenu();
}