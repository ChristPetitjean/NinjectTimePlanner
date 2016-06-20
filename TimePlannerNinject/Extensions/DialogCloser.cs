// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogCloser.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Transforme la propriété DialogResult des window en une propriété Bindable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Extensions
{
   using System.Windows;

   /// <summary>
   ///    Transforme la propriété DialogResult des window en une propriété Bindable.
   /// </summary>
   public class DialogCloser
   {
      #region Public Methods and Operators

      /// <summary>
      /// Obtient la propriété DialogResult.
      /// </summary>
      /// <param name="element">
      /// Fenêtre en cours.
      /// </param>
      /// <returns>
      /// La valeur de la propriété DialogResult.
      /// </returns>
      public static bool? GetDialogResult(Window element)
      {
         return (bool?)element.GetValue(DialogResultProperty);
      }

      /// <summary>
      /// Définit la propriété DialogResult.
      /// </summary>
      /// <param name="element">
      /// Fen^étre en cours.
      /// </param>
      /// <param name="value">
      /// La valeur à affectée à la propriété DialogResult.
      /// </param>
      public static void SetDialogResult(Window element, bool value)
      {
         element.SetValue(DialogResultProperty, value);
      }

      #endregion

      #region Methods

      /// <summary>
      /// Evènement de changement de la propriété DialogResult.
      /// </summary>
      /// <param name="d">
      /// La propriété de dépendance DialogResult.
      /// </param>
      /// <param name="e">
      /// Le <see cref="DependencyPropertyChangedEventArgs"/> correspondant.
      /// </param>
      private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var window = d as WindowView;
         if (window != null)
         {
            var result = e.NewValue as bool?;
            if (result.HasValue)
            {
               window.DialogResult = result;
               window.Close();
            }
         }
      }

      #endregion

      /// <summary>
      ///    La propriété DialogResult.
      /// </summary>
      public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
         "DialogResult", 
         typeof(bool?), 
         typeof(DialogCloser), 
         new PropertyMetadata(DialogResultChanged));
   }
}