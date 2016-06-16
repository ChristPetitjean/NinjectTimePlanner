// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePlannerDataServiceDesign.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Design
{
   using System;
   using System.Collections.Generic;
   using System.Windows.Media;

   using TimePlannerNinject.Interfaces;
   using TimePlannerNinject.Model;

   /// <summary>
   ///    The time planner data service design.
   /// </summary>
   public class TimePlannerDataServiceDesign : ATimePlannerDataService
   {
      #region Constructors and Destructors

      /// <summary>
      /// Initialise une nouvelle instance de la classe <see cref="TimePlannerDataServiceDesign"/>.
      /// </summary>
      public TimePlannerDataServiceDesign()
      {
         this.AllPlaces = new List<WorkPlace>
                             {
                                new WorkPlace
                                   {
                                      Color = Colors.Aqua, 
                                      DefaultEndTime = new DateTime(1, 1, 1, 18, 0, 0), 
                                      DefaultStartTime = new DateTime(1, 1, 1, 8, 0, 0), 
                                      Id = 1, 
                                      Kilometers = 20, 
                                      Name = "test1"
                                   }, 
                                new WorkPlace
                                   {
                                      Color = Colors.BlueViolet, 
                                      DefaultEndTime = new DateTime(1, 1, 1, 17, 0, 0), 
                                      DefaultStartTime = new DateTime(1, 1, 1, 9, 0, 0), 
                                      Id = 2, 
                                      Kilometers = 10, 
                                      Name = "test2"
                                   }
                             };
         this.AllDays = new List<InputDay>
                           {
                              new InputDay
                                 {
                                    ID = 1,
                                    ExtraHours = 1, 
                                    IdWorkPlace = 1, 
                                    IsWorked = true, 
                                    WorkEndTime = DateTime.Today.AddHours(18), 
                                    WorkStartTime = DateTime.Today.AddHours(8).AddDays(1)
                                 }
                           };
      }

      #endregion

      #region Public Methods and Operators

      /// <summary>
      /// Lit les données a partir d'un fichier.
      /// </summary>
      /// <param name="filename">
      /// Le nom du fichier contenant les données.
      /// </param>
      public override void ReadDataFromFile(string filename)
      {
         this.AllDays = new List<InputDay>
                           {
                              new InputDay
                                 {
                                    ExtraHours = 1, 
                                    IdWorkPlace = 1, 
                                    IsWorked = true, 
                                    WorkEndTime = DateTime.Today.AddHours(18), 
                                    WorkStartTime = DateTime.Today.AddHours(8)
                                 }
                           };
         this.AllPlaces = new List<WorkPlace>
                             {
                                new WorkPlace
                                   {
                                      Color = Colors.Aqua, 
                                      DefaultEndTime = new DateTime(1, 1, 1, 18, 0, 0), 
                                      DefaultStartTime = new DateTime(1, 1, 1, 8, 0, 0), 
                                      Id = 1, 
                                      Kilometers = 20, 
                                      Name = "test1"
                                   }, 
                                new WorkPlace
                                   {
                                      Color = Colors.BlueViolet, 
                                      DefaultEndTime = new DateTime(1, 1, 1, 17, 0, 0), 
                                      DefaultStartTime = new DateTime(1, 1, 1, 9, 0, 0), 
                                      Id = 2, 
                                      Kilometers = 10, 
                                      Name = "test2"
                                   }
                             };
      }

      /// <summary>
      /// Sauvegarde les données dans un fichier.
      /// </summary>
      /// <param name="filename">
      /// Le nom du fichier.
      /// </param>
      /// <returns>
      /// True en cas de succès, false sinon.
      /// </returns>
      public override bool SaveDataToFile(string filename)
      {
         return true;
      }

      #endregion
   }
}