namespace RingSoft.DbLookup.Controls.WPF
{
    public enum HostTypes
    {
        Tab = 1,
        Window = 2,
    }
    public interface IUserControlHost
    {
        HostTypes HostType { get; }

        void CloseHost();

        void ChangeTitle(string title);
    }
}
