// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePlannerDataService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Model
{
   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.IO;
   using System.Linq;
   using System.Xml.Serialization;

   using TimePlannerNinject.Interfaces;

   /// <summary>
   /// The time planner data service.
   /// </summary>
   public class TimePlannerDataService : ATimePlannerDataService
   {
      /// <summary>
      /// Lit les données a partir d'un fichier.
      /// </summary>
      /// <param name="filename">Le nom du fichier contenant les données.</param>
      public override void ReadDataFromFile(string filename)
      {
         try
         {
            using (StreamReader fileStream = new StreamReader(filename))
            {
               XmlSerializer serializer = new XmlSerializer(typeof(AppFile));
               AppFile appFile = (AppFile)serializer.Deserialize(fileStream);

               this.AllDays = new List<InputDay>(appFile.Inputdays ?? new InputDay[0]);
               this.AllPlaces = new List<WorkPlace>(appFile.Worplaces ?? new WorkPlace[0]);
            }
         }
         catch
         {
            this.AllDays = new List<InputDay>();
            this.AllPlaces = new List<WorkPlace>();
         }
      }

      /// <summary>
      /// Sauvegarde les données dans un fichier.
      /// </summary>
      /// <param name="filename">Le nom du fichier.</param>
      /// <returns>
      /// True en cas de succès, false sinon.
      /// </returns>
      public override bool SaveDataToFile(string filename)
      {
         try
         {
            using (StreamWriter fileStream = new StreamWriter(filename))
            {
               AppFile appFile = new AppFile { Inputdays = this.AllDays.ToArray(), Worplaces = this.AllPlaces.ToArray() };
               XmlSerializer serializer = new XmlSerializer(typeof(AppFile));
               serializer.Serialize(fileStream, appFile);
            }

            return true;
         }
         catch
         {
            return false;
         }
      }
   }
}