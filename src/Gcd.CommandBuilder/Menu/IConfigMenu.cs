using Gcd.CommandBuilder.Command.Config;
using Gcd.CommandBuilder.Command.Vipm;

namespace Gcd.CommandBuilder.Menu;

public interface IConfigMenu
{
    public ArgBuilderGetConfig WithGetCmd();
    public ArgBuilderSetConfig WithSetCmd();
}