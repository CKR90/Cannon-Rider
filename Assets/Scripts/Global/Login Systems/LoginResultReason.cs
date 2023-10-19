public enum LoginResultReason
{
    FailWhenStarterScene_PlayGames,
    FailWhenMenuScene_PlayGames,
    FailWhenStarterScene_Google,
    FailWhenMenuScene_Google,
    FailWhenStarterScene_Email,
    FailWhenMenuScene_Email,

    SuccessWhenStarterScene_PlayGames,
    SuccessWhenMenuScene_PlayGames,
    SuccessWhenStarterScene_Google,
    SuccessWhenMenuScene_Google,
    SuccessWhenStarterScene_Email,
    SuccessWhenMenuScene_Email,

    LoginMethodIsNone,

    TryingPlayGamesAuthOnComputer,
    TryingGoogleAuthOnComputer,

}
