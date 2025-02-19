using Gcd.CommandBuilder.Command.LabView;
using Gcd.CommandBuilder.Menu.LabView;

namespace Gcd.CommandBuilder.Menu;

public interface ILabViewMenu
{

    public IBuildSpecMenu WithBuildSpecMenu();
    public ArgBuilderLabViewKill WithKillCmd();
    public ArgBuilderRunVi WithRunViCmd();
    
    public ArgBuilderBuildProject WithBuildProjectCmd();
    
}