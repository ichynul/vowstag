namespace Tag.Vows.Interface
{
    interface ITestAble
    {
        void SetTest(string test);
        void SetContent(string content);
        void CheckTestContent();
        string GetCode();
    }
}
