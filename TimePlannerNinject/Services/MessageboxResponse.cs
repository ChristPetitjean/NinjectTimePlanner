// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageboxResponse.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Réprésente le résultat utilisateur de la MessageBox
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Extensions
{
   /// <summary>
   ///    Réprésente le résultat utilisateur de la MessageBox
   /// </summary>
   public enum MessageboxResponse
   {
      /// <summary>
      ///    Click sur le bouton non
      /// </summary>
      None = 0, 

      /// <summary>
      ///    Click sur le bouton Ok
      /// </summary>
      Ok, 

      /// <summary>
      ///    Click sur le bouton annuler
      /// </summary>
      Cancel, 

      /// <summary>
      ///    Click sur le bouton oui
      /// </summary>
      Yes, 

      /// <summary>
      ///    Click sur le bouton non
      /// </summary>
      No
   }
}