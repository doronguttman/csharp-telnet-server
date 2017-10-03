namespace TelnetServer.Telnet
{
    public class ClearScreen : CommandBase
    {
        #region Overrides of CommandBase
        public override string GetCommandString() => ANSI.Erase.Screen;
        #endregion Overrides of CommandBase
    }
}
