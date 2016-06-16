// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ATimePlannerDataService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Interfaces
{
   using System.Collections.Generic;
   using System.Collections.ObjectModel;

   using TimePlannerNinject.Model;

   /// <summary>
   ///    Classe abstraite du service de données du timeplanner.
   /// </summary>
   public abstract class ATimePlannerDataService
   {
      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="ATimePlannerDataService"/>.
      /// </summary>
      protected ATimePlannerDataService()
      {
         this.AllDays = new List<InputDay>();
         this.AllPlaces = new List<WorkPlace>();
      }

      #endregion

      #region Public Properties

      /// <summary>
      ///    Obtient ou définit les jours saisis.
      /// </summary>
      public List<InputDay> AllDays { get; set; }

      /// <summary>
      ///    Obtient ou définit les lieux de travail.
      /// </summary>
      public List<WorkPlace> AllPlaces { get; set; }

      #endregion

      #region Public Methods and Operators

      /// <summary>
      /// Lit les données a partir d'un fichier.
      /// </summary>
      /// <param name="filename">
      /// Le nom du fichier contenant les données.
      /// </param>
      public abstract void ReadDataFromFile(string filename);

      /// <summary>
      /// Sauvegarde les données dans un fichier.
      /// </summary>
      /// <param name="filename">
      /// Le nom du fichier.
      /// </param>
      /// <returns>
      /// True en cas de succès, false sinon.
      /// </returns>
      public abstract bool SaveDataToFile(string filename);

      #endregion
   }
}