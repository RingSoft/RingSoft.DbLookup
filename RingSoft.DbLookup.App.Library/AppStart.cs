using System.Linq;
using System.Threading;

namespace RingSoft.DbLookup.App.Library
{
    public interface IAppSplashWindow
    {
        void SetProgress(string progressText);

        void CloseSplash();

        bool IsDisposed { get; }

        bool Disposing { get; }
    }

    public abstract class AppStart
    {
        public abstract IAppSplashWindow AppSplashWindow { get; }

        private Thread _splashThread;
        private object _lockCloseWindow = new object();

        public virtual void StartApp(string[] args)
        {
            RsDbLookupAppGlobals.Initialize();

            if (args.Contains("-devLogix"))
            {
                ShowDevLogix();
            }
            else
            {
                InitializeSplash();

                _splashThread = new Thread(ShowSplash);
                _splashThread.SetApartmentState(ApartmentState.STA);
                _splashThread.IsBackground = true;
                _splashThread.Start();

                RsDbLookupAppGlobals.AppStartProgress += (sender, progressArgs) =>
                {
                    AppSplashWindow.SetProgress(progressArgs.ProgressText);
                };

                FinishStartup();
            }
        }

        protected void OnMainWindowLoad()
        {
            if (AppSplashWindow != null && !AppSplashWindow.Disposing && !AppSplashWindow.IsDisposed)
            {
                Monitor.Enter(_lockCloseWindow);
                try
                {
                    //Calling Close method is cleaner than setting the flag.  Otherwise startup decryption is screwing up the splash thread and making it hang on shut down.
                    //m_frmSplash.m_bCloseForm = true;
                    AppSplashWindow.CloseSplash();
                }
                finally
                {
                    Monitor.Exit(_lockCloseWindow);
                }
                while (_splashThread.IsAlive)
                    Thread.Sleep(500);

                _splashThread = null;	// we don't need it any more.
            }

        }

        protected abstract void InitializeSplash();

        protected abstract void ShowSplash();

        protected abstract void ShowDevLogix();

        protected abstract void FinishStartup();
    }
}
