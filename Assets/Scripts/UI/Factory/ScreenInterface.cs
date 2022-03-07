using System;
using UI.Enum;

namespace UI.Factory
{
    public sealed class ScreenInterface : IDisposable
    {
        private BaseScreen _currentScreen;
        private readonly ScreenFactory _screenFactory;
        private static ScreenInterface _screenInterface;

        private ScreenInterface() => _screenFactory = new ScreenFactory();

        public static ScreenInterface GetScreenInterface()
        {
            return _screenInterface ??= new ScreenInterface();
        }

        public void Execute(ScreenType screenType)
        {
            if (_currentScreen)
            {
                _currentScreen.Hide();
            }

            _currentScreen = screenType switch
            {
                ScreenType.LobbyScreen => _screenFactory.GetLobbyScreen(),
                ScreenType.GameScreen => _screenFactory.GetGameScreen(),
                ScreenType.WinScreen => _screenFactory.GetWinScreen(),
                ScreenType.LoseScreen => _screenFactory.GetLoseScreen(),
                _ => _currentScreen
            };
            
            _currentScreen.Show();
        }

        public void Dispose()
        {
            _screenInterface = null;
        }
    }
}