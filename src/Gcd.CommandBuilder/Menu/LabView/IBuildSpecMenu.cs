using Gcd.CommandBuilder.Command.LabView;

namespace Gcd.CommandBuilder.Menu.LabView;

public interface IBuildSpecMenu
{
    public ArgBuilderBuildSpec WithBuildCmd();
    
    public ArgBuilderSetVersion WithSetVersionCmd();
    
    public ArgBuilderList WithListCmd();
}