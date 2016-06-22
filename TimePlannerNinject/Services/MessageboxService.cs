// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageboxService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Service permettant l'affichage de MessageBox en MvvM pur.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Services
{
   using System;
   using System.Windows;

   using TimePlannerNinject.Extensions;

   /// <summary>
   ///    Service permettant l'affichage de MessageBox en MvvM pur.
   /// </summary>
   public class MessageboxService : IMessageboxService
   {
      #region Public Methods and Operators

      /// <summary>
      ///    Affiche une MessageBox bloquante.
      /// </summary>
      /// <param name="message">Message à afficher</param>
      /// <param name="messageboxKind">Type de MessageBox</param>
      /// <param name="title">Titre de la MessageBox</param>
      /// <returns>
      ///    Bouton cliqué par l'utilisateur
      /// </returns>
      /// <inheritdoc />
      public MessageboxResponse ShowMessagebox(string message, MessageboxKind messageboxKind, string title = null)
      {
         var result = MessageBox.Show(message, title, GetButtonFromMessageBoxKind(messageboxKind));
         return GetMessageboxResponceFromResult(result);
      }

      #endregion

      #region Methods

      /// <summary>
      /// Converti un <see cref="MessageboxKind"/> en sont équivalent <see cref="MessageBoxButton"/>.
      /// </summary>
      /// <param name="messageboxKind">
      /// Type de messagebox
      /// </param>
      /// <returns>
      /// L'équivalence en <see cref="MessageBoxButton"/>
      /// </returns>
      private static MessageBoxButton GetButtonFromMessageBoxKind(MessageboxKind messageboxKind)
      {
         switch (messageboxKind)
         {
            case MessageboxKind.Ok:
               return MessageBoxButton.OK;

            case MessageboxKind.OkCancel:
               return MessageBoxButton.OKCancel;

            case MessageboxKind.YesNo:
               return MessageBoxButton.YesNo;

            case MessageboxKind.YesNoCancel:
               return MessageBoxButton.YesNoCancel;

            default:
               throw new ArgumentException($"Unsupported message box kind '{messageboxKind}'", messageboxKind.ToString());
         }
      }

      /// <summary>
      /// Obtient le resultat du choix utilisateur
      /// </summary>
      /// <param name="messageboxResult">
      /// Le résultat.
      /// </param>
      /// <returns>
      /// Le résutlat correspondant
      /// </returns>
      /// <exception cref="ArgumentException">
      /// Le résutlat n'est pas attendu
      /// </exception>
      private static MessageboxResponse GetMessageboxResponceFromResult(MessageBoxResult messageboxResult)
      {
         switch (messageboxResult)
         {
            case MessageBoxResult.Cancel:
               return MessageboxResponse.Cancel;

            case MessageBoxResult.No:
               return MessageboxResponse.No;

            case MessageBoxResult.None:
               return MessageboxResponse.None;

            case MessageBoxResult.OK:
               return MessageboxResponse.Ok;

            case MessageBoxResult.Yes:
               return MessageboxResponse.Yes;

            default:
               throw new ArgumentException(string.Format("Unsupported message box result '{0}'", messageboxResult), messageboxResult.ToString());
         }
      }

      #endregion
   }
}