// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportingData.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   Données de reporting.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
    /// <summary>
    ///     Données de reporting.
    /// </summary>
    public class ReportingData
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="ReportingData" />.
        /// </summary>
        public ReportingData(InputDay i, WorkPlace w)
        {
            this.Id = i.ID;
            this.ReturnKilometers = w.ReturnKilometers;
            this.ExtraHours = i.ExtraHours ?? 0;
            this.EndDate = i.WorkEndTime?.ToString("d") ?? string.Empty;
            this.OneKilometers = w.OneWayKilometers;
            this.StartDate = i.WorkStartTime?.ToString("d") ?? string.Empty;
            this.WorkPlaceName = w.Name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Obtient ou définit la date de fin
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Obtient ou définit les heures supplémentaires
        /// </summary>
        public int ExtraHours { get; set; }
        
        /// <summary>
        /// Obtient ou définit l'identifiant
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre de kilomètre allé
        /// </summary>
        public decimal OneKilometers { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre de kilomètre retour
        /// </summary>
        public decimal ReturnKilometers { get; set; }

        /// <summary>
        /// Obtient ou définit la date dedébut
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du lieu de travail
        /// </summary>
        public string WorkPlaceName { get; set; }

        #endregion
    }
}