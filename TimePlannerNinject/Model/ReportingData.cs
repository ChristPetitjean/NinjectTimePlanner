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
    using System;

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
            this.EndDate = i.WorkEndTime?.ToString("hh:mm") ?? string.Empty;
            this.OneKilometers = w.OneWayKilometers;
            this.StartDate = i.WorkStartTime?.ToString("hh:mm") ?? string.Empty;
            this.WorkPlaceName = w.Name;
            this.Day = i.WorkStartTime?.ToString("dddd") ?? string.Empty;
            this.Month = i.WorkStartTime?.ToString("MMMM") ?? string.Empty;
            this.NumberOfDay = i.WorkStartTime?.Day ?? -1;
            this.NumberOfMonth = i.WorkStartTime?.Month ?? -1;
            this.Year = i.WorkStartTime?.Year ?? -1;
            this.ShortDate = i.WorkStartTime?.ToString("dddd dd MMMM") ?? string.Empty;
            this.WorkTime = (i.WorkEndTime?.TimeOfDay - i.WorkStartTime?.TimeOfDay);
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

        /// <summary>
        /// Obtient ou définit le nom du jour
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// Obtient ou définit le n° du jour
        /// </summary>
        public int NumberOfDay { get; set; }

        /// <summary>
        /// Obtient ou définit le n° du mois
        /// </summary>
        public int NumberOfMonth { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du mois
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Obtient ou définit l'année
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Obtient ou définit la date courte
        /// </summary>
        public string ShortDate { get; set; }
        
        /// <summary>
        /// Obtient ou définit le nombre d'heure effectuée
        /// </summary>
        public TimeSpan? WorkTime { get; set; }
        #endregion
    }
}