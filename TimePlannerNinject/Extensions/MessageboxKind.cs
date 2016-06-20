// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageboxKind.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Type de message box à afficher
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
   /// <summary>
   ///    Type de message box à afficher
   /// </summary>
   public enum MessageboxKind
   {
      /// <summary>
      ///    Seulement un bouton Ok.
      /// </summary>
      Ok = 0, 

      /// <summary>
      ///    Les boutons Ok et Annuler.
      /// </summary>
      OkCancel, 

      /// <summary>
      ///    Boutons oui, non et annuler.
      /// </summary>
      YesNoCancel, 

      /// <summary>
      ///    Boutons oui et non.
      /// </summary>
      YesNo
   }
}