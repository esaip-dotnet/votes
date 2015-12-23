using ElectionsMobile.Resources;

namespace ElectionsMobile
{
    /// <summary>
    /// Provides access to the string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}