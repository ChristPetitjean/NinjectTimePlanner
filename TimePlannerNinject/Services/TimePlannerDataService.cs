// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePlannerDataService.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Service de récupération des données.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Services
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using TimePlannerNinject.Model;

    /// <summary>
    ///     Service de récupération des données.
    /// </summary>
    public class TimePlannerDataService : ATimePlannerDataService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Lit les données a partir d'un fichier.
        /// </summary>
        /// <param name="filename">
        ///     Le nom du fichier contenant les données.
        /// </param>
        public override void ReadDataFromFile(string filename)
        {
            try
            {
                using (var fileStream = new StreamReader(filename))
                {
                    var serializer = new XmlSerializer(typeof(AppFile));
                    var appFile = (AppFile)serializer.Deserialize(fileStream);

                    this.AllDays = new ObservableCollection<InputDay>(appFile.Inputdays ?? new InputDay[0]);
                    this.AllPlaces = new ObservableCollection<WorkPlace>(appFile.Worplaces ?? new WorkPlace[0]);

                    base.ReadDataFromFile(filename);
                }
            }
            catch
            {
                this.AllDays = new ObservableCollection<InputDay>();
                this.AllPlaces = new ObservableCollection<WorkPlace>();
            }
        }

        /// <summary>
        ///     Sauvegarde les données dans un fichier.
        /// </summary>
        /// <param name="filename">
        ///     Le nom du fichier.
        /// </param>
        /// <returns>
        ///     True en cas de succès, false sinon.
        /// </returns>
        public override bool SaveDataToFile(string filename)
        {
            try
            {
                using (var fileStream = new StreamWriter(filename))
                {
                    var appFile = new AppFile { Inputdays = this.AllDays.ToArray(), Worplaces = this.AllPlaces.ToArray() };
                    var serializer = new XmlSerializer(typeof(AppFile));
                    serializer.Serialize(fileStream, appFile);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}