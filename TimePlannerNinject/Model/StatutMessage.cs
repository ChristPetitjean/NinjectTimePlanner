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
    /// The statut message.
    /// </summary>
    public static class StatutMessage
   {
        /// <summary>
        /// The token.
        /// </summary>
        public const string Token = "tokenStatutMessage";

        /// <summary>
        /// The send statut message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void SendStatutMessage(string message)
      {
         Messenger.Default.Send(message, Token);
      }
   }
}
