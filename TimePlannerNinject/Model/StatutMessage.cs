// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatutMessage.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Defines the StatutMessage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    ///     Classe permettant la mise en place d'un message dans la barre de statut.
    /// </summary>
    public static class StatutMessage
    {
        #region Constants

        /// <summary>
        ///     Token de messenger MvvM light pour l'affichage d'un message dans la barre de statut.
        /// </summary>
        public const string Token = "tokenStatutMessage";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Met en place un message dans la barre de statut.
        /// </summary>
        /// <param name="message">
        ///     Message a afficher.
        /// </param>
        public static void SendStatutMessage(string message)
        {
            Messenger.Default.Send(message, Token);
        }

        #endregion
    }
}