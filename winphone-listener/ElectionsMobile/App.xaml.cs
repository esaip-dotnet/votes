using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ElectionsMobile.Resources;
using ElectionsMobile.ViewModels;

namespace ElectionsMobile
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        /// <summary>
        /// Static ViewModel used by the views with which to link to.
        /// </summary>
        /// <returns>Objet MainViewModel.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delaying the creation of the model view as necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// Easy access to the root frame of the phone application.
        /// </summary>
        /// <returns>Frame root of the phone application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global Manager for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XML initialization
            InitializeComponent();

            //Specific initialization phone
            InitializePhoneApplication();

            // Initializing of the display language
            InitializeLanguage();

            // Display graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display current frequency counters frames.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

               
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to run when a contract activation such as file open or save file selector returns
        // With the selected file or other return values
        private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)
        {
        }
        // Code to execute when the application starts (eg from Start)
        // This code does not run when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (displayed in the foreground)
        // This code does not run when the application is started for the first time
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Make sure the status of the application is properly restore
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Code to execute when the application is deactivated (sent to the background)
        // This code does not run when the application is closed
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            
        }

        // Code to run when closing the application (for example, when the user clicks Back)
        // This code does not run when the application is disabled
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code à exécuter en cas d'échec d'une navigation
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                
                Debugger.Break();
            }
        }

        // Code to run on unhandled exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception occurred; stop in the debugger
                Debugger.Break();
            }
        }

        #region Initialisation de l'application téléphonique

        // Éviter l'initialisation double
        private bool phoneApplicationInitialized = false;

        // Ne pas ajouter de code supplémentaire à cette méthode
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Créez le frame, mais ne le définissez pas encore comme RootVisual ; cela permet à l'écran de
            // démarrage de rester actif jusqu'à ce que l'application soit prête pour le rendu.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Gérer les erreurs de navigation
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Gérer les requêtes de réinitialisation pour effacer la pile arrière
            RootFrame.Navigated += CheckForResetNavigation;

            // Gérer une activation de contrat telle qu’une ouverture de fichier ou un sélecteur d’enregistrement de fichier
            PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;

            // Garantir de ne pas retenter l'initialisation
            phoneApplicationInitialized = true;
        }

        // Ne pas ajouter de code supplémentaire à cette méthode
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Définir le Visual racine pour permettre à l'application d'effectuer le rendu
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Supprimer ce gestionnaire, puisqu'il est devenu inutile
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // Si l'application a reçu une navigation de « réinitialisation », nous devons vérifier
            // sur la navigation suivante pour voir si la pile de la page doit être réinitialisée
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Désinscrire l'événement pour qu'il ne soit plus appelé
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Effacer uniquement la pile des « nouvelles » navigations (avant) et des actualisations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // Pour une interface utilisateur cohérente, effacez toute la pile de la page
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // ne rien faire
            }
        }

        #endregion

        
        private void InitializeLanguage()
        {
            try
            {
                
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

               
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}