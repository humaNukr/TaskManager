using Android.App;
using Android.Runtime;

namespace KMA.TaskManager.Maui.Platforms.Android
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(nint handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => KMA.TaskManager.Maui.MauiProgram.CreateMauiApp();
    }
}
