using System.Linq;
using System.Threading;

namespace RingSoft.DbLookup.App.Library
{
    public interface IAppSplashWindow
    {
        void SetProgress(string progressText);
    }

    public abstract class AppStart
    {
        public abstract IAppSplashWindow AppSplashWindow { get; }

        public Thread SplashThread { get; private set; }

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

                SplashThread = new Thread(ShowSplash);
                SplashThread.SetApartmentState(ApartmentState.STA);
                SplashThread.IsBackground = true;
                SplashThread.Start();

                RsDbLookupAppGlobals.AppStartProgress += (sender, progressArgs) =>
                {
                    AppSplashWindow.SetProgress(progressArgs.ProgressText);
                };

                FinishStartup();
            }
        }
        
        protected abstract void InitializeSplash();

        protected abstract void ShowSplash();

        protected abstract void ShowDevLogix();

        protected abstract void FinishStartup();
    }
}
