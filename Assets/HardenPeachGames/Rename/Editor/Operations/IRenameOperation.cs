namespace HardenPeachGames.Renamer
{

    public interface IRenameOperation
    {
        string Rename(string input, int relativeCount, bool includeDiff);
    }
}