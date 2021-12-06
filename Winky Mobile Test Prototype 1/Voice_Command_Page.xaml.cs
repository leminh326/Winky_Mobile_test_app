using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI.Core;
using Windows.UI.Xaml.Automation.Peers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Xaml.Documents;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Winky_Mobile_Test_Prototype_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Voice_Command_Page : Page
    {
        private static uint HResultPrivacyStatementDeclined = 0x80045509;
        public static MainPage Current;
        private SpeechRecognizer speechRecognizer;
        private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;
        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;
        private MainPage rootPage = MainPage.Current;
        public Voice_Command_Page()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            if (permissionGained)
            {
                Language speechLanguage = SpeechRecognizer.SystemSpeechLanguage;
                speechContext = ResourceContext.GetForCurrentView();
                speechContext.Languages = new string[] { speechLanguage.LanguageTag };

                speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationSpeechResources");

                PopulateLanguageDropdown();
                await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
            }
            else
            {
                Result_Tbox.Text = "Permission to access capture resources was not given by the user; please set the application setting in Settings->Privacy->Microphone.";
            }
        }

        private void PopulateLanguageDropdown()
        {
            Language defaultLanguage = SpeechRecognizer.SystemSpeechLanguage;
            IEnumerable<Language> supportedLanguages = SpeechRecognizer.SupportedTopicLanguages;
            foreach (Language lang in supportedLanguages)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Tag = lang;
                item.Content = lang.DisplayName;

                Language_Selection_CBox.Items.Add(item);
                if (lang.LanguageTag == defaultLanguage.LanguageTag)
                {
                    item.IsSelected = true;
                    Language_Selection_CBox.SelectedItem = item;
                }
            }
        }

        private async Task InitializeRecognizer(Language recognizerLanguage)
        {
            if (speechRecognizer != null)
            {
                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            // Create an instance of SpeechRecognizer.
            speechRecognizer = new SpeechRecognizer(recognizerLanguage);

            // Compile the dictation topic constraint, which optimizes for dictated speech.
            var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
            speechRecognizer.Constraints.Add(dictationConstraint);
            SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

            // RecognizeWithUIAsync allows developers to customize the prompts.    
            speechRecognizer.UIOptions.AudiblePrompt = "Dictate a phrase or sentence...";

            // Check to make sure that the constraints were in a proper format and the recognizer was able to compile it.
            if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
            {
                // Let the user know that the grammar didn't compile properly.
                Result_Tbox.Text = "Unable to compile grammar.";
            }
        }

        private async void Start_Command_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                recognitionOperation = speechRecognizer.RecognizeAsync();
                SpeechRecognitionResult speechRecognitionResult = await recognitionOperation;
                // If successful, display the recognition result.
                if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    Result_Tbox.Text = speechRecognitionResult.Text;
                }
                else
                {
                    Result_Tbox.Text = string.Format("Speech Recognition Failed, Status: {0}", speechRecognitionResult.Status.ToString());
                }
            }
            catch (TaskCanceledException exception)
            {
                System.Diagnostics.Debug.WriteLine("TaskCanceledException caught while recognition in progress (can be ignored):");
                System.Diagnostics.Debug.WriteLine(exception.ToString());
            }
            catch (Exception exception)
            {
                // Handle the speech privacy policy error.
                if ((uint)exception.HResult == HResultPrivacyStatementDeclined)
                {
                }
                else
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog(exception.Message, "Exception");
                    await messageDialog.ShowAsync();
                }
            }
        }

        private async void Language_Selection_CBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (speechRecognizer != null)
            {
                ComboBoxItem item = (ComboBoxItem)(Language_Selection_CBox.SelectedItem);
                Language newLanguage = (Language)item.Tag;
                if (speechRecognizer.CurrentLanguage != newLanguage)
                {
                    // trigger cleanup and re-initialization of speech.
                    try
                    {
                        speechContext.Languages = new string[] { newLanguage.LanguageTag };

                        await InitializeRecognizer(newLanguage);
                    }
                    catch (Exception exception)
                    {
                        var messageDialog = new Windows.UI.Popups.MessageDialog(exception.Message, "Exception");
                        await messageDialog.ShowAsync();
                    }
                }
            }
        }

        private void Result_Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine(Result_Tbox.Text);
            switch (Result_Tbox.Text)
            {
                case "Go ahead":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text ;
                    rootPage.Motor_Power_Command(2, 50);
                    break;
                case "Go back":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text;
                    rootPage.Motor_Power_Command(2, -50);
                    break;
                case "Stop":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text;
                    rootPage.Motor_Power_Command(2, 0);
                    break;
                case "Turn left":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text;
                    rootPage.Motor_Power_Command(0, 50);
                    rootPage.Motor_Power_Command(1, 70);
                    break;
                case "Turn right":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text;
                    rootPage.Motor_Power_Command(0, 70);
                    rootPage.Motor_Power_Command(1, 50);
                    break;
                case "Turn around":
                    Result_Tbox.Text = "Command received : " + Result_Tbox.Text;
                    rootPage.Motor_Power_Command(0, 0);
                    rootPage.Motor_Power_Command(1, 50);
                    break;
                default:
                    
                    break;
            }
        }
    }
}
