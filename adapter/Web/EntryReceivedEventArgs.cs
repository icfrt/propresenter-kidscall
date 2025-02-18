namespace Icf.ProPresenter.KidsCall.Web
{
    class EntryReceivedEventArgs : EventArgs
    {
        public EntryReceivedEventArgs(EntryModel model)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public EntryModel Model { get; }
    }
}
