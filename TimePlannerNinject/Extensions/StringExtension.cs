namespace TimePlannerNinject.Extensions
{
   using System.Linq;
   using System.Text;
   using System.Text.RegularExpressions;
   using System.Threading;

   /// <summary>
   ///    Contient les méthodes d'extension de la classe string
   /// </summary>
   public static class StringExtension
   {
      #region Public Methods and Operators

      /// <summary>
      ///    Transforme une phrase en PascalCase.
      /// </summary>
      /// <param name="s">La chaine</param>
      /// <returns>La chaine modifiée</returns>
      public static string ToPascalCase(this string s)
      {
         var result = new StringBuilder();
         var nonWordChars = new Regex(@"[^a-zA-Z0-9]+");
         var tokens = nonWordChars.Split(s);
         foreach (var token in tokens)
         {
            result.Append(PascalCaseSingleWord(token));
            result.Append(" ");
         }

         return result.ToString();
      }

      #endregion

      #region Methods

      /// <summary>
      ///    Pascalise un seul mot.
      /// </summary>
      /// <param name="s">La chaine a pascalisée.</param>
      /// <returns>La chaine pascalisée</returns>
      private static string PascalCaseSingleWord(string s)
      {
         var match = Regex.Match(s, @"^(?<word>\d+|^[a-z]+|[A-Z]+|[A-Z][a-z]+|\d[a-z]+)+$");
         var groups = match.Groups["word"];

         var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
         var result = new StringBuilder();
         foreach (var capture in groups.Captures.Cast<Capture>())
         {
            result.Append(textInfo.ToTitleCase(capture.Value.ToLower()));
         }
         return result.ToString();
      }

      #endregion
   }
}