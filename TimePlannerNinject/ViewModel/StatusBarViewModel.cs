// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusBarViewModel.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.ViewModel
{
   using GalaSoft.MvvmLight;
   using GalaSoft.MvvmLight.Messaging;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;

   /// <summary>
   ///    This class contains properties that a View can data bind to.
   ///    <para>
   ///       See http://www.galasoft.ch/mvvm
   ///    </para>
   /// </summary>
   public class StatusBarViewModel : ViewModelBase
   {
      #region Constants

      /// <summary>
      ///    Nom de la propriété <see cref="MessageToDisplay" />.
      /// </summary>
      public const string MessageToDisplayPropertyName = "MessageToDisplay";

      #endregion

      #region Fields

      /// <summary>
      ///    Message à afficher
      /// </summary>
      private string messageToDisplay = string.Empty;

      /// <summary>
      ///    Service de données
      /// </summary>
      private readonly ATimePlannerDataService service;

      #endregion

      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="StatusBarViewModel"/>. 
      /// Initializes a new instance of the StatusBarViewModel class.
      /// </summary>
      /// <param name="service">
      /// The service.
      /// </param>
      public StatusBarViewModel(ATimePlannerDataService service)
      {
         this.service = service;

         Messenger.Default.Register<string>(this, StatutMessage.Token, message => this.MessageToDisplay = message);
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient ou définit le message à afficher
      /// </summary>
      public string MessageToDisplay
      {
         get
         {
            return this.messageToDisplay;
         }

         set
         {
            this.Set(MessageToDisplayPropertyName, ref this.messageToDisplay, value);
         }
      }

      #endregion
   }
}