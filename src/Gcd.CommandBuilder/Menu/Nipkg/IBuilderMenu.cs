namespace Gcd.CommandBuilder.Menu.Nipkg;

public interface IBuilderMenu
{
    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderAddContent WithAddContentCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderInit WithInitCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderSetVersion WithSetVersionCmd();
    public global::Gcd.CommandBuilder.Command.Nipkg.Builder.ArgBuilderAddInstruction WithAddInstructionCmd();
}