using System;

namespace TelnetServer.Screens
{
    internal class Welcome : ScreenBase
    {
        private const string SCREEN =
            "\r\n" +
            "\r\n" +
            "\r\n" +
            "\r\n" +
            "       WELCOME!\r\n" +
            "\r\n" +
            "\r\n" +
            "\r\n" +
            "\r\n" +
            "What would you like to do?: ";

        #region Overrides of ScreenBase
        public override string GetScreen() => SCREEN;

        public override void ProcessPresentation(string presentation)
        {
            throw new NotImplementedException();
        }
        #endregion Overrides of ScreenBase
    }
}
