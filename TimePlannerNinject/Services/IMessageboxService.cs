// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageboxService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Service permettant l'affichage d'une messagebox
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Services
{
   using TimePlannerNinject.Extensions;

   /// <summary>
   ///    Service permettant l'affichage d'une messagebox
   /// </summary>
   public interface IMessageboxService
   {
      #region Public Methods and Operators

      /// <summary>
      /// Affiche une MessageBox bloquante.
      /// </summary>
      /// <param name="message">
      /// Message à afficher
      /// </param>
      /// <param name="messageboxKind">
      /// Type de MessageBox
      /// </param>
      /// <param name="title">
      /// Titre de la MessageBox
      /// </param>
      /// <returns>
      /// Bouton cliqué par l'utilisateur
      /// </returns>
      MessageboxResponse ShowMessagebox(string message, MessageboxKind messageboxKind, string title = null);

      #endregion
   }
}